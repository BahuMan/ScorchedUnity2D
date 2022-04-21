using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuyWeaponLine : MonoBehaviour, IPointerClickHandler
{

    [SerializeField] Text StockText;
    [SerializeField] Image WeaponIcon;
    [SerializeField] Text DescriptionText;
    [SerializeField] Text PriceText;

    public void SetWeapon(int stock, Sprite icon, string description, int price)
    {
        StockText.text = stock.ToString();
        WeaponIcon.sprite = icon;
        DescriptionText.text = description;
        PriceText.text = $"$ {price}";
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("(pointer click) Player bought " + DescriptionText.text);
    }
}
