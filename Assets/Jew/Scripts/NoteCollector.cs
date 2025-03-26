using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NoteCollector : MonoBehaviour
{
    public static NoteCollector Instance;

    [Header("Note System")]
    public int totalNotes = 10;
    private int collectedNotes = 0;
    public TextMeshProUGUI notesUI;

    [Header("Note UI")]
    public GameObject notePanel;
    public Image noteImage;
    private bool isReadingNote = false;
    private bool canCollectNote = false;

    public Sprite noteSprite;
    private Sprite currentNoteSprite;

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

    public void CollectNote(Sprite noteSprite)
    {
        if (canCollectNote)
        {
            collectedNotes++;
            UpdateNotesUI();

            ShowNote(currentNoteSprite);

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

    public void ShowNote(Sprite noteSprite)
    {
        isReadingNote = true;
        notePanel.SetActive(true);
        noteImage.sprite = noteSprite;
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
            CollectNote(noteSprite);
        }

        if (isReadingNote && Input.GetKeyDown(KeyCode.E))
        {
            CloseNote();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            canCollectNote = true;
            currentNoteSprite = other.gameObject.GetComponent<Note>().noteSprite;
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