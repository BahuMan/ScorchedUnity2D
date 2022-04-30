using UnityEngine;
using SimpleBehaviour;

public class GenericPlayer : MonoBehaviour
{

    [SerializeField]
    private TankControl preferredTankPrefab;
    public TankControl getPreferredTankPrefab() { return preferredTankPrefab; }

    public enum PlayerTypeEnum { HUMAN, MORON, TOSSER, SHOOTER }
    public PlayerTypeEnum PlayerType;
    public string PlayerName;
    public Color PlayerColor;

    private void Start()
    {
        myInventory = GetComponent<WeaponInventory>();
    }

    private TankControl instantiatedTank;
    public TankControl GetTank() { return instantiatedTank; }
    public void SetTank(TankControl t)
    {
        instantiatedTank = t;
        t.gameObject.name = "Tank " + this.gameObject.name;
        if (interaction == null) Debug.LogError("tank was set before interaction for " + gameObject.name);
        else t.SetPlayer(this, interaction);
    }

    private WeaponInventory myInventory;

    public WeaponInventory GetInventory()
    {
        if (myInventory == null) Debug.LogError("Trying to retrieve Inventory before it was set for " + gameObject.name);
        return myInventory;
    }

    private INode interaction;
    public void SetInteraction(SimpleBehaviour.INode act)
    {
        if (interaction != null) throw new UnityException("GenericPlayer: can't set two interactions for one tank " + this.gameObject.name);
        interaction = act;
    }
}
