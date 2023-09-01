using System.Collections;
using System.Collections.Generic;
using System;
using TheKiwiCoder;
using UnityEngine;

public class EqualsNode<T> : DecoratorNode  where T : IEquatable<T> 
{
    public NodeProperty<T> keyToCompare;
    public NodeProperty<T> wantedValue;
    
    protected override void OnStart() {
    }
    
    protected override void OnStop() {
    }
    
    protected override State OnUpdate()
    {
        return wantedValue.Value.Equals(keyToCompare.Value) ? State.Success : State.Failure;
    }
}