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


    public static float ColorDistance(Color color1, Color color2)
    {
        return Vector3.Distance(new Vector3(color1.r, color1.g, color1.b), new Vector3(color2.r, color2.g, color2.b));
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

    private int redCount = 0;
    private int greenCount = 0;
    private int blueCount = 0;
    private int cachedRedCount = 0;
    private int cachedGreenCount = 0;
    private int cachedBlueCount = 0;

    void Update()
    {
        if (transform.localScale.y < targetY)
        {
            transform.localScale = transform.localScale.With(y: transform.localScale.y + .025f);
            transform.localPosition = transform.localPosition.With(y: transform.localScale.y * .5f);
        }

        if (redCount != cachedRedCount || greenCount != cachedGreenCount || blueCount != cachedBlueCount)
        {
            // update cache values
            cachedRedCount = redCount;
            cachedBlueCount = blueCount;
            cachedGreenCount = greenCount;

            int totalDropletCount = redCount + greenCount + blueCount;

            // Game over if it overflows
            if (totalDropletCount > 82)
            {
                GameManager.instance.FailState = true;
                return;
            }

            // calculate color...
            Color determined = new Color(
                Mathf.InverseLerp(0f, totalDropletCount, redCount),
                Mathf.InverseLerp(0f, totalDropletCount, greenCount),
                Mathf.InverseLerp(0f, totalDropletCount, blueCount)
            );

            // Match luminance to target. This mixing method can't produce all colors.
            // Considering making this a "light value" that the player can adjust.
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
        }
    }

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
