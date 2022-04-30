using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class AddPlayerPanelControl : MonoBehaviour
{
    [System.Serializable]
    public class PlayerTypesClass
    {
        public GenericPlayer LocalHumanPrefab;
        public GenericPlayer MoronPrefab;
        public GenericPlayer TosserPrefab;
        public GenericPlayer ShooterPrefab;
    }
    [SerializeField] PlayerTypesClass PlayerPrefabs;

    [SerializeField] Text PlayerOrderText;
    [SerializeField] GameObject HumanPlayerPanel;
        [SerializeField] InputField PlayerNameInput;
    [SerializeField] GameObject BotPlayerPanel;
    [SerializeField] Slider HumanPCSlider;
    
    [SerializeField] Toggle ToggleMoron;
    [SerializeField] Toggle ToggleTosser;
    [SerializeField] Toggle ToggleShooter;
    [SerializeField] Toggle ToggleNotImplemented;

    [SerializeField] Button AddPlayerButton;

    [SerializeField] TankControl tankPrefab;

    [SerializeField] Color[] PlayerColors;
    private int currentColor = 0;

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
        if (Input.GetKeyDown(KeyCode.D))
        {
            ExecuteEvents.Execute(AddPlayerButton.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
        }
        else if (Input.GetKeyDown(KeyCode.M) && BotPlayerPanel.activeSelf)
        {
            ToggleMoron.isOn = true;
        }
        else if (Input.GetKeyDown(KeyCode.N) && HumanPlayerPanel.activeSelf)
        {
            PlayerNameInput.Select();
        }
        else if (Input.GetKeyDown(KeyCode.N) && BotPlayerPanel.activeSelf)
        {
            ToggleNotImplemented.isOn = true;
        }
        else if (Input.GetKeyDown(KeyCode.S) && BotPlayerPanel.activeSelf)
        {
            ToggleShooter.isOn = true;
        }
        else if (Input.GetKeyDown(KeyCode.T) && BotPlayerPanel.activeSelf)
        {
            ToggleTosser.isOn = true;
        }
        //else if (Input.GetKeyDown(KeyCode.N) && HumanPlayerPanel.activeSelf)
        //{
        //    ExecuteEvents.Execute(AddPlayerButton.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
        //}
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
        GenericPlayer player;
        if (HumanPlayerPanel.activeSelf)
        {
            player = Instantiate<GenericPlayer>(PlayerPrefabs.LocalHumanPrefab);
            player.gameObject.name = PlayerNameInput.text + " (Human)";
            player.PlayerName = PlayerNameInput.text;
            player.PlayerColor = this.PlayerColors[currentColor++];
        }
        else
        {
            if (ToggleMoron.isOn)
            {
                player = Instantiate<GenericPlayer>(PlayerPrefabs.MoronPrefab);
                player.gameObject.name = botNames[nextBotName] + " (Moron)";
            }
            else if (ToggleTosser.isOn)
            {
                player = Instantiate<GenericPlayer>(PlayerPrefabs.TosserPrefab);
                player.gameObject.name = botNames[nextBotName] + " (Tosser)";
            }
            else if (ToggleShooter.isOn)
            {
                player = Instantiate<GenericPlayer>(PlayerPrefabs.ShooterPrefab);
                player.gameObject.name = botNames[nextBotName] + " (Shooter)";
            }
            else
            {
                player = null;
                Debug.Log("That's not going to work ...");
            }
        }
        player.transform.position = Vector3.zero;
        player.PlayerName = botNames[nextBotName];
        player.PlayerColor = this.PlayerColors[currentColor++];
        nextBotName++;
        DontDestroyOnLoad(player.gameObject);
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
