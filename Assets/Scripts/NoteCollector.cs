using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NoteCollector : MonoBehaviour
{
    public static NoteCollector Instance;

    [Header("Note System")]
    public int totalNotes = 10;
    public int collectedNotes = 0;
    public TextMeshProUGUI notesCollectorUI;
    public TextMeshProUGUI showingNoteTextUI;

    [Header("Note UI")]
    public GameObject notePanel;
    private bool isReadingNote = false;
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
        collectedNotes++;
        UpdateNotesUI();

        ShowNote(currentNoteText);

        if (collectedNotes >= totalNotes)
        {
            GameManager.Instance.UnlockExit();
        }
    }

    void UpdateNotesUI()
    {
        notesCollectorUI.text = $"Notes: {collectedNotes}/{totalNotes}";
    }

    public void ShowNote(string noteText)
    {
        isReadingNote = true;
        notePanel.SetActive(true);
        showingNoteTextUI.text = noteText;
    }

    public void CloseNote()
    {
        isReadingNote = false;
        notePanel.SetActive(false);
    }

    public void AddNoteMessage(string message)
    {
        currentNoteText = message;
    }

    private void Update()
    {
        if (!isReadingNote && Input.GetKeyDown(KeyCode.E))
        {
            ShowNote(currentNoteText);
        }
        else if (isReadingNote && Input.GetKeyDown(KeyCode.E))
        {
            CloseNote();
        }
    }
}