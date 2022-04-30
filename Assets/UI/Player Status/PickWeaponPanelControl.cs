using UnityEngine;
using UnityEngine.EventSystems;

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

    private void Update()
    {
        //on any mouse click anywhere, disable this menu again:
        if (Input.GetMouseButtonUp(0)
            || Input.GetMouseButtonUp(1)
            || Input.GetMouseButtonUp(2))
        {
            this.gameObject.SetActive(false);
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
                LinePrefab.SetPlayerStock(info.id, s.nrInStock, info.description);
            }
            else
            {
                PickLine pl = Instantiate<PickLine>(LinePrefab);
                pl.transform.SetParent(this.transform);
                pl.SetPlayerStock(info.id, s.nrInStock, info.description);
            }
        }    
    }
}
