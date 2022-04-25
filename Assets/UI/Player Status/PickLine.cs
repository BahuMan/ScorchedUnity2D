using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PickLine: MonoBehaviour, IPointerClickHandler
{

    [SerializeField] private Text StockText;
    [SerializeField] private Text DescriptionText;
    private GenericPlayer CurrentPlayer;
    private Rigidbody2D ThisWeapon;

    public void SetPlayerStock(GenericPlayer p, int stock, string description, Rigidbody2D weapon)
    {
        CurrentPlayer = p;
        StockText.text = stock.ToString();
        DescriptionText.text = description;
        ThisWeapon = weapon;
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        CurrentPlayer.GetTank().shell = ThisWeapon;
    }
}
