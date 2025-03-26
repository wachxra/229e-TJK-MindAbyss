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

    [Header("Fear System")]
    public float fear = 0f;
    public float maxFear = 100f;
    public float fearIncreaseRate = 5f;
    public float fearDecreaseRate = 2f;
    public Slider fearBar;

    [Header("Physics Interaction")]
    public float pushForce = 10f;
    public float stunMultiplier = 0.2f;
    public LayerMask pushableLayer;

    [Header("Item Collection")]
    public int notesCollected = 0;
    public int totalNotes = 10;
    public TextMeshProUGUI noteCollectorText;

    private Rigidbody rb;

    void Start()
    {
        moveSpeed = walkSpeed;
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        UpdateNoteUI();
    }

    void Update()
    {
        MovePlayer();
        HandleStamina();
        HandleFear();
        CheckForNotePickup();
        HandlePushObjects();
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

        if (Input.GetKey(KeyCode.LeftControl))
        {
            moveSpeed = crouchSpeed;
            isCrouching = true;
        }
        else
        {
            isCrouching = false;
        }
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

    void CheckForNotePickup()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 2f))
            {
                if (hit.collider.CompareTag("Note"))
                {
                    Destroy(hit.collider.gameObject);
                    notesCollected++;
                    UpdateNoteUI();

                    if (notesCollected >= totalNotes)
                    {
                        GameManager.Instance.UnlockExit();
                    }
                }
            }
        }
    }

    void UpdateNoteUI()
    {
        noteCollectorText.text = "Notes: " + notesCollected + "/" + totalNotes;
    }

    void HandlePushObjects()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 2f, pushableLayer))
            {
                Rigidbody objRb = hit.collider.GetComponent<Rigidbody>();
                if (objRb != null)
                {
                    Vector3 forceDirection = hit.point - transform.position;
                    forceDirection.Normalize();

                    float force = pushForce * rb.mass;
                    objRb.AddForce(forceDirection * force, ForceMode.Impulse);

                    if (hit.collider.CompareTag("Ghost"))
                    {
                        float stunDuration = force * stunMultiplier;
                        hit.collider.GetComponent<GhostAI>().Stun(stunDuration);
                    }
                }
            }
        }
    }
}