using UnityEngine;

public class ObjectPush : MonoBehaviour
{
    public float pushForce = 10f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    Vector3 forceDir = hit.point - transform.position;
                    forceDir.y = 0;
                    forceDir.Normalize();
                    rb.AddForce(forceDir * pushForce, ForceMode.Impulse);

                    GhostAI ghost = hit.collider.GetComponent<GhostAI>();
                    if (ghost != null)
                    {
                        ghost.Stun(pushForce);
                    }
                }
            }
        }
    }
}