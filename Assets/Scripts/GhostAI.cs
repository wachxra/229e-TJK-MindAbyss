using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class GhostAI : MonoBehaviour
{
    [Header("Ghost Settings")]
    public Transform player;
    public float speed = 2f;
    public float chaseSpeed = 5f;
    public float detectionRange = 10f;
    public float fearIncrease = 10f;
    public float sightAngle = 60f;
    public float stunTime = 0f;

    [Header("Stun Settings")]
    public float torqueMultiplier = 5f;
    public float minStunDuration = 0.5f;
    public float maxStunDuration = 3f;
    public float maxForceImpact = 10f;
    public float stunForceDivider = 15f;
    public float recoveryDelay = 0.3f;
    public float drag = 0.5f;
    public float angularDrag = 1.0f;
    public float stunForce = 5f;

    [Header("Wandering Settings")]
    public float wanderRadius = 15f;
    public float wanderInterval = 3f;

    private bool isChasing;
    private bool isStunned;
    private Rigidbody rb;
    private NavMeshAgent agent;
    private Animator animator;
    private Collider ghostCollider;
    private float wanderTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        ghostCollider = GetComponent<Collider>();
        wanderTimer = wanderInterval;

        rb.linearDamping = drag;
        rb.angularDamping = angularDrag;
    }

    void Update()
    {
        if (stunTime > 0)
        {
            stunTime -= Time.deltaTime;
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (!isStunned && distance < detectionRange && CanSeePlayer())
        {
            isChasing = true;
            player.GetComponent<PlayerController>().fear += fearIncrease * Time.deltaTime;
        }
        else if (!isStunned)
        {
            isChasing = false;
        }

        if (isChasing)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else
        {
            Wander();
        }

        if (Input.GetKeyDown(KeyCode.Space) && distance < detectionRange)
        {
            Vector3 impactDirection = (transform.position - player.position).normalized;
            Stun(stunForce, impactDirection);
        }
    }

    void Wander()
    {
        wanderTimer -= Time.deltaTime;
        if (wanderTimer <= 0f || agent.remainingDistance < 1f)
        {
            Vector3 wanderTarget = GetRandomNavMeshPosition(transform.position, wanderRadius);
            agent.isStopped = false;
            agent.SetDestination(wanderTarget);
            wanderTimer = wanderInterval;
        }
    }

    Vector3 GetRandomNavMeshPosition(Vector3 origin, float radius)
    {
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * radius;
            randomDirection.y = 0;
            randomDirection += origin;
            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, NavMesh.AllAreas))
            {
                return hit.position;
            }
        }
        return origin;
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
                return hit.transform == player;
            }
        }
        return false;
    }

    public void Stun(float force, Vector3 impactDirection)
    {
        float ghostMass = rb.mass;
        float acceleration = force / ghostMass;
        float stunDuration = Mathf.Clamp(acceleration / stunForceDivider, minStunDuration, maxStunDuration);

        stunTime = stunDuration;
        isStunned = true;
        isChasing = false;
        agent.isStopped = true;
        animator.speed = 0f;
        ghostCollider.isTrigger = false;

        rb.isKinematic = false;

        Vector3 torqueAxis = Vector3.up;
        float torqueAmount = force * torqueMultiplier;
        rb.AddTorque(torqueAxis * torqueAmount, ForceMode.Impulse);

        StartCoroutine(RecoverFromStun(stunDuration));
    }

    IEnumerator RecoverFromStun(float duration)
    {
        yield return new WaitForSeconds(duration + recoveryDelay);

        isStunned = false;
        agent.isStopped = false;
        animator.speed = 1f;
        ghostCollider.isTrigger = true;
    }
}