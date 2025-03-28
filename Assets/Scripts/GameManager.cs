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
    public bool isExitDoorUnlocked { get; private set; } = false;
    private bool isGameOver = false;

    public int notesCollected = 0;
    public int totalNotes = 10;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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

    public void CollectNote()
    {
        notesCollected++;
        if (notesCollected >= totalNotes)
        {
            ShowNotification("Find the locked door and unlock it!");
            UnlockExit();
        }
    }

    void HandleDoorUnlocked()
    {
        if (isExitUnlocked && !isExitDoorUnlocked && Input.GetKeyDown(KeyCode.E))
        {
            isExitDoorUnlocked = true;
            exitDoor.SetActive(false);

            unlockDoorSound.Play();
            ShowNotification("The door is now unlocked! Enter to escape.");
        }
    }

    public void UnlockExit()
    {
        isExitUnlocked = true;
        if (exitDoor != null)
        {
            exitDoor.SetActive(false);
        }
        HandleDoorUnlocked();
    }

    public void GameOver()
    {
        if (!isGameOver)
        {
            isGameOver = true;
        }
        
        SceneManager.LoadScene("LoseScene");
    }

    public void EndGame()
    {
        if (isExitUnlocked && isExitDoorUnlocked)
        {
            SceneManager.LoadScene("WinScene");
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