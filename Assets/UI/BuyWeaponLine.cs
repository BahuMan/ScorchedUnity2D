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
    
    private int PriceNumber;
    private GenericPlayer CurrentPlayer;

    public void SetWeapon(WeaponEnum id, Sprite icon, string description, int price)
    {
        this.gameObject.name = "WeaponLine for " + description;
        this.Id = id;
        StockText.text = "";
        WeaponIcon.sprite = icon;
        DescriptionText.text = description;
        PriceText.text = $"$ {price}";
        PriceNumber = price;
    }

    public void SetStock(GenericPlayer p, WeaponEnum id, int stock)
    {
        if (this.Id != id) Debug.LogError("Incorrect stock change at " + this.gameObject.name);
        this.StockText.text = stock == 0? "": stock.ToString();
        this.CurrentPlayer = p;
    }

    public WeaponEnum GetID()
    {
        return Id;
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (this.Id == WeaponEnum.BABY_MISSILE) return; //infinite baby missiles -> no action

        WeaponInventory inv = CurrentPlayer.GetInventory();
        if (PriceNumber <= inv.GetStockForWeapon(WeaponEnum.MONEY))
        {
            //subtract price
            inv.ChangeStockForWeapon(WeaponEnum.MONEY, -PriceNumber);
            //add 1 weapon to stock:
            inv.ChangeStockForWeapon(this.Id, 1);
            //update GUI:
            SetStock(CurrentPlayer, this.Id, inv.GetStockForWeapon(this.Id));
            FindObjectOfType<PlayerCash>().SetCash(inv.GetStockForWeapon(WeaponEnum.MONEY));
        }
    }
}
