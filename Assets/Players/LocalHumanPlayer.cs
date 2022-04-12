using SimpleBehaviour;
using UnityEngine;

public class LocalHumanPlayer : MonoBehaviour, SimpleBehaviour.INode
{

    private TankControl myTank;

    void Start()
    {
        GetComponent<GenericPlayer>().SetInteraction(this);
    }

    TreeStatusEnum INode.Tick()
    {
        if (myTank == null) myTank = GetComponent<GenericPlayer>().GetTank();

        if (Mathf.Abs(Input.GetAxis("Horizontal")) > .1f)
        {
            myTank.Angle -= Input.GetAxis("Horizontal");
            myTank.Angle = System.Math.Min(190f, System.Math.Max(-10f, myTank.Angle));
        }
        if (Mathf.Abs(Input.GetAxis("Vertical")) > .1f)
        {
            myTank.Force += Input.GetAxis("Vertical");
            myTank.Force = System.Math.Min(1000f, System.Math.Max(0f, myTank.Force));
        }

        if (Input.GetButtonDown("Fire1"))
        {
            GameController._instance.RemoveThingToDo(this);
            GameController._instance.addThingToDo(new TalkAndFire(myTank));
            return TreeStatusEnum.SUCCESS;
        }
        else
        {
            return TreeStatusEnum.RUNNING;
        }

    }

}
