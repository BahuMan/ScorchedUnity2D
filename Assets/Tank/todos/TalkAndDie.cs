using UnityEngine;
using SimpleBehaviour;

public class TalkAndDie : INode
{
    #pragma warning disable IDE0044 // Add readonly modifier
    private TankControl _tank;      // The tank may change because it can be destroyed externally
    #pragma warning restore IDE0044 // Add readonly modifier
    private const float talkdelay = 1f;
    private readonly float DieAtTime = 0f;
    private readonly GameObject _explosion;

    public TalkAndDie(TankControl tank, GameObject explosion)
    {
        _tank = tank;
        _explosion = explosion;
        DieAtTime = Time.time + talkdelay;
        ChatBubbleControl._instance.ShowChatBubble(tank.transform, ChatBubbleControl.ChatMoment.DIE);
    }

    TreeStatusEnum INode.Tick()
    {
        //...wait...
        if (Time.time < DieAtTime) return TreeStatusEnum.RUNNING;

        ChatBubbleControl._instance.HideChatBubble();
        GameController._instance.RemoveThingToDo(this);
        if (_explosion != null)
        {
            GameObject go = Object.Instantiate(_explosion);
            go.transform.position = _tank.transform.position;
        }
        //in case of multiple explosions, a "TalkAndDie" could've been instantiated
        //multiple times
        if (_tank!=null) Object.Destroy(_tank.gameObject);
        return TreeStatusEnum.SUCCESS;
    }
}
