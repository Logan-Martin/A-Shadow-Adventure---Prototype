using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    private int totalConnectedLightPillars = 0;
    private int currentLitPillars = 0;
    //
    private Renderer objectRenderer;
    //public Material baseMat;
    public Material litMat;

    private void Start()
    {
        if (this.gameObject.CompareTag("LightBridge"))
        {
            objectRenderer = this.gameObject.GetComponent<Renderer>();
        }
    }

    private void OpenDoor()
    {
        if (this.gameObject.CompareTag("LightBridge"))
        {
            this.gameObject.GetComponent<BoxCollider>().enabled = true;
            objectRenderer.material = litMat;
            return;
        }
        this.gameObject.SetActive(false);
    }

    public void AddToTotalLightPillarCount()
    {
        totalConnectedLightPillars += 1;
        // done on start by PillarToLight script through the GameObject ref assigned
    }

    public void AddToLitLanternCount()
    {
        currentLitPillars += 1;
        if (currentLitPillars >= totalConnectedLightPillars)
        {
            OpenDoor();
        }
    }
}
