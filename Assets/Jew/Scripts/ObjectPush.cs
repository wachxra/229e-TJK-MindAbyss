using UnityEngine;

public class ObjectPush : MonoBehaviour
{
    private Rigidbody targetRb;
    private Collider targetCollider;

    [Header("Physics Interaction")]
    public float pushForce = 10f;
    public float stunMultiplier = 0.2f;
    public LayerMask pushableLayer;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 2f, pushableLayer))
            {
                targetRb = hit.collider.GetComponent<Rigidbody>();
                targetCollider = hit.collider;
            }
        }

        if (Input.GetMouseButtonDown(0) && targetRb != null)
        {
            Vector3 forceDirection = targetCollider.transform.position - transform.position;
            forceDirection.Normalize();

            float force = pushForce * targetRb.mass;
            targetRb.AddForce(forceDirection * force, ForceMode.Impulse);

            if (targetCollider.CompareTag("Ghost"))
            {
                float stunDuration = force * stunMultiplier;
                targetCollider.GetComponent<GhostAI>().Stun(stunDuration);
            }

            targetRb = null;
            targetCollider = null;
        }
    }
}