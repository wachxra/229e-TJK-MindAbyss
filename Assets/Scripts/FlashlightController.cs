using UnityEngine;
using UnityEngine.UI;

public class FlashlightController : MonoBehaviour
{
    [Header("Flashlight Settings")]
    public Light flashlight;
    public bool startFlashlightOn = false;
    public AudioSource flashlightSound;

    [Header("Battery Settings")]
    public float battery = 100f;
    public float maxBattery = 100f;
    public float batteryDrainRate = 5f;
    public float batteryRegenRate = 2f;
    public Slider batteryBar;

    private bool isFlashlightOn;
    private bool canTurnOn = true;

    void Start()
    {
        isFlashlightOn = startFlashlightOn && battery > 0;
        flashlight.enabled = isFlashlightOn;
        UpdateBatteryUI();
    }

    void Update()
    {
        HandleFlashlight();
    }

    void HandleFlashlight()
    {
        if (Input.GetKeyDown(KeyCode.F) && canTurnOn)
        {
            ToggleFlashlight();
        }

        if (isFlashlightOn && battery > 0)
        {
            battery -= batteryDrainRate * Time.deltaTime;
        }
        else if (!isFlashlightOn && battery < maxBattery)
        {
            battery += batteryRegenRate * Time.deltaTime;
        }

        battery = Mathf.Clamp(battery, 0, maxBattery);
        UpdateBatteryUI();

        if (battery <= 0 && isFlashlightOn)
        {
            isFlashlightOn = false;
            flashlight.enabled = false;
            canTurnOn = false;
        }

        if (battery > 5f)
        {
            canTurnOn = true;
        }
    }

    public void ToggleFlashlight()
    {
        if (battery > 0)
        {
            isFlashlightOn = !isFlashlightOn;
            flashlight.enabled = isFlashlightOn;

            if (flashlightSound != null)
            {
                flashlightSound.Play();
            }
        }
    }

    void UpdateBatteryUI()
    {
        if (batteryBar != null)
        {
            batteryBar.value = battery / maxBattery;
        }
    }

    public void CollectBattery(float amount)
    {
        battery += amount;
        battery = Mathf.Clamp(battery, 0, maxBattery);
        UpdateBatteryUI();

        if (battery > 5f)
        {
            canTurnOn = true;
        }
    }
}