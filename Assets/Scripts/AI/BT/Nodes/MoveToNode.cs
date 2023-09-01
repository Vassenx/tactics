using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public class MoveToNode : ActionNode
{
    public NodeProperty<Cell> moveToCellKey;
    
    protected override void OnStart() {
    }
    
    protected override void OnStop() {
    }
    
    protected override State OnUpdate()
    {
        Cell moveToCell = moveToCellKey.Value;
        //if(context.character.)

        return State.Success;
    }
}
