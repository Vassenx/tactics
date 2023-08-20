using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : Character
{
    [Header("UI")]
    public ActionBar actionBar;

    public override void SelectCharacter()
    {
        base.SelectCharacter();

        actionBar.ShowActionBar();
    }
    
    public override void DeselectCharacter()
    {
        base.DeselectCharacter();
        
        actionBar.HideActionBar();
    }
}
