using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Sounds Settings")]
    public AudioSource backgroundMusic;
    public AudioSource clickSound;

    private bool isInEndCreditScene = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (backgroundMusic != null && !backgroundMusic.isPlaying)
        {
            backgroundMusic.loop = true;
            backgroundMusic.Play();
        }

        if (SceneManager.GetActiveScene().name == "EndCredit")
        {
            isInEndCreditScene = true;
        }
    }

    void Update()
    {
        if (isInEndCreditScene && Input.anyKeyDown)
        {
            LoadMainMenu();
        }
    }

    public void PlayGame()
    {
        StartCoroutine(PlayButtonClickAndLoadScene("TestingScene"));
    }

    public void QuitGame()
    {
        if (clickSound != null)
        {
            clickSound.Play();
        }
        Application.Quit();
    }

    public void EndCredit()
    {
        StartCoroutine(PlayButtonClickAndLoadScene("EndCredit"));
    }

    private IEnumerator PlayButtonClickAndLoadScene(string sceneName)
    {
        if (clickSound != null)
        {
            clickSound.Play();
            yield return new WaitForSeconds(clickSound.clip.length);
        }
        SceneManager.LoadScene(sceneName);
    }

    private void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
