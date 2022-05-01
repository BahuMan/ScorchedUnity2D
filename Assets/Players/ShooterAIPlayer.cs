using UnityEngine;
using SimpleBehaviour;


public class ShooterAIPlayer : EmptyAI, INode
{

    override protected TreeStatusEnum CalculateAngle()
    {
        oldAngle = myTank.Angle;
        oldForce = myTank.Force;
        newForce = myTank.HP;

        float ignoreHigh;
        InnerCalculateAngle(out newAngle, out ignoreHigh);
        newForce *= 1.2f;
        this.Status = AIStatusEnum.AIMING;
        return TreeStatusEnum.SUCCESS;
    }

    protected override TreeStatusEnum Shooting()
    {
        base.Shooting();
        if (myTarget != null) this.Status = AIStatusEnum.SHOOTING;
        return TreeStatusEnum.SUCCESS;
    }
}

