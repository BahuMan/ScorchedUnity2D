using UnityEngine;
using SimpleBehaviour;

public class DeclareWinner : INode
{
    public GenericPlayer player;

    public DeclareWinner(GenericPlayer p)
    {
        player = p;
    }

    TreeStatusEnum INode.Tick()
    {
        Debug.Log("Player " + player.PlayerName + " has won!");
        player.GetInventory().ChangeStockForWeapon(WeaponEnum.MONEY, 1000);
        GameController._instance.RemoveThingToDo(this);
        return TreeStatusEnum.SUCCESS;
    }
}
