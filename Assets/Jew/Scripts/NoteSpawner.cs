using UnityEngine;
using System.Collections.Generic;

public class NoteSpawner : MonoBehaviour
{
    public GameObject notePrefab;
    public Transform[] spawnPoints;
    public string[] noteMessages;
    public Sprite[] noteSprites;

    private List<Transform> availableSpawnPoints;

    private void Start()
    {
        availableSpawnPoints = new List<Transform>(spawnPoints);
        SpawnNotes();
    }

    public void SpawnNotes()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < noteMessages.Length; i++)
        {
            if (availableSpawnPoints.Count == 0) break;

            int randomIndex = Random.Range(0, availableSpawnPoints.Count);
            Transform spawnPoint = availableSpawnPoints[randomIndex];

            GameObject newNote = Instantiate(notePrefab, spawnPoint.position, Quaternion.identity);
            newNote.transform.SetParent(transform);

            Note noteScript = newNote.GetComponent<Note>();
            string randomMessage = noteMessages[Random.Range(0, noteMessages.Length)];
            noteScript.SetNoteDetails(randomMessage, noteSprites[Random.Range(0, noteSprites.Length)]);

            availableSpawnPoints.RemoveAt(randomIndex);

            NoteCollector.Instance.AddNoteMessage(randomMessage);
        }
    }
}
