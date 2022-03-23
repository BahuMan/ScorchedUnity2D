using SimpleBehaviour;
using UnityEngine;

public class HumanInteraction : MonoBehaviour, SimpleBehaviour.INode
{

    private TankControl myTank;
    private void Awake()
    {
        myTank = GetComponent<TankControl>();
        myTank.SetInteraction(this);
    }

    TreeStatusEnum INode.Tick()
    {
        if (Mathf.Abs(Input.GetAxis("Horizontal")) > .1f)
        {
            myTank.angle -= Input.GetAxis("Horizontal");
            myTank.angle = System.Math.Min(190f, System.Math.Max(-10f, myTank.angle));
        }
        if (Mathf.Abs(Input.GetAxis("Vertical")) > .1f)
        {
            myTank.force += Input.GetAxis("Vertical");
            myTank.force = System.Math.Min(1000f, System.Math.Max(0f, myTank.force));
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
