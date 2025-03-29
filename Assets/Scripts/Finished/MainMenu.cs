using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Sounds Settings")]
    public AudioSource backgroundMusic;
    public AudioSource clickSound;

    void Start()
    {
        if (backgroundMusic != null && !backgroundMusic.isPlaying)
        {
            backgroundMusic.loop = true;
            backgroundMusic.Play();
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
}