using SimpleBehaviour;
using UnityEngine;
public class ConnectUI : INode
{
    private TankControl currentTank;

    public ConnectUI(TankControl t)
    {
        currentTank = t;
    }
    TreeStatusEnum INode.Tick()
    {
        Debug.Log("Connecting " + currentTank.name + " to UI");
        CurrentPlayerPanelControl.instance.SetCurrentTank(currentTank);
        GameController._instance.RemoveThingToDo(this);
        return TreeStatusEnum.SUCCESS;
    }
}
