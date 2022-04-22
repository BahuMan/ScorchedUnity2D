using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//adding a new WeaponType, you should also update the WeaponInfo maps
public enum WeaponTypeEnum { MONEY, MISSILE, DEFENSIVE, GUIDANCE }
//adding a new Weapon, you should also update the WeaponInfo maps
public enum WeaponEnum { MONEY, BABY_MISSILE, MIRV }

[System.Serializable]
public class WeaponInfo
{
    public WeaponEnum id;
    public WeaponTypeEnum type;
    public string description;
    public Sprite icon;
    public int price;
    public Rigidbody2D prefab;
}

public class WeaponInfoControl : MonoBehaviour
{

    [SerializeField] private WeaponInfo[] AllWeapons;

    /**
     * STATIC PART (semi-generated, by hand)
     */
    private static Dictionary<WeaponTypeEnum, List<WeaponEnum>> TypeToWeapons = new Dictionary<WeaponTypeEnum, List<WeaponEnum>>();
    private static Dictionary<WeaponEnum, WeaponInfo> WeaponToInfo = new Dictionary<WeaponEnum, WeaponInfo>();

    void Start ()
    {

        foreach (var weapon in AllWeapons)
        {
            if (!TypeToWeapons.ContainsKey(weapon.type)) TypeToWeapons.Add(weapon.type, new List<WeaponEnum>(AllWeapons.Length));
            TypeToWeapons[weapon.type].Add(weapon.id);

            WeaponToInfo.Add(weapon.id, weapon);
        }
    }

    public static IEnumerable<WeaponEnum> WeaponsOfType(WeaponTypeEnum t)
    {
        return TypeToWeapons[t];
    }

    public static WeaponInfo GetInfo(WeaponEnum w)
    {
        return WeaponToInfo[w];
    }
}
