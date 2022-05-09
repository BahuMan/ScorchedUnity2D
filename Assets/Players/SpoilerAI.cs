using System.Collections.Generic;
using UnityEngine;
using SimpleBehaviour;

public class SpoilerAI : EmptyAI, INode
{

    protected override TreeStatusEnum Shooting()
    {
        this.Status = AIStatusEnum.CALCULATE;
        return base.Shooting();
    }
}