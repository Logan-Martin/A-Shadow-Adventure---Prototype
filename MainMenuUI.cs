using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class MainMenuUI : MonoBehaviour
{
    //
    public GameObject IsThisKeyboardPortMode;
    //

    // Init:
    public MainPlrScript mainPlrScript;
    public TeleportationProvider teleportProvider;
    //
    public Button playButton;
    public Button exitGameButton;
    public Button playButton_PCPort;
    public Button exitGameButton_PCPort;

    //
    public GameObject spawnObj;
    public GameObject InGameUI;
    public GameObject lanternObj;
    public GameObject swordObj;
    //

    public void ManualSafeTeleport(Vector3 targetPos) {
        mainPlrScript.TogglePlayerMovement(false);

        if (IsThisKeyboardPortMode.activeSelf == true)
        {
            IsThisKeyboardPortMode.transform.position = targetPos;
            Physics.SyncTransforms();

            mainPlrScript.TogglePlayerMovement(true);
            return;
        }

        // Create a request that the Mediator can understand
        var request = new TeleportRequest
        {
            destinationPosition = targetPos,
            matchOrientation = MatchOrientation.WorldSpaceUp // Keeps player upright
        };

        // Queues the teleport safely through the XRI 3.0 system
        teleportProvider.QueueTeleportRequest(request);
        //
        mainPlrScript.TogglePlayerMovement(true);
    }

    //--------//

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (IsThisKeyboardPortMode.activeSelf == true)
        {
            InGameUI = IsThisKeyboardPortMode.transform.GetChild(0).GetChild(0).GetChild(0).gameObject;
            mainPlrScript = IsThisKeyboardPortMode.GetComponent<MainPlrScript>();
            lanternObj = IsThisKeyboardPortMode.transform.GetChild(0).GetChild(1).GetChild(0).gameObject;
            swordObj = IsThisKeyboardPortMode.transform.GetChild(0).GetChild(2).GetChild(0).gameObject;
            //
            playButton_PCPort.onClick.AddListener(StartGameFunc);
            exitGameButton_PCPort.onClick.AddListener(QuitGameFunc);
            return;
        }
        playButton.onClick.AddListener(StartGameFunc);
        exitGameButton.onClick.AddListener(QuitGameFunc);
    }

    public void StartGameFunc()
    {
        ManualSafeTeleport(spawnObj.transform.position);
        mainPlrScript.TogglePlayerMovement(true);
        //print("teleported player to main world!");
        InGameUI.SetActive(true);
        lanternObj.SetActive(true);
        swordObj.SetActive(true);
    }

    public void QuitGameFunc()
    {
        print("Quit game!");
        Application.Quit();
    }
}
