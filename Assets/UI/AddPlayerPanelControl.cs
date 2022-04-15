using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

    // Start is called before the first frame update
    void Start()
    {
        PlayerNameInput.onEndEdit.AddListener(PlayerName_OnEndEdit);
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == PlayerNameInput.gameObject) return;
        if (PlayerNameInput.isFocused) return;

        //if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        //{
        if (Input.GetKeyDown(KeyCode.N) && HumanPlayerPanel.activeSelf)
        {
            PlayerNameInput.Select();
        }
        else if (Input.GetKeyDown(KeyCode.M) && BotPlayerPanel.activeSelf)
        {
            ToggleMoron.isOn = true;
        }
        else if (Input.GetKeyDown(KeyCode.N) && BotPlayerPanel.activeSelf)
        {
            ToggleNotImplemented.isOn = true;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            ExecuteEvents.Execute(AddPlayerButton.gameObject, new BaseEventData(EventSystem.current), ExecuteEvents.submitHandler);
        }
        //}
    }

    public void PlayerName_OnEndEdit(string newname)
    {
        Debug.Log("Player name changed to " + newname);
        AddPlayerButton.Select();
    }
    public void BotToggle_OnValueChanged(string bottype)
    {
        Debug.Log("toggle selected: " + bottype);
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
        if (HumanPlayerPanel.activeSelf)
        {
            Debug.Log("Adding human '" + PlayerNameInput.text + "'");
        }
        else
        {
            if (ToggleMoron.isOn)
            {
                Debug.Log("Adding Moron Computer");
            }
            else if (ToggleNotImplemented.isOn)
            {
                Debug.Log("That's not going to work ...");
            }
        }
    }
}
