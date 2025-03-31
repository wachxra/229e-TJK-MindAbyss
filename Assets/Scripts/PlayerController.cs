using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 3f;
    public float sprintSpeed = 6f;
    public float crouchSpeed = 1.5f;
    private float moveSpeed;
    private bool isSprinting;
    private bool isCrouching;
    private bool canSprint = true;
    private bool sprintKeyReleased = true;
    private Vector3 moveDirection;

    [Header("Stamina Settings")]
    public float stamina = 100f;
    public float maxStamina = 100f;
    public float staminaDrain = 10f;
    public float staminaRegen = 5f;
    public Slider staminaBar;

    [Header("Fear Settings")]
    public float fear = 0f;
    public float maxFear = 100f;
    public float fearIncreaseRate = 5f;
    public float fearDecreaseRate = 2f;
    public Slider fearBar;

    [Header("Parry Settings")]
    public float parryDistance = 2f;
    public float parryWindow = 0.5f;
    private float lastParryTime = 0f;

    private bool isOnStairs;
    private Vector3 stairNormal;

    private Rigidbody rb;
    private Camera playerCamera;
    public float crouchHeight = 0.5f;
    public float standHeight = 2f;
    private float currentHeight;

    private CapsuleCollider playerCollider;
    private CharacterController controller;
    public Transform ghost;

    void Start()
    {
        moveSpeed = walkSpeed;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        playerCamera = GetComponentInChildren<Camera>();
        playerCollider = GetComponent<CapsuleCollider>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentHeight = standHeight;
        playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, standHeight, playerCamera.transform.localPosition.z);
    }

    void Update()
    {
        MovePlayer();
        HandleStamina();
        HandleFear();
        Crouching();
        Parry();
    }

    private void FixedUpdate()
    {
        rb.angularVelocity = Vector3.zero;
    }

    void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        moveDirection = transform.right * moveX + transform.forward * moveZ;
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.deltaTime);

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            sprintKeyReleased = true;
        }

        if (Input.GetKey(KeyCode.LeftShift) && stamina > 0 && canSprint && sprintKeyReleased)
        {
            moveSpeed = sprintSpeed;
            isSprinting = true;
        }
        else
        {
            moveSpeed = walkSpeed;
            isSprinting = false;
        }
    }

    void Crouching()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && !isCrouching)
        {
            CrouchDown();
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl) && isCrouching)
        {
            StandUp();
        }
    }

    void CrouchDown()
    {
        playerCollider.height = crouchHeight;
        playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, crouchHeight, playerCamera.transform.localPosition.z);
        isCrouching = true;
        moveSpeed = crouchSpeed;
    }

    void StandUp()
    {
        playerCollider.height = standHeight;
        playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, standHeight, playerCamera.transform.localPosition.z);
        isCrouching = false;
        moveSpeed = walkSpeed;
    }

    void HandleStamina()
    {
        if (isSprinting)
        {
            stamina -= staminaDrain * Time.deltaTime;
            if (stamina <= 0)
            {
                stamina = 0;
                canSprint = false;
                moveSpeed = walkSpeed;
            }
        }
        else if (stamina < maxStamina)
        {
            stamina += staminaRegen * Time.deltaTime;
            if (stamina > maxStamina * 0.5f)
            {
                canSprint = true;
            }
        }

        stamina = Mathf.Clamp(stamina, 0, maxStamina);
        staminaBar.value = stamina / maxStamina;
    }

    void HandleFear()
    {
        fear = Mathf.Clamp(fear, 0, maxFear);
        fearBar.value = fear / maxFear;

        if (fear >= maxFear)
        {
            GameManager.Instance.GameOver();
        }
    }

    void Parry()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Time.time - lastParryTime <= parryWindow && Vector3.Distance(transform.position, ghost.position) <= parryDistance)
            {
                if (stamina >= 10f)
                {
                    PerformParry();
                }
            }
            else
            {
                stamina -= 10f;
                stamina = Mathf.Clamp(stamina, 0, maxStamina);
            }
            lastParryTime = Time.time;
        }
    }

    void PerformParry()
    {
        stamina -= 10f;
        stamina = Mathf.Clamp(stamina, 0, maxStamina);

        GhostAI ghostAI = ghost.GetComponent<GhostAI>();
        if (ghostAI != null)
        {
            Vector3 impactDirection = (ghost.position - transform.position).normalized;
            float parryForce = 15f;
            ghostAI.Stun(parryForce, impactDirection);
        }
    }
}