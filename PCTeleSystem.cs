using System.Runtime.CompilerServices;
using UnityEngine;

public class PCTeleSystem : MonoBehaviour
{
    public Camera cameraMain;
    public GameObject previewCube;
    private float rayDist = 100f;
    private float playerHeightOffset = 1.0f;
    //
    private Renderer objectRenderer;
    public Material baseMat;
    public Material allowedTeleMat;
    private bool onAllowedSurface = false;
    //
    public MainMenuUI mainMenuUI;
    //

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        objectRenderer = previewCube.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1)) // right click
        {
            Ray ray = cameraMain.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, rayDist))
            {
                previewCube.SetActive(true);
                if (LayerMask.LayerToName(hit.transform.gameObject.layer) == "Ground")
                {
                    onAllowedSurface = true;
                    objectRenderer.material = allowedTeleMat;
                }
                else
                {
                    onAllowedSurface = false;
                    objectRenderer.material = baseMat;
                }
                previewCube.transform.position = hit.point;
            }
            else
            {
                previewCube.SetActive(false);
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            if (previewCube.activeSelf == true)
            {
                previewCube.SetActive(false);
                if (onAllowedSurface == true)
                {
                    mainMenuUI.ManualSafeTeleport(previewCube.transform.position + new Vector3(0, playerHeightOffset, 0));
                }
            }
        }

    }
}
