using UnityEngine;
using SimpleBehaviour;
using System.Collections.Generic;
using System;

public class ShooterAIPlayer : MonoBehaviour, INode
{
    [SerializeField]
    private IndicateTargetTank indicator;
    [SerializeField]
    float aimTime = 1f;

    private enum AIStatusEnum { INIT, CHOOSETARGET, AIMING, SHOOTING }
    private AIStatusEnum Status = AIStatusEnum.INIT;

    private TankControl myTank;
    private TankControl myTarget;

    void Start()
    {
        GetComponent<GenericPlayer>().SetInteraction(this);
    }

    TreeStatusEnum INode.Tick()
    {
        switch (Status)
        {
            case AIStatusEnum.INIT: return Init();
            case AIStatusEnum.CHOOSETARGET: return ChooseTarget();
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
        float closestDistance = float.MaxValue;
        TankControl closestTank = null;
        int p = 0;
        Vector3 myPos = myTank.transform.position;
        while (p < potentials.Count)
        {
            if (potentials[p] != myTank)
            {
                float distance = (myPos - potentials[p].transform.position).sqrMagnitude;
                if (distance < closestDistance) closestTank = potentials[p];
            }
            p++;
        }
        myTarget = closestTank;
        IndicateTargetTank t = Instantiate<IndicateTargetTank>(indicator);
        t.SetTarget(myTarget);
        GameController._instance.addThingToDo(t);
        Status = AIStatusEnum.AIMING;
        return TreeStatusEnum.RUNNING;
    }

    float oldAngle = 0;
    float newAngle = 0;
    float oldForce = -1;
    float aimStartTime = -1;
    private TreeStatusEnum Aiming()
    {
        if (aimStartTime < 0)
        {
            aimStartTime = Time.time;
            oldAngle = myTank.Angle;
            oldForce = myTank.Force;
            newAngle = CalculateAngle(myTarget);
        }

        float halfway = (Time.time - aimStartTime) / aimTime;
        myTank.Angle = Mathf.Lerp(oldAngle, newAngle, halfway);
        myTank.Force = Mathf.Lerp(oldForce, myTank.HP, halfway);

        if (Time.time > (aimStartTime + aimTime))
        {
            aimStartTime = -1; //reset for next aim animation
            Status = AIStatusEnum.SHOOTING;
            return TreeStatusEnum.RUNNING;
        }
        
        return TreeStatusEnum.RUNNING;
    }

    private float CalculateAngle(TankControl target)
    {
        Vector3 towardsTarget = target.transform.position - myTank.transform.position;
        //Quaternion aim = Quaternion.LookRotation(towardsTarget, Vector3.up);
        float a = Vector3.Angle(Vector3.right, towardsTarget);
        Debug.Log("Calculated angle " + a);
        return a;
    }

    private TreeStatusEnum Shooting()
    {
        if (myTank == null)
        {
            Debug.Log("Are we starting a new round?");
            Status = AIStatusEnum.INIT;
            return TreeStatusEnum.RUNNING;
        }
        if (myTarget==null)
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

