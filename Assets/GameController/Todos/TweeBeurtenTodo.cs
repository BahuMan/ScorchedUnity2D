using UnityEngine;
using SimpleBehaviour;


public class TweeBeurtenTodo : INode
{

    private Sequence beurtSeq;

    public TweeBeurtenTodo(TankControl[] tanks)
    {
        INode[] beurten = new INode[tanks.Length];
        for (int n=0; n<beurten.Length; ++n) {
            beurten[n] = tanks[n].GetInteraction();
        }
        beurtSeq = new Sequence(beurten);
    }

    public TreeStatusEnum Tick()
    {
        TreeStatusEnum result = beurtSeq.Tick();
        switch (result)
        {
            case TreeStatusEnum.RUNNING: return TreeStatusEnum.RUNNING;
            case TreeStatusEnum.FAILURE:
                Debug.LogError("TweeBeurtenTodo encountered an error but we ignore it");
                return TreeStatusEnum.RUNNING;
            case TreeStatusEnum.SUCCESS:
                Debug.Log("All tanks had a turn, we start anew");
                return TreeStatusEnum.RUNNING;
            default:
                return TreeStatusEnum.RUNNING;
        }
    }
}
