using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class MainPlrScript : MonoBehaviour
{
    //
    public MainMenuUI mainMenuUI;
    public GameObject worldSpawnGameObj;
    //
    //
    public PlrKBMCamScript plrKBMCamScript;
    public PlayerKBMMovement playerKBMMovement;
    //
    private int health = 100;
    private int maxHealth = 100;
    private int cannotBeHurtCoolDown = 1;
    private bool canBeDamaged = true;
    //
    public GameObject healthUI;
    public RectTransform actualHealthBar;
    private float maxXCordForHealthBar;
    // ---- //

    // Toggling Movement Stuff:
    public GameObject LocomotionMove;
    public GameObject LocomotionTurn;
    public GameObject LocomotionTeleport;
    //

    private void UpdateHealthUI()
    {
        //print("update health UI!");
        float tempPercent = (float)health / maxHealth;
        float XCord = tempPercent * maxXCordForHealthBar;
        actualHealthBar.sizeDelta = new Vector2(XCord, actualHealthBar.sizeDelta.y);
        //healthUI.GetComponent<TMPro.TextMeshProUGUI>().text = "";
    }

    private void Start()
    {
        maxXCordForHealthBar = actualHealthBar.sizeDelta.x;
        UpdateHealthUI();
    }

    public int GetHealth()
    {
        return health;
    }


    public void AddOrSubToPlayerHealth(int addOrSub)
    {
        //print("TAKE DAMAGE!");
        //
        int tempCheck = health + addOrSub;
        if (tempCheck > maxHealth)
        {
            tempCheck = maxHealth;
        }
        
        if (tempCheck <= 0)
        {
            tempCheck = 0;
        }
        health = tempCheck;
        UpdateHealthUI();

        // Reset stuff
        if (health <= 0)
        {
            ResetPlayer();
        }
    }

    IEnumerator DamageCooldown()
    {
        canBeDamaged = false;
        yield return new WaitForSeconds(cannotBeHurtCoolDown);
        canBeDamaged = true;
    }
    private void OnTriggerEnter(Collider hit)
    {
        if (!hit.gameObject.CompareTag("Enemy")) return;

        if (!canBeDamaged) return;

        AddOrSubToPlayerHealth(-10);
        StartCoroutine(DamageCooldown());
    }

    public void TogglePlayerMovement(bool toggle)
    {
        if (this.gameObject.name == "PORT-CHAR-KeyboardAndMouse")
        {
            // STUFF HERE!
            plrKBMCamScript.enabled = toggle;
            playerKBMMovement.enabled = toggle;

            if (toggle == true)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
            }

            return;
        }

        LocomotionMove.SetActive(toggle);
        LocomotionTurn.SetActive(toggle);
        LocomotionTeleport.SetActive(toggle);
    }

    public void ResetPlayer()
    {
        health = maxHealth;
        UpdateHealthUI();
        mainMenuUI.ManualSafeTeleport(worldSpawnGameObj.transform.position);
    }
}
