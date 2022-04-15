using SimpleBehaviour;

public class DebugWaitFor1Frame : INode
{
    TreeStatusEnum INode.Tick()
    {
        GameController._instance.RemoveThingToDo(this);
        return TreeStatusEnum.SUCCESS;
    }
}
