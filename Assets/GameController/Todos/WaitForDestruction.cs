using SimpleBehaviour;
using UnityEngine;

public class WaitForDestruction : SimpleBehaviour.INode
{
    private Object waiting;
    public WaitForDestruction(Object waitfor)
    {
        waiting = waitfor;
    }

    TreeStatusEnum INode.Tick()
    {
        if (waiting == null)
        {
            GameController._instance.RemoveThingToDo(this);
            return TreeStatusEnum.SUCCESS;
        }
        else return TreeStatusEnum.RUNNING;
    }
}
