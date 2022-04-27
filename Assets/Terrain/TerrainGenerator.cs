using UnityEngine;
using SimpleBehaviour;

public class TerrainGenerator : MonoBehaviour, INode
{

    public TerrainTile TilePrefab;
    public int roughness = 5;

    public int minHeight = 10;
    public int maxHeight = 80;
    public int width = 128;


    [ContextMenu("Destroy")]
    public void DestroyTerrain()
    {
        while (transform.childCount > 0) DestroyImmediate(transform.GetChild(0).gameObject);
    }

    [ContextMenu("Generate")]
    public void GenerateTerrain()
    {

        //DestroyTerrain();

        TerrainTile topTile = Instantiate<TerrainTile>(TilePrefab);
        topTile.transform.SetParent(transform);
        topTile.transform.position = Vector2.up * width/2;
        topTile.transform.localScale = Vector2.one * width;
        topTile.SplitRandomly(width);
    }

    TreeStatusEnum INode.Tick()
    {
        this.GenerateTerrain();
        GameController._instance.RemoveThingToDo(this);
        return TreeStatusEnum.SUCCESS;
    }
}
