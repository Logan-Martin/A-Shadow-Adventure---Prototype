using UnityEngine;

public class PillarToLight : MonoBehaviour
{
    //
    public GameObject IsThisKeyboardPortMode;
    //
    private ParticleSystem lightParticle;
    public LanternScript lanternScriptRef;
    public GameObject doorToOpen;
    private bool customIsThisPillarLit = false;
    private int fuelNeededToLightPillar = 10;
    //
    public bool fuelNeededOverride = false;
    //

    private void Start()
    {
        if (IsThisKeyboardPortMode.activeSelf == true)
        {
            lanternScriptRef = IsThisKeyboardPortMode.transform.GetChild(0).GetChild(1).gameObject.GetComponent<LanternScript>();
        }

        lightParticle = this.gameObject.GetComponent<ParticleSystem>();
        doorToOpen.GetComponent<LockedDoor>().AddToTotalLightPillarCount();
    }

    public bool GetPillarLitBool()
    {
        return customIsThisPillarLit;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Lantern"))
        {
            if (customIsThisPillarLit == false)
            {
                if  (lanternScriptRef.GetLightEnabledState())
                {
                    if (!fuelNeededOverride)
                    {
                        if (lanternScriptRef.GetFuel() >= fuelNeededToLightPillar)
                        {
                            customIsThisPillarLit = true;
                            if (!lightParticle.isPlaying) lightParticle.Play();
                            doorToOpen.GetComponent<LockedDoor>().AddToLitLanternCount();
                            //
                            lanternScriptRef.AddFuelToLantern((-fuelNeededToLightPillar + 2));
                        }
                        else
                        {
                            print("Not enough fuel to light pillar!");
                        }
                    }
                    else
                    {
                        customIsThisPillarLit = true;
                        doorToOpen.GetComponent<LockedDoor>().AddToLitLanternCount();
                    }
                }
            }
        }
    }
}
