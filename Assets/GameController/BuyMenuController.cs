using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyMenuController : MonoBehaviour
{

    [SerializeField] Text PlayerNameText;
    [SerializeField] Text CashText;
    [SerializeField] Text RemainingRoundsText;

    [SerializeField] RectTransform WeaponList;
    
    [SerializeField] BuyWeaponLine WeaponLinePrefab;

    [SerializeField] Button UpdateButton;
    [SerializeField] Button InventoryButton;
    [SerializeField] Button DoneButton;

    private GenericPlayer[] AllPlayers;
    private int currentPlayer;

    // Start is called before the first frame update
    void Start()
    {
        FillWeaponList(WeaponTypeEnum.MISSILE);
        AllPlayers = FindObjectsOfType<GenericPlayer>();
        currentPlayer = -1;
        FindNextHumanPlayer();
    }

    private void FillWeaponList(WeaponTypeEnum wt)
    {
        foreach (var w in WeaponInfoControl.WeaponsOfType(wt))
        {
            WeaponInfo wi = WeaponInfoControl.GetInfo(w);
            Debug.Log("showing weapon " + wi.description);
            if (wi.id == WeaponEnum.BABY_MISSILE)
            {
                WeaponLinePrefab.SetWeapon(wi.id, wi.icon, wi.description, wi.price);
            }
            else
            {
                BuyWeaponLine weaponLine = Instantiate<BuyWeaponLine>(WeaponLinePrefab);
                weaponLine.SetWeapon(wi.id, wi.icon, wi.description, wi.price);
                weaponLine.transform.SetParent(WeaponList.transform, false);
            }

        }
    }

    private void FindNextHumanPlayer()
    {
        do
        {
            currentPlayer++;
        } while (currentPlayer < AllPlayers.Length && AllPlayers[currentPlayer].PlayerType != GenericPlayer.PlayerTypeEnum.HUMAN);
        
        if (currentPlayer < AllPlayers.Length)
        {
            BindPlayer(currentPlayer);
        }
        else
        {
            Debug.Log("All human players got to shop, starting the next game");
            UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
        }
    }

    private void BindPlayer(int p)
    {
        GenericPlayer player = AllPlayers[p];

        PlayerNameText.text = player.PlayerName;
        WeaponInventory inventory = player.GetInventory();
        CashText.text = $"Cash: {inventory.GetStockForWeapon(WeaponEnum.MONEY)}";

        foreach (var w in FindObjectsOfType<BuyWeaponLine>())
        {
            WeaponEnum id = w.GetID();
            int playerstock = inventory.GetStockForWeapon(id);
            w.SetStock(playerstock);
        }
    }

    public void UpdateButton_Clicked()
    {
        Debug.Log("I have no idea what purpose is served by the update button");
    }

    public void InventoryButton_Clicked()
    {
        Debug.Log("Inventory and sales functionality not (yet) implemented");
    }

    public void DoneButton_Clicked()
    {
        FindNextHumanPlayer();
    }
}
