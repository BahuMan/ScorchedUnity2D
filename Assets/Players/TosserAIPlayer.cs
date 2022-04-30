using System.Collections.Generic;
using UnityEngine;
using SimpleBehaviour;

public class TosserAIPlayer : MonoBehaviour, INode
{
    [SerializeField] IndicateTargetTank indicator;
    [SerializeField] int maxNrTries = 5;

    private TankControl myTank;
    private TankControl targetTank;
    private int currentNrTries = 0;
    private Vector3 lastShellLanded = new Vector3(-1f, -1f, -1f);
    private Vector3 previousShellLanded = new Vector3(-1f, -1f, -1f);

    private const float delaySeconds = 0.2f;

    private float oldAngle;
    private float newAngle;
    private float oldForce;
    private float newForce;

    private float lastAngleDelta;
    private float lastForceDelta;

    private bool newTurn = true;
    private float turnStartTime = 0f;

    void Start()
    {
        GetComponent<GenericPlayer>().SetInteraction(this);
    }

    TreeStatusEnum INode.Tick()
    {
        if (myTank == null)
        {
            myTank = GetComponent<GenericPlayer>().GetTank();
            myTank.OnShellFired.AddListener(tank_OnFired);
        }

        currentNrTries++;
        if (targetTank == null || currentNrTries > maxNrTries) ChooseTarget();
        if (newTurn == true) ChooseNewValues();

        float halfway = (Time.time - turnStartTime) / delaySeconds;
        myTank.Angle = Mathf.Lerp(oldAngle, newAngle, halfway);
        myTank.Force = Mathf.Lerp(oldForce, newForce, halfway);

        if (Time.time > (turnStartTime + delaySeconds))
        {
            GameController._instance.RemoveThingToDo(this);
            GameController._instance.addThingToDo(new TalkAndFire(myTank));
            newTurn = true; //next time we enter tick(), a new turn will have started
            return TreeStatusEnum.SUCCESS;
        }

        return TreeStatusEnum.RUNNING;
    }

    private TankControl ChooseTarget()
    {
        List<TankControl> potentials = new List<TankControl>(FindObjectsOfType<TankControl>());
        for (int i = 0; i < potentials.Count; i++) if (potentials[i] == myTank) potentials.RemoveAt(i);

        oldAngle = myTank.Angle;
        oldForce = myTank.Force;
        newAngle = Random.Range(30, 150f);
        newForce = Random.Range(150f, 400f);
        Debug.Log($"angle = {oldAngle} -> {newAngle}, force = {oldForce} -> {newForce} ");
        lastAngleDelta = lastForceDelta = 0f;
        previousShellLanded = lastShellLanded = new Vector3(-1, -1, -1);
        targetTank = potentials[Random.Range(0, potentials.Count)];
        currentNrTries = 0;
        return potentials[Random.Range(0, potentials.Count)];
    }

    private void IndicateTarget()
    {
        Debug.Log("I'm targetting " + targetTank.gameObject.name);
        IndicateTargetTank i = Instantiate<IndicateTargetTank>(indicator);
        i.SetTarget(targetTank);
        GameController._instance.addThingToDo(i);
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
        else if(targetTank == null)
        {
            Debug.Log("That's a hit!");
            return;
        }
        else if (256 > (this.transform.position - lastShellLanded).sqrMagnitude)
        {
            Debug.Log("That was too close to myself! Let's choose new target");
            targetTank = null;
        }
        else if (previousShellLanded.Equals(new Vector3(-1, -1, -1)))
        {
            Debug.Log("Shell landed at " + lastShellLanded + ", next shot is random again");
            //we can't calibrate yet; so next shot will be random again
            lastAngleDelta = Random.Range(-20f, 20f);
            lastForceDelta = Random.Range(-150f, 150f);
            newAngle = newAngle + lastAngleDelta;
            newForce = newForce + lastForceDelta;
        }
        else
        {
            //we calculate the next angle immediately after previous shell landed
            float previousDistance = (targetTank.transform.position - previousShellLanded).sqrMagnitude;
            float lastDistance = (targetTank.transform.position - lastShellLanded).sqrMagnitude;
            float myDistance = (this.transform.position - lastShellLanded).sqrMagnitude;

            oldAngle = newAngle;
            oldForce = newForce;
            if (previousDistance < lastDistance)
            {
                Debug.Log("getting worse, let's turn the angle/force");
                if (Random.Range(0, 2) == 0)
                {
                    lastAngleDelta = -lastAngleDelta / 2f;
                    newAngle += lastAngleDelta;
                }
                else
                {
                    lastForceDelta = -lastForceDelta / 2f;
                    newForce += lastForceDelta;
                }
            }
            else
            {
                Debug.Log("getting closer, let's do more delta:");
                newAngle = Mathf.Max(0f, oldAngle + lastAngleDelta);
                newForce = Mathf.Max(0f, oldForce + lastForceDelta);
            }
        }
    }

    //a new turn started; choose new angle/force and reset timer:
    private void ChooseNewValues()
    {
        IndicateTarget();
        previousShellLanded = lastShellLanded;
        oldAngle = myTank.Angle;
        oldForce = myTank.Force;
        turnStartTime = Time.time;
        newTurn = false;
    }
}
