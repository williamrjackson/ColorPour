using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LiquidBase { Red, Green, Blue }
public class Liquid : MonoBehaviour
{
    public static Liquid instance;
    private float targetY;
    private SpriteRenderer sprite;

    public static Color BaseToColor(LiquidBase inColor)
    {
        if (inColor == LiquidBase.Blue)
        {
            return Color.blue;
        }
        else if (inColor == LiquidBase.Green)
        {
            return Color.green;
        }
        else
        {
            return Color.red;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    void Start()
    {
        targetY = transform.localScale.y;
        sprite = GetComponent<SpriteRenderer>();
        GameManager.instance.SetRandomTargetColor();
    }

    void Update()
    {
        if (transform.localScale.y < targetY)
        {
            transform.localScale = transform.localScale.With(y: transform.localScale.y + .01f);
            transform.localPosition = transform.localPosition.With(y: transform.localScale.y * .5f);
        }

        if (redCount != cachedRedCount || greenCount != cachedGreenCount || blueCount != cachedBlueCount)
        {
            int totalDropletCount = redCount + greenCount + blueCount;

            if (totalDropletCount > 82)
            {
                GameManager.instance.FailState = true;
            }
            // calculate color...
            Color determined = new Color(
                redCount.Remap(0f, totalDropletCount, 0f, 1f),
                greenCount.Remap(0f, totalDropletCount, 0f, 1f),
                blueCount.Remap(0f, totalDropletCount, 0f, 1f)
            );
            float H;
            float S;
            float V;
            Color.RGBToHSV(determined, out H, out S, out V);
            V = GameManager.instance.TargetLuminance;
            determined = Color.HSVToRGB(H, S, V);
            if (GameManager.instance.CheckForWin(determined))
            {
                sprite.color = GameManager.instance.targetColor;
            }
            sprite.color = determined;

            // update cache values
            cachedRedCount = redCount;
            cachedBlueCount = blueCount;
            cachedGreenCount = greenCount;
        }
    }

    private int redCount = 0;
    private int greenCount = 0;
    private int blueCount = 0;
    private int cachedRedCount = 0;
    private int cachedGreenCount = 0;
    private int cachedBlueCount = 0;

    // Update is called once per frame
    public void Add(LiquidBase liquidBase)
    {
        if (GameManager.instance.WinState || GameManager.instance.FailState)
        {
            return;
        }

        targetY += .05f;
        if (liquidBase == LiquidBase.Blue)
        {
            blueCount++;
        }
        else if (liquidBase == LiquidBase.Green)
        {
            greenCount++;
        }
        else
        {
            redCount++;
        }
    }
}
