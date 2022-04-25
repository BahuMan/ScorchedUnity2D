using System;
using UnityEngine;

public class PickWeaponPanelControl: MonoBehaviour
{

    [SerializeField] PickLine LinePrefab;

    private void OnEnable()
    {
        FillAvailableWeapons();
    }

    private void OnDisable()
    {
        foreach (Transform child in transform)
        {
            if (child != LinePrefab.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }

    private void FillAvailableWeapons()
    {
        CurrentPlayerPanelControl curPanel = FindObjectOfType<CurrentPlayerPanelControl>();
        GenericPlayer curPlayer = curPanel.CurrentPlayer;
        Debug.Log("Filling available weapons for " + curPlayer.PlayerName);

        foreach (var s in curPlayer.GetInventory().GetStockOfType(WeaponTypeEnum.MISSILE))
        {
            WeaponInfo info = WeaponInfoControl.GetInfo(s.weapon);
            if (s.weapon == WeaponEnum.BABY_MISSILE)
            {
                LinePrefab.SetPlayerStock(curPlayer, s.nrInStock, info.description, info.prefab);
            }
            else
            {
                PickLine pl = Instantiate<PickLine>(LinePrefab);
                pl.transform.SetParent(this.transform);
                pl.SetPlayerStock(curPlayer, s.nrInStock, info.description, info.prefab);
            }
        }    
    }
}
