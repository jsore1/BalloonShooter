using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour
{
    public GameObject scoreText; // текущий счет, который отображается в процессе игры
    public GameObject gameOverScore; // текущий счет, который отображается при проигрыше
    public GameObject gameOverBestScore; // лучший счёт, который отображается при проигрыше
    public GameObject menu; // главное меню
    public GameObject startButton; // кнопка старта игры
    public GameObject player; // объект прицела
    public GameObject playButton; // кнопка запуска игры
    public GameObject pauseButton; // кнопка паузы
    public GameObject gameOverMenu; // меню конца игры
    public GameObject menuButton; // кнопка меню
    public GameObject restartButton; // кнопка рестарт
    public GameObject shareButton; // кнопка поделиться
    public GameObject soundButtonOn;
    public GameObject soundButtonOff;
    //public string ScreenshotName = "screenshot.png"; // название скриншота
    //public string url = ""; // адрес для метода "поделиться"
    public GameObject fadeImage; // объект для затемнения экрана
    public AimScript aimScript;
    public int score = 0; // переменная текущего счета
    
    private bool is_started = false; // вначале игровой процесс отключен
    private bool isPaused = false; // внчале пауза отключена
    private Vector3 playerPosition; // переменная для начальной позиции прицела
    private int bestScore; // переменная для лучшего счета
    private bool fade; // переменная для затемнения экрана
    private float alphaColor; // переменная для альфа-канала

    void Start()
    {

        aimScript = GameObject.FindGameObjectWithTag("Aim").GetComponent<AimScript>();

        Screen.sleepTimeout = SleepTimeout.NeverSleep; // Делаем так, чтобы экран не гас во время игры
        playerPosition = player.transform.position; // запоминаем позицию прицела

        // Обработчик кнопки Start
        startButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            fade = true; // включить затемнение экрана
            aimScript.CalibrateAccelerometer();
            Invoke("startGame", 1f); // задержка перед запуском игрового процесса
        });

        // Обработчик кнопки Продолжить игру
        playButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            isPaused = false; // выключить паузу
            playButton.SetActive(false); // скрыть кнопку запуска игры
            pauseButton.SetActive(true); // показать кнопку паузы
            Time.timeScale = 1; // продолжить игровой процесс
            player.SetActive(true); // показать прицел
        });

        // Обработчик кнопки приостановить игру
        pauseButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            isPaused = true; // включить паузу
        });

        // Обработчик кнопки меню
        menuButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            gameOverMenu.SetActive(false); // скрыть экран проигрыша
            menu.SetActive(true); // показать главное меню
        });

        // Обработчик кнопки рестарт
        restartButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            fade = true; // включить затемнение экрана
            aimScript.CalibrateAccelerometer();
            Invoke("restartGame", 1f); // задержка перед запуском игрового процесса
        });

        // Обработчик кнопки поделиться
        //shareButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        //{
        //    ShareScreenshotWithText("Hello, world");
        //});

        soundButtonOff.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            soundButtonOff.SetActive(false);
            soundButtonOn.SetActive(true);
            AudioListener.volume = 1.0f;
        });

        soundButtonOn.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() =>
        {
            soundButtonOn.SetActive(false);
            soundButtonOff.SetActive(true);
            AudioListener.volume = 0.0f;
        });
    }

    public void startGame()
    {
        is_started = true; // запустить игровой процесс
        menu.SetActive(false); // скрыть главное меню
        player.SetActive(true); // показать прицел
        player.transform.position = playerPosition; // поставить прицел в начальную позицию
    }

    public void restartGame()
    {
        is_started = true; // запустить игровой процесс
        gameOverMenu.SetActive(false); // скрыть экран проигрыша
        player.SetActive(true); // сделать прицел активным
        player.transform.position = playerPosition; // поставить прицел в начальную позицию
    }

    public void stopGame()
    {
        if (PlayerPrefs.HasKey("bestScore"))
        {
            if (score > PlayerPrefs.GetInt("bestScore"))
            {
                bestScore = score;
                PlayerPrefs.SetInt("bestScore", bestScore);
                PlayerPrefs.Save();
                gameOverBestScore.GetComponent<UnityEngine.UI.Text>().text = "";
                gameOverBestScore.GetComponent<UnityEngine.UI.Text>().text = "" + bestScore;
            }
        } else
        {
            if (score > bestScore)
            {
                bestScore = score;
                PlayerPrefs.SetInt("bestScore", bestScore);
                PlayerPrefs.Save();
                gameOverBestScore.GetComponent<UnityEngine.UI.Text>().text = "";
                gameOverBestScore.GetComponent<UnityEngine.UI.Text>().text = "" + bestScore;
            }
        }

        is_started = false;
        gameOverBestScore.GetComponent<UnityEngine.UI.Text>().text = "";
        gameOverBestScore.GetComponent<UnityEngine.UI.Text>().text = "" + PlayerPrefs.GetInt("bestScore");
        gameOverScore.GetComponent<UnityEngine.UI.Text>().text = "";
        gameOverScore.GetComponent<UnityEngine.UI.Text>().text = "" + score;
        gameOverMenu.SetActive(true);
        player.SetActive(false);
        GameObject[] Baloons = GameObject.FindGameObjectsWithTag("Baloon");
        foreach (GameObject baloon in Baloons)
        {
            Destroy(baloon);
        }
        score = 0;
        scoreText.GetComponent<UnityEngine.UI.Text>().text = "0";
    }

    public bool IsStarted()
    {
        return is_started;
    }

    public void IncreaseScore(int increment)
    {
        score += increment;
        scoreText.GetComponent<UnityEngine.UI.Text>().text = "" + score;
    }

    void OnGUI()
    {
        if (isPaused)
        {
            Time.timeScale = 0;
            playButton.SetActive(true);
            pauseButton.SetActive(false); // скрыть кнопку включения паузы
            player.SetActive(false);
        }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        //isPaused = !hasFocus;
        if (!hasFocus && is_started == true)
        {
            isPaused = true;
        }
    }

    void OnApplicationPause(bool pauseStatus)
    {
        isPaused = pauseStatus;
        if (!pauseStatus && is_started == true)
        {
            isPaused = true;
        }
    }

    //public void ShareScreenshotWithText(string text)
    //{
    //    ScreenCapture.CaptureScreenshot(ScreenshotName);
    //    StartCoroutine(DelayedShare(text, url));
    //}

    //private IEnumerator DelayedShare(string text, string urlApp)
    //{
    //    yield return new WaitForSeconds(0.25f);
    //    string screenShotPath = Application.persistentDataPath + "/" + ScreenshotName;
    //    Share(text, screenShotPath, urlApp);
    //}

    //private void Share(string shareText, string imagePath, string url, string subject = "")
    //{
    //    AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
    //    AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");

    //    intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
    //    AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
    //    AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + imagePath);
    //    intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
    //    intentObject.Call<AndroidJavaObject>("setType", "image/png");

    //    intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), shareText + "\n" + url);

    //    AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    //    AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

    //    AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, subject);
    //    currentActivity.Call("startActivity", jChooser);
    //}

    void Update()
    {
        if (fade)
        {
            alphaColor = Mathf.Lerp(fadeImage.GetComponent<UnityEngine.UI.Image>().color.a, 1, Time.deltaTime * 4f);
            fadeImage.GetComponent<UnityEngine.UI.Image>().color = new Color(0, 0, 0, alphaColor);
        }

        if (alphaColor > 0.99 && fade)
        {
            fadeImage.GetComponent<UnityEngine.UI.Image>().color = new Color(0, 0, 0, 0);
            fade = false;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && is_started == true)
        {
            isPaused = true;
        }
    }
}
