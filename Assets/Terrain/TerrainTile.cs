using UnityEngine;

public class TerrainTile : MonoBehaviour
{

    //method to be registered with ReceiveDamage component
    public void OnDamageReceived(GameObject src)
    {
        Destroy(this.gameObject);
    }
}
