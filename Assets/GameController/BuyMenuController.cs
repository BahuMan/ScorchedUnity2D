using System;
using UnityEngine;
using UnityEngine.UI;

public class BuyMenuController : MonoBehaviour
{

    [SerializeField] Text PlayerNameText;
    [SerializeField] Text CashText;
    [SerializeField] Text RemainingRoundsText;

    [SerializeField] RectTransform WeaponList;
    
    [SerializeField] BuyWeaponLine[] WeaponLine;

    [SerializeField] Button UpdateButton;
    [SerializeField] Button InventoryButton;
    [SerializeField] Button DoneButton;

    private GenericPlayer[] AllPlayers;
    private int currentPlayer;

    // Start is called before the first frame update
    void Start()
    {
        AllPlayers = FindObjectsOfType<GenericPlayer>();
        currentPlayer = -1;
        FindNextHumanPlayer();
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
            UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
        }
    }

    private void BindPlayer(int p)
    {
        GenericPlayer player = AllPlayers[p];

        PlayerNameText.text = player.PlayerName;
        WeaponInventory inventory = player.GetInventory();
        CashText.text = $"Cash: {inventory.GetStockForWeapon(WeaponEnum.MONEY)}";

        foreach (var w in WeaponLine)
        {
            //w.SetWeapon()
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
