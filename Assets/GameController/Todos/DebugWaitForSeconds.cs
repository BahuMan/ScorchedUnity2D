using UnityEngine;
using SimpleBehaviour;

public class DebugWaitForSeconds : INode
{
    private float endTime = 0f;

    public DebugWaitForSeconds(float seconds)
    {
        endTime = Time.time + seconds;
    }

    TreeStatusEnum INode.Tick()
    {
        if (Time.time < endTime) return TreeStatusEnum.RUNNING;

        GameController._instance.RemoveThingToDo(this);
        return TreeStatusEnum.SUCCESS;
    }
}
