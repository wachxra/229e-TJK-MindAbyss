using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Transform door;
    public float openSpeed = 2f;
    public float closedPositionY = 0f;
    public float openPositionY = 5f;
    public KeyCode openCloseKey = KeyCode.E;
    public float interactionRange = 3f;

    public Camera fpsCamera;

    private bool isOpen = false;

    void Update()
    {
        if (fpsCamera == null)
        {
            Debug.LogError("FPS Camera is not assigned!");
            return;
        }

        if (door == null)
        {
            Debug.LogError("Door Transform is not assigned!");
            return;
        }

        float distance = Vector3.Distance(transform.position, fpsCamera.transform.position);

        if (distance <= interactionRange && Input.GetKeyDown(openCloseKey))
        {
            if (isOpen)
            {
                CloseDoor();
            }
            else
            {
                OpenDoor();
            }
        }
    }

    void OpenDoor()
    {
        StopAllCoroutines();
        StartCoroutine(MoveDoor(openPositionY));
        isOpen = true;
    }

    void CloseDoor()
    {
        StopAllCoroutines();
        StartCoroutine(MoveDoor(closedPositionY));
        isOpen = false;
    }

    IEnumerator MoveDoor(float targetY)
    {
        while (Mathf.Abs(door.position.y - targetY) > 0.01f)
        {
            float newY = Mathf.MoveTowards(door.position.y, targetY, Time.deltaTime * openSpeed);
            door.position = new Vector3(door.position.x, newY, door.position.z);
            yield return null;
        }
        door.position = new Vector3(door.position.x, targetY, door.position.z);
    }
}