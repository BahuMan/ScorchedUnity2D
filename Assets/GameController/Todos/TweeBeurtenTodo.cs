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
                //right before the current tank, we need to couple the UI: (and we add it AFTER the turn because we're working with a stack)
                GameController._instance.addThingToDo(new ConnectUI(tank));
            }
        }
        Debug.Log($"Nieuwe beurt; {nrAlive} levende tanks");
        if (nrAlive > 0) return TreeStatusEnum.RUNNING;
        return TreeStatusEnum.SUCCESS;
    }
}
