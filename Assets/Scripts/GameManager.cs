using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;
    public enum DifficultyLevel { Easy, Medium, Hard };
    public Color targetColor;
    public SpriteRenderer targetColorSprite;
    public float requiredDistance = .25f;
    public Transform successCanvas;
    public Transform failCanvas;
    public DifficultyLevel difficultyLevel;
    private bool winState;
    private bool failState;

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
        if ((WinState || FailState) && Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(0);
        }
    }

    public void SetRandomTargetColor()
    {
        float saturationMin = 1f;
        if (difficultyLevel == DifficultyLevel.Medium) saturationMin = .5f;
        else if (difficultyLevel == DifficultyLevel.Hard) saturationMin = 0f;

        targetColor = Random.ColorHSV(0f, 1f, saturationMin, 1f, 1f, 1f);
        targetColorSprite.color = targetColor;
        successCanvas.gameObject.SetActive(false);
        failCanvas.gameObject.SetActive(false);
        winState = false;
        failState = false;
    }

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

    public bool CheckForWin(Color currentColor)
    {
        float distance = Liquid.ColorDistance(currentColor, targetColor);
        // Debug.Log(distance);
        if (distance < requiredDistance)
        {
            successCanvas.gameObject.SetActive(true);
            winState = true;
        }
        return winState;
    }

    public bool WinState
    {
        private set { }
        get
        {
            return winState;
        }
    }
    public bool FailState
    {
        set
        {
            failState = true;
            failCanvas.gameObject.SetActive(true);
        }
        get
        {
            return failState;
        }
    }
}
