using UnityEngine;

public class Note : MonoBehaviour
{
    [Header("Note Settings")]
    public Sprite noteSprite;
    public string noteText;

    private bool playerNearby = false;

    public void SetNoteDetails(string text, Sprite sprite)
    {
        noteText = text;
        noteSprite = sprite;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            NoteCollector.Instance.AddNoteMessage(noteText);
            playerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
        }
    }

    private void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            NoteCollector.Instance.CollectNote();
            Destroy(gameObject);
        }
    }
}