using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuyWeaponLine : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] WeaponEnum Id;
    [SerializeField] Text StockText;
    [SerializeField] Image WeaponIcon;
    [SerializeField] Text DescriptionText;
    [SerializeField] Text PriceText;

    public void SetWeapon(WeaponEnum id, Sprite icon, string description, int price)
    {
        this.Id = id;
        StockText.text = "";
        WeaponIcon.sprite = icon;
        DescriptionText.text = description;
        PriceText.text = $"$ {price}";
    }

    public void SetStock(int stock)
    {
        this.StockText.text = stock.ToString();
    }

    public WeaponEnum GetID()
    {
        return Id;
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("(pointer click) Player bought " + DescriptionText.text);
    }
}
