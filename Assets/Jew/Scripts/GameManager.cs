using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Audio Settings")]
    public AudioSource backgroundMusic;
    public AudioSource heartbeatSound;
    public AudioSource notePickupSound;
    public AudioSource unlockDoorSound;
    public AudioSource winSound;

    public float maxHeartbeatVolume = 1f;
    public float minHeartbeatVolume = 0f;
    public float backgroundVolume = 1f;

    [Header("Game Elements")]
    public Transform player;
    public GhostAI ghost;
    public GameObject exitDoor;
    public TextMeshProUGUI notificationText;

    private bool isExitUnlocked = false;
    private bool isGameOver = false;

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

        if (backgroundMusic != null && backgroundMusic.transform != null)
        {
            Vector3 position = backgroundMusic.transform.position;
        }
        else if (heartbeatSound != null && heartbeatSound.transform != null)
        {
            Vector3 position = heartbeatSound.transform.position;
        }
        else
        {
            return;
        }

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

    public void UnlockExit()
    {
        isExitUnlocked = true;
        if (exitDoor != null)
        {
            exitDoor.SetActive(false);
        }
        unlockDoorSound.Play();
        ShowNotification("The exit is now unlocked!");
    }

    void ShowNotification(string message)
    {
        notificationText.text = message;
        notificationText.gameObject.SetActive(true);
        Invoke(nameof(HideNotification), 3f);
    }

    void HideNotification()
    {
        notificationText.gameObject.SetActive(false);
    }

    public void WinGame()
    {
        if (!isExitUnlocked)
        {
            return;
        }

        winSound.Play();
        EndGame();
    }

    public void GameOver()
    {
        isGameOver = true;
        EndGame();
    }

    public void EndGame()
    {
        if (isExitUnlocked || isGameOver)
        {
            SceneManager.LoadScene("EndGame");
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}