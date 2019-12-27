using UnityEngine;

public enum LiquidBase { Red, Green, Blue }
public class Liquid : MonoBehaviour
{
    private float targetY;
    private SpriteRenderer sprite;

    // Singleton behavior
    public static Liquid instance;
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

    // Get the distance between 2 colors. Regular vector math works!?
    // TODO: Make this a Color extension method in WRJ.Utils if it proves reliable.
    public static float ColorDistance(Color color1, Color color2)
    {
        return Vector3.Distance(new Vector3(color1.r, color1.g, color1.b), new Vector3(color2.r, color2.g, color2.b));
    }

    public Color CurrentBlendColor
    {
        get
        {
            return sprite.color;
        }
    }
    void Start()
    {
        targetY = transform.localScale.y;
        sprite = GetComponent<SpriteRenderer>();
        GameManager.instance.StartGameWithRandomTarget();
    }

    // Keep track of each color. Also remember previous values to compare and prevent unnecessary work in Update()
    private int redCount = 0;
    private int greenCount = 0;
    private int blueCount = 0;
    private int cachedRedCount = 0;
    private int cachedGreenCount = 0;
    private int cachedBlueCount = 0;

    void Update()
    {
        // Only if the water level has risen
        if (transform.localScale.y < targetY)
        {
            // Increase water height
            Vector3 scale = transform.localScale;
            scale.y = transform.localScale.y + .025f;
            transform.localScale = scale;

            // Shift up accordingly
            Vector3 pos = transform.localPosition;
            pos.y = transform.localScale.y * .5f;
            transform.localPosition = pos;
        }

        // Only if new color has been added
        if (redCount != cachedRedCount || greenCount != cachedGreenCount || blueCount != cachedBlueCount)
        {
            // update cache values
            cachedRedCount = redCount;
            cachedBlueCount = blueCount;
            cachedGreenCount = greenCount;

            // Sum all droplets
            int totalDropletCount = redCount + greenCount + blueCount;

            // Game over if it overflows
            if (totalDropletCount > 70)
            {
                GameManager.instance.FailState = true;
                return;
            }

            // calculate new color...
            Color balancedColor = new Color(
                Mathf.InverseLerp(0f, totalDropletCount, redCount),
                Mathf.InverseLerp(0f, totalDropletCount, greenCount),
                Mathf.InverseLerp(0f, totalDropletCount, blueCount)
            );

            // Match luminance to target. This mixing method can't produce all colors, so I force it.
            // Considering making this a "light value" (or something) that the player can adjust.
            float H;
            float S;
            float V;
            Color.RGBToHSV(balancedColor, out H, out S, out V);

            V = GameManager.instance.TargetLuminance;
            balancedColor = Color.HSVToRGB(H, S, V);
            if (GameManager.instance.CheckForWin(balancedColor))
            {
                sprite.color = GameManager.instance.targetColor;
            }
            sprite.color = balancedColor;
        }
    }

    // Add in new color to he liquid basin
    public void Add(LiquidBase liquidBase)
    {
        if (GameManager.instance.WinState || GameManager.instance.FailState)
        {
            return;
        }

        // Increase water level 5% per drop. Adjustment made in Update().
        targetY += .05f;

        // Log new color contribution. Also applied in Update().
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
