using UnityEngine;

public class PushableObject : MonoBehaviour
{
    [Header("Physics Settings")]
    public float objectMass = 5f;
    public float forceMultiplier = 10f;
    public float drag = 0.5f;
    public float angularDrag = 0.5f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.mass = objectMass;
        rb.linearDamping = drag;
        rb.angularDamping = angularDrag;

        gameObject.tag = "Pushable";
    }

    public void ApplyForce(Vector3 forceDirection)
    {
        float force = objectMass * forceMultiplier;
        rb.AddForce(forceDirection * force, ForceMode.Impulse);
    }
}
