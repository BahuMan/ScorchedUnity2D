using System.Collections.Generic;
using UnityEngine;
using SimpleBehaviour;

public class EmptyAI : MonoBehaviour, INode
{
    [SerializeField]
    private IndicateTargetTank indicator;
    [SerializeField]
    float aimTime = 1f;

    protected enum AIStatusEnum { INIT, CHOOSETARGET, CALCULATE, AIMING, SHOOTING }
    protected AIStatusEnum Status = AIStatusEnum.INIT;

    protected TankControl myTank;
    protected TankControl myTarget;

    virtual protected void Start()
    {
        GetComponent<GenericPlayer>().SetInteraction(this);
    }

    TreeStatusEnum INode.Tick()
    {
        if (myTank == null)
        {
            Debug.Log("Are we starting a new round?");
            Status = AIStatusEnum.INIT;
        }

        switch (Status)
        {
            case AIStatusEnum.INIT: return Init();
            case AIStatusEnum.CHOOSETARGET: return ChooseTarget();
            case AIStatusEnum.CALCULATE: return CalculateAngle();
            case AIStatusEnum.AIMING: return Aiming();
            case AIStatusEnum.SHOOTING: return Shooting();
            default: return TreeStatusEnum.FAILURE;
        }
    }

    virtual protected TreeStatusEnum Init()
    {
        myTank = GetComponent<GenericPlayer>().GetTank();
        Status = AIStatusEnum.CHOOSETARGET;
        return TreeStatusEnum.RUNNING;
    }

    virtual protected TreeStatusEnum ChooseTarget()
    {
        List<TankControl> potentials = new List<TankControl>(FindObjectsOfType<TankControl>());
        potentials.Remove(myTank);
        myTarget = potentials[Random.Range(0, potentials.Count)];

        IndicateTargetTank t = Instantiate<IndicateTargetTank>(indicator);
        t.SetTarget(myTarget);
        GameController._instance.addThingToDo(t);

        this.Status = AIStatusEnum.CALCULATE;
        return TreeStatusEnum.SUCCESS;
    }

    protected float oldAngle = 0;
    protected float newAngle = 0;
    protected float oldForce = -1;
    protected float newForce = -1;
    protected float aimStartTime = -1;
    virtual protected TreeStatusEnum CalculateAngle()
    {
        if (myTarget == null)
        {
            Debug.Log("calculating angle for a dead target won't do anything");
            Status = AIStatusEnum.CHOOSETARGET;
            return TreeStatusEnum.RUNNING;
        }

        oldAngle = myTank.Angle;
        oldForce = myTank.Force;
        newForce = Random.Range(100, Mathf.Min(myTank.HP, 400));

        float ignoreLow;
        InnerCalculateAngle(out ignoreLow, out newAngle);
        newForce *= 1.2f;
        this.Status = AIStatusEnum.AIMING;
        return TreeStatusEnum.SUCCESS;
    }

    protected bool InnerCalculateAngle(out float low, out float high)
    {
        Vector3 dir = myTarget.transform.position - myTank.transform.position;
        float vSqr = newForce * newForce / TankControl.ForceMultiplier / TankControl.ForceMultiplier;
        float g = -Physics2D.gravity.y;
        float root = vSqr * vSqr - g * (g * dir.x * dir.x + 2 * dir.y * vSqr);

        if (root < 0)
        {
            Debug.Log("Not enough power to reach target");
            low = 45f;
            high = 135f;
            return false;
        }
        else
        {
            high = Mathf.Atan2(vSqr + Mathf.Sqrt(root), g * dir.x) * Mathf.Rad2Deg;
            low = Mathf.Atan2(vSqr - Mathf.Sqrt(root), g * dir.x) * Mathf.Rad2Deg;
            Debug.Log($"2 angles : {high} and {low}");
            return true;
        }
    }

    virtual protected TreeStatusEnum Aiming()
    {
        if (aimStartTime < 0)
        {
            aimStartTime = Time.time;
        }

        float halfway = (Time.time - aimStartTime) / aimTime;
        myTank.Angle = Mathf.Lerp(oldAngle, newAngle, halfway);
        myTank.Force = Mathf.Lerp(oldForce, newForce, halfway);

        if (Time.time > (aimStartTime + aimTime))
        {
            aimStartTime = -1; //reset for next aim animation
            Status = AIStatusEnum.SHOOTING;
            return TreeStatusEnum.RUNNING;
        }

        return TreeStatusEnum.RUNNING;
    }

    virtual protected TreeStatusEnum Shooting()
    {
        //since the empty AI does not typically recalculate after firing,
        //we need to check before firing whether the target is still alive
        if (myTarget == null)
        {
            Debug.Log("Shooting a dead target won't do anything");
            Status = AIStatusEnum.CHOOSETARGET;
            return TreeStatusEnum.RUNNING;
        }

        //shooting is the end of my turn, so remove myself from todo stack:
        GameController._instance.RemoveThingToDo(this);
        myTank.Fire();

        return TreeStatusEnum.SUCCESS;
    }
}
