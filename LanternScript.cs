using UnityEngine;
using UnityEngine.InputSystem;

public class LanternScript : MonoBehaviour
{
    public GameObject IsThisKeyboardPortMode;

    public InputActionReference leftTriggerAction; // PUBLIC!
    private InputAction leftSelectAction; // PRIVATE!
    private bool printDebounceForForceTurnOff = false;

    //
    private Renderer objectRenderer;
    public Material baseMat;
    public Material litMat;
    //
    public GameObject lightToToggle;
    private bool lightEnabled = false;
    //
    private int fuel = 100;
    private int maxFuelAmount = 100;
    public GameObject fuelUI;
    private TMPro.TextMeshProUGUI fuelTextboxRef;
    //
    private float startTimeForFuel;
    private float timeBeforeFuelTicksDown = 2f;
    private int fuelDecreaseBy = 1;
    private int fuelNeededToTurnOnLight = 5;
    //
    private float distanceForLightToImpactEnemy = 10f;
    //

    private void ToggleLitMaterial(bool toggle)
    {
        if (toggle == true)
        {
            objectRenderer.material = litMat;
            return;
        }
        objectRenderer.material = baseMat;
    }

    public bool GetLightEnabledState()
    {
        return lightEnabled;
    }
    public float GetDistanceForLightImpactingEnemies()
    {
        return distanceForLightToImpactEnemy;
    }
    //

    public int GetFuel()
    {
        return fuel;
    }

    public void AddFuelToLantern(int howMuchToAdd)
    {
        int tempCheck = fuel += howMuchToAdd;
        if (tempCheck > maxFuelAmount)
        {
            tempCheck = maxFuelAmount;
        }

        if (tempCheck <= 0)
        {
            tempCheck = 0;
        }

        fuel = tempCheck;
        fuelTextboxRef.text = "Fuel: " + ((fuel / (float)maxFuelAmount) * 100).ToString() + "%";
        //Debug.Log("added to fuel!");
        printDebounceForForceTurnOff = false;
    }


    private void Start()
    {
        startTimeForFuel = Time.time;
        fuelTextboxRef = fuelUI.GetComponent<TMPro.TextMeshProUGUI>();
        objectRenderer = this.gameObject.transform.GetChild(0).gameObject.GetComponent<Renderer>();
    }


    void Awake()
    {
        if (IsThisKeyboardPortMode.activeSelf == true) return;
        leftSelectAction = leftTriggerAction.action; // use action to them subscribe to the events later
    }

    void OnEnable()
    {
        if (IsThisKeyboardPortMode.activeSelf == true) return;
        // Subscribe to event
        if (leftSelectAction != null)
        {
            leftSelectAction.performed += OnSelectPerformed;
            leftSelectAction.Enable();
        }
    }

    void OnDisable()
    {
        if (IsThisKeyboardPortMode.activeSelf == true) return;
        // Unsubscribe from the event to prevent memory leaks
        if (leftSelectAction != null)
        {
            leftSelectAction.performed -= OnSelectPerformed;
            leftSelectAction.Disable();
        }
    }

    private void TryToToggleLantern()
    {
        if (fuel >= fuelNeededToTurnOnLight)
        {
            if (lightEnabled == true)
            {
                lightEnabled = false;
            }
            else
            {
                lightEnabled = true;
            }
            lightToToggle.SetActive(lightEnabled);
            ToggleLitMaterial(lightEnabled);
            //print("SHOULDVE TOGGLED!");
        }
        else
        {
            Debug.Log("Not enough fuel to toggle lantern!");
        }
    }
    private void OnSelectPerformed(InputAction.CallbackContext context)
    {
        //Debug.Log("Left controller select (trigger) pressed!");
        TryToToggleLantern();
    }

    private void Update()
    {
        if (IsThisKeyboardPortMode.activeSelf == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                TryToToggleLantern();
            }
        }
        //

        if (fuel == 0)
        {
            if (lightEnabled == true)
            {
                lightEnabled = false;
            }
            lightToToggle.SetActive(false);

            if (printDebounceForForceTurnOff == false)
            {
                printDebounceForForceTurnOff = true;
                Debug.Log("forced turn off lantern due to lack of fuel!");
            }
        }
        else
        {
            if (lightEnabled)
            {
                float timeDiff = Time.time - startTimeForFuel;
                if (timeDiff >= timeBeforeFuelTicksDown)
                {
                    startTimeForFuel = Time.time;
                    int tempCheck = fuel - fuelDecreaseBy;
                    if (tempCheck <= 0)
                    {
                        tempCheck = 0;
                    }
                    else
                    {
                        fuel = tempCheck;
                    }
                    fuelTextboxRef.text = "Fuel: " + ((fuel / (float)maxFuelAmount) * 100).ToString() + "%";
                }
            }
        }

    }

}
