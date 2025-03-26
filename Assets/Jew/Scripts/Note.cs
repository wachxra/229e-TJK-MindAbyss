using UnityEngine;

public class Note : MonoBehaviour
{
    [Header("Note Settings")]
    public Sprite noteSprite;
    public string noteText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            NoteCollector.Instance.CollectNote(noteSprite);
            Destroy(gameObject);
        }
    }
}