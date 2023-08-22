using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : Character
{
    [Header("UI")]
    public ActionBar actionBar;

    protected override void Start()
    {
        base.Start();
        TurnManager.onTurnEnd += ResetTurn;
    }

    protected void OnDestroy()
    {
        TurnManager.onTurnEnd -= ResetTurn;
    }

    public override bool isDoneTurn
    {
        get { return base.isDoneTurn; }
        protected set
        {
            bool wasDone = isDoneTurn;
            base.isDoneTurn = value;
            
            if (!wasDone && isDoneTurn)
            {
                TurnManager.Instance.AllyFinishTurn(); 
            }
        }
    }
    
    public override void SelectCharacter()
    {
        base.SelectCharacter();

        if (!isDoneTurn)
        {
            actionBar.ShowActionBar();
        }
    }
    
    public override void DeselectCharacter()
    {
        base.DeselectCharacter();
        
        actionBar.HideActionBar();
    }

    public void TurnSkipped()
    {
        isDoneTurn = true;
    }

    public void ResetTurn(TurnManager.TurnState turnToEnd)
    {
        if (turnToEnd != TurnManager.TurnState.PLAYERTURN)
            return;
        
        isDoneTurn = false;
    }
}
