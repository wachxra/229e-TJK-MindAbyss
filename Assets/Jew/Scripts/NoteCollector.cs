using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NoteCollector : MonoBehaviour
{
    public static NoteCollector Instance;

    [Header("Note System")]
    public int totalNotes = 10;
    public int collectedNotes = 0;
    public TextMeshProUGUI notesUI;
    public TextMeshProUGUI currentNoteTextUI;

    [Header("Note UI")]
    public GameObject notePanel;
    private bool isReadingNote = false;
    private bool canCollectNote = false;

    private string currentNoteText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        notePanel.SetActive(false);
        UpdateNotesUI();
    }

    public void CollectNote()
    {
        if (canCollectNote)
        {
            collectedNotes++;
            UpdateNotesUI();

            ShowNote(currentNoteText);

            if (collectedNotes >= totalNotes)
            {
                GameManager.Instance.UnlockExit();
            }

            canCollectNote = false;
        }
    }

    void UpdateNotesUI()
    {
        notesUI.text = $"Notes: {collectedNotes}/{totalNotes}";
    }

    public void ShowNote(string noteText)
    {
        isReadingNote = true;
        notePanel.SetActive(true);
        currentNoteTextUI.text = noteText;
        Time.timeScale = 0;
    }

    public void CloseNote()
    {
        isReadingNote = false;
        notePanel.SetActive(false);
        Time.timeScale = 1;
    }

    void Update()
    {
        if (canCollectNote && Input.GetKeyDown(KeyCode.E))
        {
            CollectNote();
        }

        if (isReadingNote && Input.GetKeyDown(KeyCode.E))
        {
            CloseNote();
        }
    }

    public void AddNoteMessage(string message)
    {
        currentNoteText = message;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canCollectNote = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canCollectNote = false;
        }
    }
}
