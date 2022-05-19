using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
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

public class WeaponInventory: MonoBehaviour
{

    [SerializeField] List<WeaponStock> _stock;
    private Dictionary<WeaponEnum, WeaponStock> stockMap;

    private void OnValidate()
    {
        stockMap = new Dictionary<WeaponEnum, WeaponStock>();
        if (_stock == null) _stock = new List<WeaponStock>(10);
        foreach (var weapon in _stock)
        {
            stockMap.Add(weapon.weapon, weapon);
        }
        SetStockForWeapon(WeaponEnum.BABY_MISSILE, 666);
        SetStockForWeapon(WeaponEnum.MONEY, Preferences._instance != null? Preferences._instance.StartMoney: 500);
    }
    public void Start()
    {
        if (stockMap == null) OnValidate();
    }

    public int GetStockForWeapon(WeaponEnum w)
    {
        if (stockMap.ContainsKey(w)) return stockMap[w].nrInStock;
        else return 0;
    }

    public List<WeaponStock> GetStockOfType(WeaponTypeEnum t)
    {
        List<WeaponStock> s = new List<WeaponStock>();
        foreach(var w in WeaponInfoControl.WeaponsOfType(t))
        {
            if (stockMap.ContainsKey(w))
            {
                s.Add(stockMap[w].copy());
            }
        }
        return s;
    }
    public IEnumerator<WeaponEnum> GetAllWeapons()
    {
        return stockMap.Keys.GetEnumerator();
    }

    private void SetStockForWeapon(WeaponEnum w, int s)
    {
        if (!stockMap.ContainsKey(w))
        {
            WeaponStock ws = new WeaponStock { weapon = w, nrInStock = s };
            stockMap.Add(w, ws);
            _stock.Add(ws);
        }
        else
        {
            stockMap[w].nrInStock = s;
        }

    }

    public int ChangeStockForWeapon(WeaponEnum w, int delta)
    {
        //baby missiles are infinite; never change the number in stock;
        if (w == WeaponEnum.BABY_MISSILE) return 666;

        if (!stockMap.ContainsKey(w)) stockMap.Add(w, new WeaponStock { nrInStock = delta, weapon = w });
        else stockMap[w].nrInStock += delta;

        return stockMap[w].nrInStock;
    }
}
