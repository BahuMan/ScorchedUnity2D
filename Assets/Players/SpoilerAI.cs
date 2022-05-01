using System.Collections.Generic;
using UnityEngine;
using SimpleBehaviour;

public class SpoilerAI : MonoBehaviour, INode
{
    [SerializeField]
    private IndicateTargetTank indicator;
    [SerializeField]
    float aimTime = 1f;

    private enum AIStatusEnum { INIT, CHOOSETARGET, CALCULATE, AIMING, SHOOTING }
    private AIStatusEnum Status = AIStatusEnum.INIT;

    private TankControl myTank;
    private TankControl myTarget;

    void Start()
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

    private TreeStatusEnum Init()
    {
        myTank = GetComponent<GenericPlayer>().GetTank();
        Status = AIStatusEnum.CHOOSETARGET;
        return TreeStatusEnum.RUNNING;
    }

    private TreeStatusEnum ChooseTarget()
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

    float oldAngle = 0;
    float newAngle = 0;
    float oldForce = -1;
    float newForce = -1;
    float aimStartTime = -1;
    private TreeStatusEnum CalculateAngle()
    {
        oldAngle = myTank.Angle;
        oldForce = myTank.Force;
        newForce = Random.Range(100, Mathf.Min(myTank.HP, 400));

        Vector3 dir = myTarget.transform.position - myTank.transform.position;
        float vSqr = newForce * newForce / TankControl.ForceMultiplier / TankControl.ForceMultiplier;
        float g = -Physics2D.gravity.y;
        float root = vSqr * vSqr - g * (g * dir.x * dir.x + 2 * dir.y * vSqr);

        if (root < 0)
        {
            Debug.Log("Not enough power to reach target");
            newAngle = 45f;
        }
        else
        {
            newAngle = Mathf.Atan2( vSqr + Mathf.Sqrt(root), g * dir.x) * Mathf.Rad2Deg;
            Debug.Log($"Alternative angle : {Mathf.Atan2(vSqr - Mathf.Sqrt(root), g * dir.x) * Mathf.Rad2Deg}");
        }
        newForce *= 1.2f;
        this.Status = AIStatusEnum.AIMING;
        return TreeStatusEnum.SUCCESS;
    }

    private TreeStatusEnum Aiming()
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

    private TreeStatusEnum Shooting()
    {

        if (myTarget == null)
        {
            Debug.Log("Shooting a dead target won't do anything");
            Status = AIStatusEnum.CHOOSETARGET;
            return TreeStatusEnum.RUNNING;
        }

        //shooting is the end of my turn, so remove myself from todo stack:
        GameController._instance.RemoveThingToDo(this);
        myTank.Fire();

        //when it's my turn again, I want to choose a new target:
        this.Status = AIStatusEnum.CHOOSETARGET;
        return TreeStatusEnum.SUCCESS;
    }
}