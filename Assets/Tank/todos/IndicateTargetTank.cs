using UnityEngine;
using SimpleBehaviour;

public class IndicateTargetTank : MonoBehaviour, INode
{
    public float startSize;
    public TankControl target;
    private float startTime;

    private float DURATION = 1f;

    public void SetTarget(TankControl t)
    {
        target = t;
        startTime = Time.time;
        transform.position = t.transform.position;
        transform.localScale = Vector2.one * startSize;
    }

    TreeStatusEnum INode.Tick()
    {
        if (Time.time > startTime + DURATION)
        {
            GameController._instance.RemoveThingToDo(this);
            Destroy(this.gameObject);
            return TreeStatusEnum.SUCCESS;
        }
        transform.localScale = Vector2.one * Mathf.Lerp(startSize, 0, (Time.time - startTime) / DURATION);

        return TreeStatusEnum.RUNNING;
    }
}
