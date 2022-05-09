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
    private Dictionary<WeaponEnum, WeaponStock> stock;

    private void OnValidate()
    {
        stock = new Dictionary<WeaponEnum, WeaponStock>();
        if (_stock == null) _stock = new List<WeaponStock>(10);
        foreach (var weapon in _stock)
        {
            stock.Add(weapon.weapon, weapon);
        }
        SetStockForWeapon(WeaponEnum.BABY_MISSILE, 666);
        SetStockForWeapon(WeaponEnum.MONEY, Preferences._instance.StartMoney);
    }
    public void Start()
    {
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

    private void SetStockForWeapon(WeaponEnum w, int s)
    {
        if (!stock.ContainsKey(w))
        {
            WeaponStock ws = new WeaponStock { weapon = w, nrInStock = s };
            stock.Add(w, ws);
            _stock.Add(ws);
        }
        else
        {
            stock[w].nrInStock = s;
        }

    }

    public int ChangeStockForWeapon(WeaponEnum w, int delta)
    {
        //baby missiles are infinite; never change the number in stock;
        if (w == WeaponEnum.BABY_MISSILE) return 666;

        if (!stock.ContainsKey(w)) stock.Add(w, new WeaponStock { nrInStock = delta, weapon = w });
        else stock[w].nrInStock += delta;

        return stock[w].nrInStock;
    }
}
