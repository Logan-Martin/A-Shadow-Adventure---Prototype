using UnityEngine;

public class HurtPlrTest : MonoBehaviour
{
    public GameObject IsThisKeyboardPortMode;
    public GameObject playerGameObj;

    private void Start()
    {
        if (IsThisKeyboardPortMode.activeSelf == true)
        {
            playerGameObj = IsThisKeyboardPortMode;
        }
    }

    private void OnTriggerEnter(Collider hit)
    {
        if (!hit.gameObject.CompareTag("Player")) return;
        IsThisKeyboardPortMode.GetComponent<MainPlrScript>().ResetPlayer();
    }
}
