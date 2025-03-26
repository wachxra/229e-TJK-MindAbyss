using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public AudioSource backgroundMusic;
    public AudioSource heartbeatSound;

    public float maxHeartbeatVolume = 1f;
    public float minHeartbeatVolume = 0f;

    public float backgroundVolume = 1f;

    public Transform player;
    public GhostAI ghost;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadSettings();
    }

    void Update()
    {
        HandleAudio();
    }

    void HandleAudio()
    {
        float distance = Vector3.Distance(player.position, ghost.transform.position);
        if (distance < ghost.detectionRange)
        {
            backgroundMusic.volume = Mathf.Lerp(backgroundMusic.volume, 0f, Time.deltaTime * 2);
            heartbeatSound.volume = Mathf.Lerp(heartbeatSound.volume, maxHeartbeatVolume, Time.deltaTime * 2);
        }
        else
        {
            backgroundMusic.volume = Mathf.Lerp(backgroundMusic.volume, backgroundVolume, Time.deltaTime * 2);
            heartbeatSound.volume = Mathf.Lerp(heartbeatSound.volume, minHeartbeatVolume, Time.deltaTime * 2);
        }
    }

    public void SetBackgroundVolume(float volume)
    {
        backgroundVolume = Mathf.Clamp01(volume);
        backgroundMusic.volume = backgroundVolume;
        PlayerPrefs.SetFloat("BackgroundVolume", backgroundVolume);
        PlayerPrefs.Save();
    }

    void LoadSettings()
    {
        backgroundVolume = PlayerPrefs.GetFloat("BackgroundVolume", 1f);
        if (backgroundMusic != null)
        {
            backgroundMusic.volume = backgroundVolume;
        }
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}