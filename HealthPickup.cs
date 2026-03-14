using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public GameObject IsThisKeyboardPortMode;
    //
    public GameObject playerRef;
    private int healHowMuch = 25;

    private void Start()
    {
        if (IsThisKeyboardPortMode.activeSelf)
        {
            playerRef = IsThisKeyboardPortMode;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        playerRef.GetComponent<MainPlrScript>().AddOrSubToPlayerHealth(healHowMuch);

        this.gameObject.SetActive(false);
        Destroy(this.gameObject, 1f);
    }
}
