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

    public bool IsPartiallyVisible()
    {
        return Visible
            || (bottomLeft != null && bottomLeft.IsPartiallyVisible())
            || (bottomRight != null && bottomRight.IsPartiallyVisible())
            || (topRight != null && topRight.IsPartiallyVisible())
            || (topLeft != null && topLeft.IsPartiallyVisible());
    }
    /** 
     * return true if dirt was added
     */
    public bool AddDirt(Collider2D shape)
    {
        //if already fully visible -> no change
        if (Visible) return true; 

        //if no overlap 
        if (!OverlapWithExplosion(shape)) return IsPartiallyVisible();

        //end recursion and simply become visible:
        if (depth < 2)
        {
            Visible = true;
            return true;
        }

        //check if explosion only covers part of this terrain:
        Split4Ways();
        bool dirty = false;
        if (bottomLeft.AddDirt(shape)) dirty = true; else Destroy(bottomLeft.gameObject);
        if (bottomRight.AddDirt(shape)) dirty = true; else Destroy(bottomRight.gameObject);
        if (topRight.AddDirt(shape)) dirty = true; else Destroy(topRight.gameObject);
        if (topLeft.AddDirt(shape)) dirty = true; else Destroy(topLeft.gameObject);

        StartCoroutine(AttemptMerge());

        return dirty;
    }

    private System.Collections.IEnumerator AttemptMerge()
    {

        yield return null;

        if ((bottomLeft != null && bottomLeft.Visible)
            && (bottomRight != null && bottomRight.Visible)
            && (topRight != null && topRight.Visible)
            && (topLeft != null && topLeft.Visible))
        {
            Visible = true;
            Destroy(bottomLeft.gameObject);
            Destroy(bottomRight.gameObject);
            Destroy(topRight.gameObject);
            Destroy(topLeft.gameObject);
        }


    }

    [ContextMenu("Split4Ways")]
    public void Split4Ways()
    {

        if (bottomLeft == null)
        {
            bottomLeft = CreateTile();
            bottomLeft.Visible = false;
            bottomLeft.gameObject.name = "bottom left " + depth;
            bottomLeft.transform.SetParent(this.transform, false);
            bottomLeft.transform.localScale = Vector2.one / 2;
            bottomLeft.transform.localPosition = new Vector2(-1, -1) / 4;
            bottomLeft.depth = this.depth - 1;
        }

        if (bottomRight == null)
        {
            bottomRight = CreateTile();
            bottomRight.Visible = false;
            bottomRight.gameObject.name = "bottom right " + depth;
            bottomRight.transform.SetParent(this.transform, false);
            bottomRight.transform.localScale = Vector2.one / 2;
            bottomRight.transform.localPosition = new Vector2(1, -1) / 4;
            bottomRight.depth = this.depth - 1;
        }

        if (topRight == null)
        {
            topRight = CreateTile();
            topRight.Visible = false;
            topRight.gameObject.name = "top right " + depth;
            topRight.transform.SetParent(this.transform, false);
            topRight.transform.localScale = Vector2.one / 2;
            topRight.transform.localPosition = new Vector2(1, 1) / 4;
            topRight.depth = this.depth - 1;
        }

        if (topLeft == null)
        {
            topLeft = CreateTile();
            topLeft.Visible = false;
            topLeft.gameObject.name = "top left " + depth;
            topLeft.transform.SetParent(this.transform, false);
            topLeft.transform.localScale = Vector2.one / 2;
            topLeft.transform.localPosition = new Vector2(-1, 1) / 4;
            topLeft.depth = this.depth - 1;
        }
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

    private bool OverlapWithExplosion(Collider2D explosion)
    {
        return null != Physics2D.OverlapBox(transform.position, transform.lossyScale, 0f, LayerMask.GetMask("Explosion"));
    }

    private bool SplitDamage(Collider2D dmgCollider, int dmg)
    {
        Visible = true;

        if (!OverlapWithExplosion(dmgCollider))
        {
            //Debug.Log("no explosion found");
            return false;
        }
        
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
