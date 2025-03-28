using System.Collections;
using UnityEngine;

public class GhostAI : MonoBehaviour
{
    public Transform player;
    public float speed = 2f;
    public float chaseSpeed = 5f;
    public float detectionRange = 10f;
    public float fearIncrease = 10f;
    public float sightAngle = 60f;
    public float stunTime = 0f;
    private bool isChasing;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (stunTime > 0)
        {
            stunTime -= Time.deltaTime;
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < detectionRange)
        {
            if (CanSeePlayer())
            {
                isChasing = true;
                player.GetComponent<PlayerController>().fear += fearIncrease * Time.deltaTime;
            }
            else
            {
                isChasing = false;
            }
        }
        else
        {
            isChasing = false;
        }

        if (isChasing)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            rb.MovePosition(transform.position + direction * chaseSpeed * Time.deltaTime);
        }
    }

    bool CanSeePlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

        if (angleToPlayer <= sightAngle / 2)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, directionToPlayer.normalized, out hit, detectionRange))
            {
                if (hit.transform == player)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void Stun(float force)
    {
        stunTime = Mathf.Clamp(force / 10, 1, 5);
    }
}