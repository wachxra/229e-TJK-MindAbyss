using UnityEngine;

public class GravityChanger : MonoBehaviour
{
    public Transform gravityDirection;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerGravity = other.GetComponent<PlayerController>();
            if (playerGravity != null)
            {
                playerGravity.SetCustomGravity(-gravityDirection.up);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController playerGravity = other.GetComponent<PlayerController>();
            if (playerGravity != null)
            {
                playerGravity.ResetGravity();
            }
        }
    }
}
