using UnityEngine;
using SimpleBehaviour;

public class TalkAndDie : INode
{
    private TankControl _tank;
    private float talkdelay = 1f;
    private float DieAtTime = 0f;
    private GameObject _explosion;

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
        Object.Destroy(_tank.gameObject);
        return TreeStatusEnum.SUCCESS;
    }
}
