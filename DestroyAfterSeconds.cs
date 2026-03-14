using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    public float despawnTime = 8f;
    void Start()
    {
        Destroy(this.gameObject, despawnTime);
    }
}
