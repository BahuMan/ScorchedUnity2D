using UnityEngine;
using UnityEngine.UI;

public class CurrentPlayerPanelControl : MonoBehaviour
{
    static public CurrentPlayerPanelControl instance;

    [SerializeField] private Text PlayerNameText;
    [SerializeField] private InputField AngleInput;
    [SerializeField] private InputField ForceInput;
    [SerializeField] private InputField LifeText;

    [SerializeField] private Image ActiveWeaponIcon;

    [SerializeField] private PickWeaponPanelControl PickWeaponPanel;

    private void Start()
    {
        instance = this;
    }

    TankControl currentTank;
    public TankControl CurrentTank { get { return currentTank; } }
    public GenericPlayer CurrentPlayer { get { return CurrentTank.GetPlayer(); } }

    public void SetCurrentTank(TankControl tank)
    {
        if (currentTank != null)
        {
            currentTank.OnForceChanged.RemoveListener(tank_OnForceChanged);
            currentTank.OnAngleChanged.RemoveListener(tank_OnAngleChanged);
            currentTank.OnHealthChanged.RemoveListener(tank_OnHealthChanged);
        }

        currentTank = tank;
        PlayerNameText.color = currentTank.GetPlayer().PlayerColor;
        PlayerNameText.text = currentTank.GetPlayer().PlayerName;
        AngleInput.text = currentTank.Angle.ToString();
        ForceInput.text = currentTank.Force.ToString();
        LifeText.text = currentTank.HP.ToString();

        currentTank.OnForceChanged.AddListener(tank_OnForceChanged);
        currentTank.OnAngleChanged.AddListener(tank_OnAngleChanged);
        currentTank.OnHealthChanged.AddListener(tank_OnHealthChanged);
    }

    public void MissileButton_OnClicked()
    {
        //toggle panel:
        PickWeaponPanel.gameObject.SetActive(!PickWeaponPanel.gameObject.activeSelf);
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

    private void tank_OnHealthChanged(int newHealth)
    {
        this.LifeText.text = Mathf.RoundToInt(newHealth).ToString();
    }

    public void SetActiveWeapon(WeaponEnum w)
    {
        WeaponInfo info = WeaponInfoControl.GetInfo(w);
        CurrentTank.shell = info.prefab;
        ActiveWeaponIcon.sprite = info.icon;
    }

    private void OnEnable()
    {
        if (currentTank != null)
        {
            currentTank.OnForceChanged.AddListener(tank_OnForceChanged);
            currentTank.OnAngleChanged.AddListener(tank_OnAngleChanged);
            currentTank.OnHealthChanged.AddListener(tank_OnHealthChanged);
        }
    }

    private void OnDisable()
    {
        if (currentTank != null)
        {
            currentTank.OnForceChanged.RemoveListener(tank_OnForceChanged);
            currentTank.OnAngleChanged.RemoveListener(tank_OnAngleChanged);
            currentTank.OnHealthChanged.RemoveListener(tank_OnHealthChanged);
        }
    }
}
