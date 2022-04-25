using UnityEngine;

public class TerrainTile : MonoBehaviour
{

    private void Start()
    {
        RandomBackgroundControl bg = FindObjectOfType<RandomBackgroundControl>();
        if (bg != null)
        {
            SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
            mySprite.color = bg.terrainColor;
        }
    }
    //method to be registered with ReceiveDamage component
    public void OnDamageReceived(GameObject src, int dmg)
    {
        Destroy(this.gameObject);
    }
}
