using UnityEngine;
using UnityEngine.UI;

public class CurrentPlayerPanelControl : MonoBehaviour
{
    static public CurrentPlayerPanelControl instance;

    [SerializeField] private Text PlayerNameText;
    [SerializeField] private InputField AngleInput;
    [SerializeField] private InputField ForceInput;
    [SerializeField] private InputField LifeText;

    private void Start()
    {
        instance = this;
    }

    TankControl currentPlayer;

    public void SetCurrentPlayer(TankControl tank)
    {
        if (currentPlayer != null)
        {
            currentPlayer.OnForceChanged.RemoveListener(tank_OnForceChanged);
            currentPlayer.OnAngleChanged.RemoveListener(tank_OnAngleChanged);
        }

        currentPlayer = tank;
        PlayerNameText.text = currentPlayer.gameObject.name;
        AngleInput.text = currentPlayer.Angle.ToString();
        ForceInput.text = currentPlayer.Force.ToString();
        LifeText.text = currentPlayer.HP.ToString();

        currentPlayer.OnForceChanged.AddListener(tank_OnForceChanged);
        currentPlayer.OnAngleChanged.AddListener(tank_OnAngleChanged);
    }

    public void input_OnAngleChanged()
    {
        OnDisable(); //temporarily disable listeners at tank to avoid event loop
        currentPlayer.Angle = float.Parse(AngleInput.text);
        OnEnable();
    }

    public void input_OnForceChanged()
    {
        OnDisable(); //temporarily disable listeners at tank to avoid event loop
        currentPlayer.Force= float.Parse(ForceInput.text);
        OnEnable();
    }

    private void tank_OnAngleChanged(float newAngle)
    {
        AngleInput.text = Mathf.RoundToInt(newAngle).ToString();
    }

    private void tank_OnForceChanged(float newForce)
    {
        ForceInput.text = Mathf.RoundToInt(newForce).ToString();
    }

    private void OnEnable()
    {
        if (currentPlayer != null)
        {
            currentPlayer.OnForceChanged.AddListener(tank_OnForceChanged);
            currentPlayer.OnAngleChanged.AddListener(tank_OnAngleChanged);
        }
    }

    private void OnDisable()
    {
        if (currentPlayer != null)
        {
            currentPlayer.OnForceChanged.RemoveListener(tank_OnForceChanged);
            currentPlayer.OnAngleChanged.RemoveListener(tank_OnAngleChanged);
        }
    }
}
