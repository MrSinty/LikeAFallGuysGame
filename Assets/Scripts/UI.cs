using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private Color32 textColor = new Color32(255, 0, 0, 255);
    [SerializeField] private GameObject volumeSlider;
    [SerializeField] private GameObject volumeText;

    private PlayerHP playerHP;
    private GUIStyle style;


    private bool showWinScreen = false;
    private bool showLossScreen = false;
    private float timeAtStart;
    private float timeAtEnd;
    private string timerText;
    private string labelText;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        volumeSlider.SetActive(false);
        volumeText.SetActive(false);

        playerHP = playerObject.GetComponent<PlayerHP>();

        style = new GUIStyle { fontSize = 15 };
        style.normal.textColor = Color.black;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            QuitGame();
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleVolume();
        }
    }

    private void OnGUI()
    {
        GUI.contentColor = textColor;
        if (!(showLossScreen || showWinScreen))
        {
            GUI.Box(new Rect(20, 20, 150, 25), "Health Points: " + playerHP.PlayerHealth);
        }

        if (showWinScreen)
        {
            Cursor.visible = true;
            labelText = "Победа!";

            GUI.Label(new Rect(Screen.width / 2 - 40, 100, 100, 50), labelText, style);
            GUI.Label(new Rect(Screen.width / 2 - 80, 80, 100, 100), timerText, style);
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 100), "Еще раз!"))
            {
                RestartLevel();
            }
        }

        if (showLossScreen)
        {
            Cursor.visible = true;
            labelText = "Поражение!";

            GUI.Label(new Rect(Screen.width / 2 - 40, 100, 100, 50), labelText, style);
            GUI.Label(new Rect(Screen.width / 2 - 80, 80, 100, 100), timerText, style);
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 100),
                "Еще раз!"))
            {
                RestartLevel();
            }
        }
    }
    public void ShowScreen(bool winOrLose, float timeScale)
    {
        if (winOrLose)
        {
            showWinScreen = true;
        }
        else
        {
            showLossScreen = true;
        }

        Time.timeScale = timeScale;
    }

    public void StartTimer()
    {
        timeAtStart = Time.time;
        timeAtEnd = 0f;
    }

    public void EndTimer()
    {
        timeAtEnd = Time.time;
        TimeCalculate();
    }

    private void TimeCalculate()
    {
        string s, m;
        float elapsedTime = timeAtEnd - timeAtStart;

        int sec = (int)(elapsedTime % 60);
        int min = (int)(elapsedTime / 60);
        if (sec < 10) s = "0" + sec; else s = sec.ToString();
        if (min < 10) m = "0" + min; else m = min.ToString();

        timerText = "Время в игре: " + m + "min " + s + "sec";
    }

    public void AudioVolume(float sliderVolume)
    {
        audioMixer.SetFloat("masterVolume", sliderVolume);
    }

    private void ToggleVolume()
    {
        Cursor.visible = !Cursor.visible;
        volumeSlider.SetActive(!volumeSlider.active);
        volumeText.SetActive(!volumeText.active);
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1.0f;
    }

    private void QuitGame()
    {
        Application.Quit();
    }
}
