using UnityEngine;
using UnityEngine.UI;

public class NoteCollector : MonoBehaviour
{
    public int totalNotes = 10;
    public int collectedNotes = 0;
    public Text noteUI;

    void Start()
    {
        UpdateNoteUI();
    }

    void UpdateNoteUI()
    {
        noteUI.text = "Notes: " + collectedNotes + "/" + totalNotes;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Note"))
        {
            collectedNotes++;
            Destroy(other.gameObject);
            UpdateNoteUI();
        }
    }
}