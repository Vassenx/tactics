using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : Character
{
    public ActionBar actionBar;

    public override void SelectCharacter()
    {
        base.SelectCharacter();
        
        // TODO: if is my turn:
        actionBar.ShowActionBar();
    }
    
    public override void DeselectCharacter()
    {
        base.DeselectCharacter();
        
        actionBar.HideActionBar();
    }
}
