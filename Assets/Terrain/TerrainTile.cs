using UnityEngine;

public class TerrainTile : MonoBehaviour
{

    [SerializeField] private BoxCollider2D _boxCollider;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    public int depth;
    private TerrainTile bottomLeft;
    private TerrainTile bottomRight;
    private TerrainTile topLeft;
    private TerrainTile topRight;

    private void Start()
    {
        RandomBackgroundControl bg = FindObjectOfType<RandomBackgroundControl>();
        if (bg != null)
        {
            _spriteRenderer.color = bg.terrainColor;
        }
    }

    //"disable" this square without disabling all references and code:
    public bool Visible
    {
        get => _boxCollider.enabled && _spriteRenderer.enabled;
        private set { _boxCollider.enabled = _spriteRenderer.enabled = value; }
    }

    private float InverseLerp(float min, float max, float v)
    {
        return (v - min) / (max - min);
    }

    public void SplitRandomly ()
    {
        //const float randomness = -5f;

        TerrainGenerator gen = FindObjectOfType<TerrainGenerator>();
        float odds = InverseLerp(gen.minHeight, gen.maxHeight, this.transform.position.y);
        //odds = (randomness * .5f + odds) / (randomness+1);
        //odds = Mathf.Sin(odds * Mathf.PI);
        //odds = Mathf.Pow(odds, randomness);
        Visible = Random.value > odds;

        //stop the recursion when we become too small:
        if (depth < 2) return;

        Split4Ways();

        if (Visible)
        {
            //split top 2 children:
            bottomLeft.Visible = true;
            bottomRight.Visible = true;
            topRight.SplitRandomly();
            topLeft.SplitRandomly();
            Visible = false; //but we have to disable again
        }
        else
        {
            //split bottom 2 children:
            bottomLeft.SplitRandomly();
            bottomRight.SplitRandomly();
            topRight.Visible = false;
            topLeft.Visible = false;
        }
    }

    private void Split4Ways()
    {

        if (bottomLeft != null || bottomRight != null || topLeft != null || topRight != null)
        {
            return;
        }
        bottomLeft = CreateTile();
        bottomLeft.gameObject.name = "bottom left " + depth;
        bottomLeft.transform.SetParent(this.transform, false);
        bottomLeft.transform.localScale = Vector2.one / 2;
        bottomLeft.transform.localPosition = new Vector2(-1, -1) / 4;
        bottomLeft.depth = this.depth - 1;

        bottomRight = CreateTile();
        bottomRight.gameObject.name = "bottom right " + depth;
        bottomRight.transform.SetParent(this.transform, false);
        bottomRight.transform.localScale = Vector2.one / 2;
        bottomRight.transform.localPosition = new Vector2(1, -1) / 4;
        bottomRight.depth = this.depth - 1;

        topRight = CreateTile();
        topRight.gameObject.name = "top right " + depth;
        topRight.transform.SetParent(this.transform, false);
        topRight.transform.localScale = Vector2.one / 2;
        topRight.transform.localPosition = new Vector2(1, 1) / 4;
        topRight.depth = this.depth - 1;

        topLeft = CreateTile();
        topLeft.gameObject.name = "top left " + depth;
        topLeft.transform.SetParent(this.transform, false);
        topLeft.transform.localScale = Vector2.one / 2;
        topLeft.transform.localPosition = new Vector2(-1, 1) / 4;
        topLeft.depth = this.depth - 1;
    }

    private TerrainTile CreateTile()
    {
        GameObject go = new GameObject();
        TerrainTile tile = go.AddComponent<TerrainTile>();
        tile._boxCollider = go.AddComponent<BoxCollider2D>();
        tile._spriteRenderer = go.AddComponent<SpriteRenderer>();
        tile._spriteRenderer.sprite = _spriteRenderer.sprite;
        tile._spriteRenderer.color = _spriteRenderer.color;
        tile._spriteRenderer.sortingLayerID = _spriteRenderer.sortingLayerID;
        ReceiveDamage dmg = go.AddComponent<ReceiveDamage>();
        //for some reason, this UnityEvent is usually null, but I can initialize it myself:
        dmg.OnDamageReceived = new UnityEngine.Events.UnityEvent<GameObject, int>();
        dmg.OnDamageReceived.AddListener(tile.OnDamageReceived);
        return tile;
    }
    //method to be registered with ReceiveDamage component
    public void OnDamageReceived(GameObject src, int dmg)
    {
        SplitDamage(src.GetComponentInChildren<Collider2D>(), dmg);

    }

    private bool SplitDamage(Collider2D dmgCollider, int dmg)
    {
        Visible = true;

        if (null == Physics2D.OverlapBox(transform.position, transform.lossyScale, 0f, LayerMask.GetMask("Explosion")))
        {
            //Debug.Log("no explosion found");
            return false;
        }
        //if (!Physics2D.IsTouching(this._boxCollider, dmgCollider)) return false;
        
        //stop recursion
        if (depth < 2)
        {
            Destroy(this.gameObject);
            return true;
        }

        //the big quad is gone, and we check which of the 4 quarters can remain:
        Visible = false;

        Split4Ways();

        bool bl = bottomLeft.SplitDamage(dmgCollider, dmg);
        bool br = bottomRight.SplitDamage(dmgCollider, dmg);
        bool tl = topLeft.SplitDamage(dmgCollider, dmg);
        bool tr = topRight.SplitDamage(dmgCollider, dmg);

        if (bl && br && tl && tr)
        {
            Debug.Log("all 4 quadrants destroyed => selfdestruct " + this.gameObject.name);
            Destroy(this.gameObject);
            return true;
        }

        //at least one child remaining, so this remains as well:
        return false;

    }
}
