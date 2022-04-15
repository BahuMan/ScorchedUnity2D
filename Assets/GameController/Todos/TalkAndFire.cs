using UnityEngine;
using SimpleBehaviour;

public class TalkAndFire : INode
{
    private TankControl _tank;
    private float talkdelay = 1f;
    private float FireAtTime = 0f;

    public TalkAndFire(TankControl tank)
    {
        _tank = tank;
        FireAtTime = Time.time + talkdelay;
        ChatBubbleControl._instance.ShowChatBubble(_tank.transform, ChatBubbleControl.ChatMoment.FIRE);
    }

    TreeStatusEnum INode.Tick()
    {
        //...wait...
        if (Time.time < FireAtTime) return TreeStatusEnum.RUNNING;

        ChatBubbleControl._instance.HideChatBubble();
        GameController._instance.RemoveThingToDo(this);
        _tank.Fire();
        return TreeStatusEnum.SUCCESS;
    }
}
