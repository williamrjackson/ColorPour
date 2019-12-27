using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public enum DifficultyLevel { Easy, Medium, Hard };

    public Color targetColor;
    public SpriteRenderer targetColorSprite;
    [Tooltip("How close you need to get the blended color to the target")]
    public float requiredDistance = .25f;
    public Transform successCanvas;
    public Transform failCanvas;
    public DifficultyLevel difficultyLevel;
    private bool _bannerLatch;
    private bool _winState;
    private bool _failState;

    // Singleton behavior
    public static GameManager instance;
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

    void Update()
    {
        // If a click is received after winning or losing, reload the scene.
        if ((WinState || FailState) && Input.GetMouseButtonDown(0) && !_bannerLatch)
        {
            SceneManager.LoadScene(0);
        }
    }

    // Select a suitable random color and start the game
    public void StartGameWithRandomTarget()
    {
        // Easy mode gets a color with max saturation, meaning only two colors are required to achieve the target
        float saturationMin = 1f;
        // Medium yields 50%-100% saturation.
        if (difficultyLevel == DifficultyLevel.Medium) saturationMin = .5f;
        // Hard produces a color within the full range
        else if (difficultyLevel == DifficultyLevel.Hard) saturationMin = 0f;

        // Get random color
        targetColor = Random.ColorHSV(0f, 1f, saturationMin, 1f, 1f, 1f);
        // Set the reference block to the target color
        targetColorSprite.color = targetColor;

        // Hide UI canvases
        successCanvas.gameObject.SetActive(false);
        failCanvas.gameObject.SetActive(false);
        _winState = false;
        _failState = false;
    }

    // public getter for the target luminance.
    public float TargetLuminance
    {
        private set { }
        get
        {
            float H;
            float S;
            float V;
            Color.RGBToHSV(targetColor, out H, out S, out V);
            return V;
        }
    }

    // Checks to see if a given color is close to the target color 
    public bool CheckForWin(Color currentColor)
    {
        float distance = Liquid.ColorDistance(currentColor, targetColor);

        // If so, WIN!
        if (distance < requiredDistance)
        {
            successCanvas.gameObject.SetActive(true);
            _winState = true;
            StartCoroutine(BannerLatch());
        }
        return _winState;
    }

    // Prevent accidental immediate dismissal of win/gameover banners
    private IEnumerator BannerLatch()
    {
        _bannerLatch = true;
        yield return new WaitForSeconds(1f);
        _bannerLatch = false;
    }

    public bool WinState
    {
        private set { }
        get
        {
            return _winState;
        }
    }
    public bool FailState
    {
        set
        {
            _failState = true;
            StartCoroutine(BannerLatch());
            failCanvas.gameObject.SetActive(true);
        }
        get
        {
            return _failState;
        }
    }
}
