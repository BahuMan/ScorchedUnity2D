using UnityEngine;

public class WeaponSelfDescription : MonoBehaviour
{
    [SerializeField] private WeaponEnum id;
    [SerializeField] private WeaponTypeEnum type;
    [SerializeField] private string description;
    [SerializeField] private Sprite icon;
    [SerializeField] private int price;

    public WeaponEnum Id { get => id; }
    public WeaponTypeEnum Type { get => type; }
    public string Description { get => description; }
    public Sprite Icon { get => icon; }
    public int Price { get => price; }
}
