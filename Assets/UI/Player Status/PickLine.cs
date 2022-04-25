using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PickLine: MonoBehaviour, IPointerClickHandler
{

    [SerializeField] private Text StockText;
    [SerializeField] private Text DescriptionText;
    private WeaponEnum weaponID;

    public void SetPlayerStock(WeaponEnum id, int stock, string description)
    {
        weaponID = id;
        StockText.text = stock.ToString();
        DescriptionText.text = description;
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        FindObjectOfType<CurrentPlayerPanelControl>().SetActiveWeapon(weaponID);
    }
}
