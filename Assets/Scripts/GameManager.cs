using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager instance;

    public Color targetColor;
    public SpriteRenderer targetColorSprite;
    public float requiredDistance = .25f;
    public Transform successCanvas;
    public Transform failCanvas;
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
        targetColor = Random.ColorHSV(0f, 1f, 0f, 1f, .5f, 1f);
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
        float distance = Vector3.Distance(new Vector3(currentColor.r, currentColor.g, currentColor.b), new Vector3(targetColor.r, targetColor.g, targetColor.b));
        Debug.Log(currentColor.ToString() + " vs " + targetColor.ToString());
        Debug.Log(distance);
        if (distance < requiredDistance)
        {
            Debug.Log("WIN!!!");
            successCanvas.gameObject.SetActive(true);
            winState = true;
            return true;
        }
        return false;
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
