using UnityEngine;
using UnityEngine.AI;

public class GhostAI : MonoBehaviour
{
    public Transform[] patrolPoints;
    public Transform[] respawnPoints;
    public float chaseRange = 10f;
    public float disappearRange = 20f;
    public float fearIncreaseRate = 5f;
    public float fearMax = 100f;
    public float stunForce = 10f;
    public float detectionRange = 15f;

    private int currentPatrolIndex = 0;
    private NavMeshAgent agent;
    private Transform player;
    private Rigidbody rb;
    private bool isChasing = false;
    private bool isStunned = false;
    private float fearLevel = 0f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent is missing on Ghost!", gameObject);
            return;
        }

        MoveToNextPatrolPoint();
    }

    void Update()
    {
        if (isStunned || agent == null || player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= chaseRange)
        {
            isChasing = true;
            fearLevel += fearIncreaseRate * Time.deltaTime;
            agent.SetDestination(player.position);
        }
        else if (distanceToPlayer >= disappearRange)
        {
            RespawnGhost();
        }
        else
        {
            isChasing = false;
            Patrol();
        }

        fearLevel = Mathf.Clamp(fearLevel, 0, fearMax);
    }

    void Patrol()
    {
        if (agent == null || patrolPoints.Length == 0) return;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
            MoveToNextPatrolPoint();
        }
    }

    void MoveToNextPatrolPoint()
    {
        if (patrolPoints.Length > 0 && agent.isOnNavMesh)
        {
            agent.SetDestination(patrolPoints[currentPatrolIndex].position);
        }
    }

    void RespawnGhost()
    {
        if (respawnPoints.Length == 0)
        {
            Debug.LogError("No respawn points assigned!", gameObject);
            return;
        }

        int randomIndex = Random.Range(0, respawnPoints.Length);
        transform.position = respawnPoints[randomIndex].position;
        fearLevel = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            fearLevel = fearMax;
        }
    }

    public void Stun(Vector3 forceDirection)
    {
        if (rb == null) return;

        isStunned = true;
        if (agent != null) agent.enabled = false;

        rb.isKinematic = false;
        rb.AddForce(forceDirection.normalized * stunForce, ForceMode.Impulse);
        Invoke("RecoverFromStun", 2f);
    }

    void RecoverFromStun()
    {
        isStunned = false;
        if (rb != null) rb.isKinematic = true;
        if (agent != null)
        {
            agent.enabled = true;
            MoveToNextPatrolPoint();
        }
    }
}