using UnityEngine;
using System.Collections.Generic;

//adding a new WeaponType, you should also update the WeaponInfo maps
public enum WeaponTypeEnum { MONEY, MISSILE, DEFENSIVE, GUIDANCE }
//adding a new Weapon, you should also update the WeaponInfo maps
public enum WeaponEnum { MONEY, BABY_MISSILE }

public class WeaponStock
{
    public WeaponStock() { }
    public WeaponStock copy()
    {
        return new WeaponStock { nrInStock = this.nrInStock, weapon = this.weapon };
    }

    public WeaponEnum weapon;
    public int nrInStock;
}

/**
 * static Utility class to keep track of the types of each weapon.
 * This is generated code, but by hand :)
 */
public class WeaponInfo
{
    private static Dictionary<WeaponTypeEnum, List<WeaponEnum>> TypeToWeapons = new Dictionary<WeaponTypeEnum, List<WeaponEnum>>();
    private static Dictionary<WeaponEnum, WeaponTypeEnum> WeaponToType = new Dictionary<WeaponEnum, WeaponTypeEnum>();

    static WeaponInfo() {
        List<WeaponEnum> MoneyList = new List<WeaponEnum>();
        MoneyList.Add(WeaponEnum.MONEY);
        TypeToWeapons.Add(WeaponTypeEnum.MONEY, MoneyList);

        List<WeaponEnum> MissileList = new List<WeaponEnum>();
        MissileList.Add(WeaponEnum.BABY_MISSILE);
        TypeToWeapons.Add(WeaponTypeEnum.MISSILE, MissileList);

        WeaponToType.Add(WeaponEnum.MONEY, WeaponTypeEnum.MONEY);
        WeaponToType.Add(WeaponEnum.BABY_MISSILE, WeaponTypeEnum.MISSILE);
    }

    public static IEnumerable<WeaponEnum> WeaponsOfType(WeaponTypeEnum t)
    {
        return TypeToWeapons[t];
    }

    public static WeaponTypeEnum TypeOf(WeaponEnum w)
    {
        return WeaponToType[w];
    }
}

/**
 * Each tank will have an instance of inventory
 */
public class WeaponInventory
{
    private Dictionary<WeaponEnum, WeaponStock> stock;

    public WeaponInventory()
    {
        stock = new Dictionary<WeaponEnum, WeaponStock>();
        stock.Add(WeaponEnum.BABY_MISSILE, new WeaponStock { weapon = WeaponEnum.BABY_MISSILE, nrInStock = 666});
    }

    public int GetStockForWeapon(WeaponEnum w)
    {
        if (stock.ContainsKey(w)) return stock[w].nrInStock;
        else return 0;
    }

    public List<WeaponStock> GetStockOfType(WeaponTypeEnum t)
    {
        List<WeaponStock> s = new List<WeaponStock>();
        foreach(var w in WeaponInfo.WeaponsOfType(t))
        {
            if (stock.ContainsKey(w))
            {
                s.Add(stock[w].copy());
            }
        }
        return s;
    }
    public IEnumerator<WeaponEnum> GetAllWeapons()
    {
        return stock.Keys.GetEnumerator();
    }
}
