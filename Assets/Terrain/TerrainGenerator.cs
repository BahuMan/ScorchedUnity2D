using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{

    public GameObject TilePrefab;
    public int roughness = 5;

    public int minHeight = 10;
    public int maxHeight = 80;
    public int width = 96;

    void Start()
    {
    }

    [ContextMenu("Generate")]
    public void GenerateTerrain()
    {

        foreach (Transform child in this.transform) DestroyImmediate(child.gameObject);

        int curHeight = Random.Range(minHeight, maxHeight);
        for (int x = -width; x <= width; ++x)
        {
            for (int y = 0; y<curHeight; ++y)
            {
                GameObject t = Instantiate(TilePrefab);
                t.transform.position = new Vector2(x, y);
                t.transform.SetParent(transform);
            }
            curHeight = curHeight + Random.Range(-roughness, roughness);
            if (curHeight < minHeight) curHeight = minHeight;
            if (curHeight > maxHeight) curHeight = maxHeight;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
