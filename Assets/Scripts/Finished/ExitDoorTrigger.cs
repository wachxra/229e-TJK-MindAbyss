using UnityEngine;

public class ExitDoorTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && GameManager.Instance.isExitDoorUnlocked)
        {
            GameManager.Instance.EndGame();
        }
    }
}