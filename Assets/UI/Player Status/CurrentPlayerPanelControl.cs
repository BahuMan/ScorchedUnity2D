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

    TankControl currentTank;

    public void SetCurrentTank(TankControl tank)
    {
        if (currentTank != null)
        {
            currentTank.OnForceChanged.RemoveListener(tank_OnForceChanged);
            currentTank.OnAngleChanged.RemoveListener(tank_OnAngleChanged);
        }

        currentTank = tank;
        PlayerNameText.text = currentTank.GetPlayer().PlayerName;
        AngleInput.text = currentTank.Angle.ToString();
        ForceInput.text = currentTank.Force.ToString();
        LifeText.text = currentTank.HP.ToString();

        currentTank.OnForceChanged.AddListener(tank_OnForceChanged);
        currentTank.OnAngleChanged.AddListener(tank_OnAngleChanged);
    }

    public void input_OnAngleChanged()
    {
        OnDisable(); //temporarily disable listeners at tank to avoid event loop
        currentTank.Angle = float.Parse(AngleInput.text);
        OnEnable();
    }

    public void input_OnForceChanged()
    {
        OnDisable(); //temporarily disable listeners at tank to avoid event loop
        currentTank.Force= float.Parse(ForceInput.text);
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
        if (currentTank != null)
        {
            currentTank.OnForceChanged.AddListener(tank_OnForceChanged);
            currentTank.OnAngleChanged.AddListener(tank_OnAngleChanged);
        }
    }

    private void OnDisable()
    {
        if (currentTank != null)
        {
            currentTank.OnForceChanged.RemoveListener(tank_OnForceChanged);
            currentTank.OnAngleChanged.RemoveListener(tank_OnAngleChanged);
        }
    }
}
