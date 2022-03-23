using UnityEngine;
using SimpleBehaviour;
using System.Collections.Generic;


public class TweeBeurtenTodo : INode
{

    private List<TankControl> TankBeurten;

    public TweeBeurtenTodo(TankControl[] tanks)
    {
        TankBeurten = new List<TankControl>(tanks);
    }

    public TreeStatusEnum Tick()
    {
        int nrAlive = 0;
        foreach (TankControl tank in TankBeurten)
        {
            if (tank.HP > 0)
            {
                nrAlive++;
                GameController._instance.addThingToDo(tank.GetInteraction());
            }
        }
        Debug.Log($"Nieuwe beurt; {nrAlive} levende tanks");
        if (nrAlive > 0) return TreeStatusEnum.RUNNING;
        return TreeStatusEnum.SUCCESS;
    }
}
