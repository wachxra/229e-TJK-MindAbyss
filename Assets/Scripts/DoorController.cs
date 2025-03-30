using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public List<Transform> doors;
    public float openSpeed = 2f;
    public float closedPositionY = 0f;
    public float openPositionY = 5f;
    public KeyCode openCloseKey = KeyCode.E;
    public float interactionRange = 3f;
    public Camera fpsCamera;

    private Dictionary<Transform, bool> doorStates = new Dictionary<Transform, bool>();

    void Start()
    {
        foreach (var door in doors)
        {
            if (door != null)
            {
                doorStates[door] = false;
            }
        }
    }

    void Update()
    {
        if (fpsCamera == null || doors == null || doors.Count == 0)
        {
            return;
        }

        foreach (var door in doors)
        {
            if (door == null) continue;

            float distance = Vector3.Distance(door.position, fpsCamera.transform.position);

            if (distance <= interactionRange && Input.GetKeyDown(openCloseKey))
            {
                if (doorStates[door])
                {
                    CloseDoor(door);
                }
                else
                {
                    OpenDoor(door);
                }
            }
        }
    }

    void OpenDoor(Transform door)
    {
        StopAllCoroutines();
        StartCoroutine(MoveDoor(door, openPositionY));
        doorStates[door] = true;
    }

    void CloseDoor(Transform door)
    {
        StopAllCoroutines();
        StartCoroutine(MoveDoor(door, closedPositionY));
        doorStates[door] = false;
    }

    IEnumerator MoveDoor(Transform door, float targetY)
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
