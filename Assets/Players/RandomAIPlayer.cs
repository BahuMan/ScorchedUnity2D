using SimpleBehaviour;
using UnityEngine;


public class RandomAIPlayer : MonoBehaviour, SimpleBehaviour.INode
{
    private TankControl myTank;
    [SerializeField]
    private float delaySeconds;

    private float oldAngle;
    private float newAngle;
    private float oldForce;
    private float newForce;
    private bool newTurn = true;
    private float turnStartTime = 0f;

    void Start()
    {
        GetComponent<GenericPlayer>().SetInteraction(this);
    }


    TreeStatusEnum INode.Tick()
    {
        if (myTank == null) myTank = GetComponent<GenericPlayer>().GetTank();
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

    //a new turn started; choose new angle/force and reset timer:
    private void ChooseNewValues()
    {
        oldAngle = myTank.Angle;
        oldForce = myTank.Force;
        newAngle = Random.Range(-10f, 190f);
        newForce = Random.Range(5f, 50f);
        Debug.Log("My turn, now! angle = " + oldAngle + " -> " + newAngle + ", force = " + oldForce + " -> " + newForce);
        turnStartTime = Time.time;
        newTurn = false;
    }
}
