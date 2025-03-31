using UnityEngine;

public class Note : MonoBehaviour
{
    private bool playerNearby = false;
    public float detectionRadius = 2f;

    private void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius);
        playerNearby = false;

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                playerNearby = true;
                break;
            }
        }

        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            NoteCollector.Instance.CollectNote();
            Destroy(gameObject);
        }
    }
}