using UnityEngine;
using System.Collections.Generic;

public class NoteSpawner : MonoBehaviour
{
    public GameObject notePrefab;
    public Transform[] spawnPoints;

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

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (availableSpawnPoints.Count == 0) break;

            int randomIndex = Random.Range(0, availableSpawnPoints.Count);
            Transform spawnPoint = availableSpawnPoints[randomIndex];

            GameObject newNote = Instantiate(notePrefab, spawnPoint.position, Quaternion.identity);
            newNote.transform.SetParent(transform);

            availableSpawnPoints.RemoveAt(randomIndex);
        }
    }
}