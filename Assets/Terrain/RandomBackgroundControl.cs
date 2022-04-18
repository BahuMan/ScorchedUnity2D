using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class RandomBackgroundControl : MonoBehaviour
{

    [System.Serializable]
    public struct ColorScheme
    {
        public Sprite background;
        public Color terrainColor;
    }

    [SerializeField] ColorScheme[] schemes;
    private SpriteRenderer _sprite;
    public Color terrainColor { get; private set; }

    // Start is called before the first frame update
    void OnEnable()
    {
        _sprite = GetComponent<SpriteRenderer>();
        int choice = Random.Range(0, schemes.Length);
        _sprite.sprite = schemes[choice].background;
        terrainColor = schemes[choice].terrainColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
