using UnityEngine;
using UnityEngine.UI;

public class FlashlightController : MonoBehaviour
{
    [Header("Flashlight Settings")]
    public Light flashlight;
    public float battery = 100f;
    public float maxBattery = 100f;
    public float batteryDrainRate = 5f;
    public float batteryRegenRate = 2f;
    public Slider batteryBar;
    public AudioSource flashlightSound;

    private bool isFlashlightOn = false;

    void Start()
    {
        UpdateBatteryUI();
    }

    void Update()
    {
        HandleFlashlight();
    }

    void HandleFlashlight()
    {
        if (Input.GetKeyDown(KeyCode.F))
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
            ToggleFlashlight();
        }
    }

    public void ToggleFlashlight()
    {
        if (battery > 0)
        {
            isFlashlightOn = !isFlashlightOn;
            flashlight.enabled = isFlashlightOn;
            flashlightSound.Play();
        }
    }

    void UpdateBatteryUI()
    {
        if (batteryBar != null)
        {
            batteryBar.value = battery / maxBattery;
        }
    }
}