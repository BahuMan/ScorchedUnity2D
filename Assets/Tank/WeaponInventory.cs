using UnityEngine;
using System.Collections.Generic;

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
        foreach(var w in WeaponInfoControl.WeaponsOfType(t))
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

    public int ChangeStockForWeapon(WeaponEnum w, int delta)
    {
        //baby missiles are infinite; never change the number in stock;
        if (w == WeaponEnum.BABY_MISSILE) return 666;

        if (!stock.ContainsKey(w)) stock[w] = new WeaponStock { nrInStock = delta, weapon = w };
        else stock[w].nrInStock += delta;

        return stock[w].nrInStock;
    }
}
