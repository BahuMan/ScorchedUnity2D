using UnityEngine;
using SimpleBehaviour;
using System.Collections.Generic;

public class RemoveDeadTanks : INode
{

    private TankControl tankToCheck;
    private List<TankControl> tanksInPlay;
    public RemoveDeadTanks(TankControl tank, List<TankControl> LiveTanks)
    {
        tankToCheck = tank;
        tanksInPlay = LiveTanks;
    }

    TreeStatusEnum INode.Tick()
    {
        if (tankToCheck.HP <= 0)
        {
            Debug.Log("Tank " + tankToCheck.name + " is dead -> taken out of game");
            tanksInPlay.Remove(tankToCheck);
        }
        GameController._instance.RemoveThingToDo(this);
        return TreeStatusEnum.SUCCESS;
    }
}
