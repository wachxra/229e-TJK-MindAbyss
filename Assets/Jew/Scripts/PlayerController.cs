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

    private Rigidbody rb;
    private Camera playerCamera;
    public float crouchHeight = 0.5f;
    public float standHeight = 2f;
    private float currentHeight;

    private CapsuleCollider playerCollider;

    void Start()
    {
        moveSpeed = walkSpeed;
        rb = GetComponent<Rigidbody>();
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

        if (Input.GetKeyDown(KeyCode.LeftControl) && !isCrouching)
        {
            CrouchDown();
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl) && isCrouching)
        {
            StandUp();
        }
    }

    void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        moveDirection = transform.right * moveX + transform.forward * moveZ;
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.LeftShift) && stamina > 0)
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

    void CrouchDown()
    {
        // ��͵�Ƿѹ��
        playerCollider.height = crouchHeight;
        playerCamera.transform.localPosition = new Vector3(playerCamera.transform.localPosition.x, crouchHeight, playerCamera.transform.localPosition.z);
        isCrouching = true;
        moveSpeed = crouchSpeed;
    }

    void StandUp()
    {
        // ��Ѻ���׹�ѹ��
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
        }
        else if (stamina < maxStamina)
        {
            stamina += staminaRegen * Time.deltaTime;
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
}