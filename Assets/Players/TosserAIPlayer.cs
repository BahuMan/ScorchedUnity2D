using System.Collections.Generic;
using UnityEngine;
using SimpleBehaviour;

public class TosserAIPlayer : EmptyAI, INode
{
    [SerializeField] int maxNrTries = 5;


    private int currentNrTries = 0;
    private Vector3 lastShellLanded = new Vector3(-1f, -1f, -1f);
    private Vector3 previousShellLanded = new Vector3(-1f, -1f, -1f);

    //private float lastAngleDelta;
    private float lastForceDelta;

    protected override TreeStatusEnum Init()
    {
        base.Init();
        //base init already changes this.Status = AIStatusEnum.CHOOSE_TARGET
        //but we want to add a listener to the tank so we can watch the shell fly
        myTank.OnShellFired.AddListener(tank_OnFired);
        return TreeStatusEnum.RUNNING;

    }
    protected override TreeStatusEnum ChooseTarget()
    {
        currentNrTries = 0;
        //lastAngleDelta = float.MaxValue;
        lastForceDelta = 100f;
        return base.ChooseTarget();
    }

    protected override TreeStatusEnum CalculateAngle()
    {
        //first guess is random
        if (currentNrTries == 0) return base.CalculateAngle();
        if (myTarget == null || currentNrTries > maxNrTries)
        {
            this.Status = AIStatusEnum.CHOOSETARGET;
            return TreeStatusEnum.RUNNING;
        }

        float ignoreLow = 0f;
        while (!InnerCalculateAngle(out ignoreLow, out newAngle))
        {
            newForce *= 1.5f;
            Debug.Log("Recalculating with force " + newForce);
        }
        //newForce += Random.Range(-10f, 10f);

        previousShellLanded = lastShellLanded;
        this.Status = AIStatusEnum.AIMING;
        return TreeStatusEnum.SUCCESS;
    }

    protected override TreeStatusEnum Shooting()
    {
        this.Status = AIStatusEnum.CALCULATE;
        currentNrTries++;
        return base.Shooting();
    }

    private void tank_OnFired(Rigidbody2D shell)
    {
        GameController._instance.addThingToDo(new TrackShell(shell, TrackShell_UpdateShellPosition));
    }

    private void TrackShell_UpdateShellPosition(Vector3 position, bool dead)
    {
        if (!dead)
        {
            //missile in-flight; let's track position
            lastShellLanded = position;
        }
        //missile landed, ignore "current" position and use lastShellLanded
        else if(myTarget == null)
        {
            Debug.Log("That's a hit!");
            this.Status = AIStatusEnum.CHOOSETARGET;
            return;
        }
        else if (256 > (myTank.transform.position - lastShellLanded).sqrMagnitude)
        {
            Debug.Log($"{(myTank.transform.position - lastShellLanded).sqrMagnitude} was too close to myself! Let's choose new target");
            this.Status = AIStatusEnum.CHOOSETARGET;
            myTarget = null;
        }
        else if (previousShellLanded.Equals(new Vector3(-1, -1, -1)))
        {
            Debug.Log("Shell landed at " + lastShellLanded + ", next shot is random again");
            //we can't calibrate yet; so next shot will be random again
            //lastAngleDelta = Random.Range(-20f, 20f);
            lastForceDelta = Random.Range(-150f, 150f);
            //newAngle = newAngle + lastAngleDelta;
            newForce = newForce + lastForceDelta;
        }
        else
        {
            //we calculate the next angle immediately after previous shell landed
            float previousDistance = (myTarget.transform.position - previousShellLanded).sqrMagnitude;
            float lastDistance = (myTarget.transform.position - lastShellLanded).sqrMagnitude;
            float myDistance = (this.transform.position - lastShellLanded).sqrMagnitude;

            oldAngle = newAngle;
            oldForce = newForce;
            if (previousDistance < lastDistance)
            {
                Debug.Log("getting worse, let's turn the angle/force");
                //if (Random.Range(0, 2) == 0)
                //{
                //    lastAngleDelta = -lastAngleDelta / 2f;
                //    newAngle += lastAngleDelta;
                //}
                //else
                //{
                    lastForceDelta = -lastForceDelta / 2f;
                    newForce += lastForceDelta;
                //}
            }
            else
            {
                Debug.Log("getting closer, let's do more delta:");
                lastForceDelta *= 1.5f;
                //newAngle = Mathf.Max(0f, oldAngle + lastAngleDelta);
                newForce = Mathf.Max(0f, oldForce + lastForceDelta);
            }
        }
    }

}
