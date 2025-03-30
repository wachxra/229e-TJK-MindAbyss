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
        UpdateNotesUI();
    }

    public void CollectNote()
    {
        collectedNotes++;
        UpdateNotesUI();

        if (collectedNotes >= totalNotes)
        {
            GameManager.Instance.UnlockExit();
        }
    }

    void UpdateNotesUI()
    {
        notesCollectorUI.text = $"Notes: {collectedNotes}/{totalNotes}";
    }
}