using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Sound theme;
    public GameManagerScript gameManager;
    [Header("Main Menu")]
    public GameObject mainMenu;
    public TextMeshProUGUI rewindCounter;
    [Header("Help Menu")]
    public GameObject helpMenu;
    public bool isPaused = false;


    private void Start()
    {
        if (theme != null)
        {
            FindObjectOfType<SoundManager>().Play(theme.name);

        }
        try
        {
            gameManager = FindObjectOfType<GameManagerScript>();

        } catch (System.Exception e)
        {
            Debug.LogWarning("game manager not found");
        }

    }
    public void toMainMenu()
    {
        SceneManager.LoadScene(0);

    }

    public void toHelpMenu()
    {
        SceneManager.LoadScene(1);
    }

    public void startLevel1()
    {
        SceneManager.LoadScene(2);
    }
    public void startLevel2()
    {
        SceneManager.LoadScene(3);
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void show_HelpMenu()
    {
        showCursor();
        if (!isPaused)
        {
            isPaused = true;
            helpMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    public void close_HelpMenu()
    {
        hideCursor();
        isPaused = false;
        helpMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void show_GameWin()
    {
        showCursor();

        isPaused = true;
        Time.timeScale = 0f;
        mainMenu.SetActive(true);
        displayRewindCounter();
    }

    void displayRewindCounter()
    {
        if (rewindCounter != null)
        {
            rewindCounter.text = gameManager.rewindCounter.ToString();
        }
    }
    void showCursor()
    {
        if (!Cursor.visible)
        {
            Cursor.visible = true;
        }
    }

    void hideCursor()
    {
        if (Cursor.visible)
        {
            Cursor.visible = false;
        }
    }

}
