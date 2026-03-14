using System.Runtime.CompilerServices;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class EnemyScript : MonoBehaviour
{
    //
    public GameObject IsThisKeyboardPortMode;
    //
    public GameObject target;
    public LanternScript lanternScriptRef;
    private float distToBeImpactedByLight;
    private float distForLightImpact_Squared;
    //
    public GameObject heartPickupRef;
    public GameObject copiesFolder;
    //
    private float speed = 3f;
    private int radiusToStayAwayFromPlayer_ALWAYS = 3;
    private int radiusForPlrToMakeEnemyReact = 60;
    private Rigidbody rb;
    //
    private int health = 2;
    private int cannotBeHurtCoolDown = 1;
    private bool canBeDamaged = true;
    //

    // Kill Enemy Stuff:
    void KillThisEnemy() 
    {
        lanternScriptRef.AddFuelToLantern(100);
        //
        int randNum = Random.Range(1, 4); // excludes the max val, So: 1,2,3
        if (randNum == 1)
        {
            GameObject heartToDrop = Object.Instantiate(heartPickupRef, copiesFolder.transform);
            heartToDrop.SetActive(true);
            heartToDrop.transform.position = this.gameObject.transform.position;
        }
        else
        {
            print("Did not drop heart!");
        }
        //
        this.gameObject.SetActive(false);
        Destroy(this.gameObject, 3f);
    }
    // ---- //


    // Flash Red when taking Damage //
    private Renderer objectRenderer;
    private Color originalColor;
    public Color damageColor = Color.red;
    public float flashDuration = 0.15f;
    //
    private IEnumerator FlashRoutine()
    {
        // Change to damage color
        objectRenderer.material.SetColor("_BaseColor", damageColor);
        yield return new WaitForSeconds(flashDuration);
        // Revert to original color
        objectRenderer.material.SetColor("_BaseColor", originalColor);
    }
    private void DoRedFlashWhenTakingDamage()
    {
        StartCoroutine(FlashRoutine());
    }
    // -------------------------- //

    public void TakeDamage(int dmgAmount)
    {
        
         if (health == 0)
         {
             return;
         }

         int tempCheck = health -= dmgAmount;
         if (tempCheck <= 0)
         {
             tempCheck = 0;
         }
         else
         {
             DoRedFlashWhenTakingDamage();
         }

         health = tempCheck;
         //
         if (health == 0)
         {
            KillThisEnemy();
         }

    }


    private void Start()
    {
        if (IsThisKeyboardPortMode.activeSelf == true)
        {
            target = IsThisKeyboardPortMode;
            lanternScriptRef = IsThisKeyboardPortMode.transform.GetChild(0).GetChild(1).gameObject.GetComponent<LanternScript>();
        }

        rb = this.gameObject.GetComponent<Rigidbody>();
        distToBeImpactedByLight = lanternScriptRef.GetDistanceForLightImpactingEnemies();
        distForLightImpact_Squared = distToBeImpactedByLight * distToBeImpactedByLight;
        radiusForPlrToMakeEnemyReact = radiusForPlrToMakeEnemyReact * radiusForPlrToMakeEnemyReact;
        // for flash red when damage:
        objectRenderer = this.gameObject.transform.GetChild(2).gameObject.GetComponent<Renderer>();
        originalColor = objectRenderer.material.GetColor("_BaseColor");
        //
    }

    private void MoveTowardPlayerStep()
    {
        Vector3 moveToVect = Vector3.MoveTowards(rb.position, target.transform.position + (target.transform.forward * radiusToStayAwayFromPlayer_ALWAYS), speed * Time.deltaTime);
        rb.MovePosition(moveToVect);
        //
        Vector3 directionToPlayer = new Vector3(target.transform.position.x - rb.position.x, 0, target.transform.position.z - rb.position.z);
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        rb.MoveRotation(targetRotation);
    }

    private void MoveAwayFromLightStep()
    {
        Vector3 moveToVect = Vector3.MoveTowards(rb.position, target.transform.position + (target.transform.forward * radiusToStayAwayFromPlayer_ALWAYS * -1), speed * 1.5f * Time.deltaTime);
        rb.MovePosition(moveToVect);
        //
        Vector3 directionAwayFromPlayer = new Vector3(target.transform.position.x - rb.position.x, 0, target.transform.position.z - rb.position.z) * -1; // -1 to make it away from playuer
        Quaternion targetRotation = Quaternion.LookRotation(directionAwayFromPlayer);
        rb.MoveRotation(targetRotation);
    }

    IEnumerator DamageCooldown()
    {
        canBeDamaged = false;
        yield return new WaitForSeconds(cannotBeHurtCoolDown);
        canBeDamaged = true;
    }

    private void OnTriggerEnter(Collider hit)
    {
        if (!hit.gameObject.CompareTag("Sword")) return;

        if (!canBeDamaged) return;

        TakeDamage(1);
        if (health - 1 == 0) return;

        if (this.gameObject.activeSelf == true)
        {
            StartCoroutine(DamageCooldown());
        }
    }

    void FixedUpdate()
    {
        if ((target != null) && (health > 0))
        {
            Vector3 initDistance = (target.transform.position - this.gameObject.transform.position);
            float distance = initDistance.sqrMagnitude;
            if (distance <= radiusForPlrToMakeEnemyReact)
            {
                if (lanternScriptRef.GetLightEnabledState() == false)
                {
                    MoveTowardPlayerStep();
                }
                else
                {
                    // LANTERN ON!!!
                    if (distance >= distForLightImpact_Squared)
                    {
                        MoveAwayFromLightStep();
                    }
                }
            }
        }
    }
}
