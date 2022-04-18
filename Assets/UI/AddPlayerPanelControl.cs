using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class AddPlayerPanelControl : MonoBehaviour
{

    [SerializeField] Text PlayerOrderText;
    [SerializeField] GameObject HumanPlayerPanel;
        [SerializeField] InputField PlayerNameInput;
    [SerializeField] GameObject BotPlayerPanel;
    [SerializeField] Slider HumanPCSlider;
    
    [SerializeField] Toggle ToggleMoron;
    [SerializeField] Toggle ToggleNotImplemented;

    [SerializeField] Button AddPlayerButton;

    [SerializeField] TankControl tankPrefab;
    private string[] botNames;
    private int nextBotName;
    private int currentPlayerNumber;

    private void Start()
    {
        TextAsset botnamesTA = Resources.Load<TextAsset>("BotPlayerNames");
        botNames = botnamesTA.text.Split("\r\n".ToCharArray(), System.StringSplitOptions.RemoveEmptyEntries);
        nextBotName = 0;
        currentPlayerNumber = 1;
        this.PlayerOrderText.text = "Player " + currentPlayerNumber.ToString() + " (of " + Preferences._instance.NrPlayers + ")";
    }

    private void OnEnable()
    {
        PlayerNameInput.onEndEdit.AddListener(PlayerName_OnEndEdit);
    }

    private void OnDisable()
    {
        PlayerNameInput.onEndEdit.RemoveListener(PlayerName_OnEndEdit);
    }

    // Update is called once per frame
    void Update()
    {
        //disable any hotkeys if user is inputting a name:
        if (EventSystem.current.currentSelectedGameObject == PlayerNameInput.gameObject) return;

        //otherwise, these keys form a hotkey:
        if (Input.GetKeyDown(KeyCode.N) && HumanPlayerPanel.activeSelf)
        {
            PlayerNameInput.Select();
        }
        else if (Input.GetKeyDown(KeyCode.M) && BotPlayerPanel.activeSelf)
        {
            ToggleMoron.isOn = true;
        }
        else if (Input.GetKeyDown(KeyCode.N) && HumanPlayerPanel.activeSelf)
        {
            ExecuteEvents.Execute(AddPlayerButton.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
        }
        else if (Input.GetKeyDown(KeyCode.N) && BotPlayerPanel.activeSelf)
        {
            ToggleNotImplemented.isOn = true;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            ExecuteEvents.Execute(AddPlayerButton.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
        }
    }

    public void PlayerName_OnEndEdit(string newname)
    {
        AddPlayerButton.Select();
    }

    public void HumanPCSlider_OnValueChanged()
    {
        if (HumanPCSlider.value < .5f)
        {
            HumanPlayerPanel.SetActive(false);
            BotPlayerPanel.SetActive(true);
        }
        else
        {
            HumanPlayerPanel.SetActive(true);
            BotPlayerPanel.SetActive(false);
        }
    }
    public void DoneButton_OnClick()
    {
        GameObject newp;
        GenericPlayer player;
        if (HumanPlayerPanel.activeSelf)
        {
            newp = new GameObject("Human Player " + PlayerNameInput.text);
            player = newp.AddComponent<GenericPlayer>();
            newp.AddComponent<LocalHumanPlayer>();
        }
        else
        {
            if (ToggleMoron.isOn)
            {
                newp = new GameObject("Bot " + botNames[nextBotName] + " (Moron)");
                player = newp.AddComponent<GenericPlayer>();
                newp.AddComponent<RandomAIPlayer>();
                nextBotName++;
            }
            else
            {
                newp = null;
                player = null;
                Debug.Log("That's not going to work ...");
            }
        }
        player.SetPreferredTankPrefab(tankPrefab);
        WeaponInventory wi = new WeaponInventory();
        wi.ChangeStockForWeapon(WeaponEnum.MONEY, Preferences._instance.StartMoney);
        player.SetInventory(wi);
        newp.transform.position = Vector3.zero;
        DontDestroyOnLoad(newp);
        currentPlayerNumber++;

        //start play if we've defined all players:
        if (currentPlayerNumber > Preferences._instance.NrPlayers)
        {
            SceneManager.LoadScene("SampleScene");
        }
        else
        {
            this.PlayerOrderText.text = "Player " + currentPlayerNumber.ToString() + " (of " + Preferences._instance.NrPlayers + ")";
        }
    }

}
