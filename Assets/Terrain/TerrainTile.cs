using UnityEngine;

public class TerrainTile : MonoBehaviour
{

    [SerializeField] private BoxCollider2D _boxCollider;
    [SerializeField] private SpriteRenderer _spriteRenderer;

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

    public void SplitRandomly (int depth)
    {
        Visible = Random.value > .5f;


        //stop the recursion when we become too small:
        if (depth < 2) return;

        TerrainTile bottomLeft = CreateTile();
        bottomLeft.gameObject.name = "bottom left " + depth;
        bottomLeft.transform.SetParent(this.transform, false);
        bottomLeft.transform.localScale = Vector2.one / 2;
        bottomLeft.transform.localPosition = new Vector2(-1, -1) /4;

        TerrainTile bottomRight = CreateTile();
        bottomRight.gameObject.name = "bottom right " + depth;
        bottomRight.transform.SetParent(this.transform, false);
        bottomRight.transform.localScale = Vector2.one / 2;
        bottomRight.transform.localPosition = new Vector2(1, -1) / 4;

        TerrainTile topRight = CreateTile();
        topRight.gameObject.name = "top right " + depth;
        topRight.transform.SetParent(this.transform, false);
        topRight.transform.localScale = Vector2.one / 2;
        topRight.transform.localPosition = new Vector2(1, 1) / 4;

        TerrainTile topLeft = CreateTile();
        topLeft.gameObject.name = "top left " + depth;
        topLeft.transform.SetParent(this.transform, false);
        topLeft.transform.localScale = Vector2.one / 2;
        topLeft.transform.localPosition = new Vector2(-1, 1) / 4;

        if (Visible)
        {
            //split top 2 children:
            bottomLeft.Visible = true;
            bottomRight.Visible = true;
            topRight.SplitRandomly(depth-1);
            topLeft.SplitRandomly(depth - 1);
            Visible = false; //but we have to disable again
        }
        else
        {
            //split bottom 2 children:
            bottomLeft.SplitRandomly(depth - 1);
            bottomRight.SplitRandomly(depth - 1);
            topRight.Visible = false;
            topLeft.Visible = false;
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
        go.AddComponent<ReceiveDamage>();
        return tile;
    }
    //method to be registered with ReceiveDamage component
    public void OnDamageReceived(GameObject src, int dmg)
    {
        Destroy(this.gameObject);
    }
}
