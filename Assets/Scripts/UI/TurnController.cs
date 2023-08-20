using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TurnController : MonoBehaviour
    {
        [SerializeField] private Button endTurnButton;

        private void Awake()
        {
            TurnManager.onTurnStart += OnPlayerTurnStart;
            TurnManager.onTurnEnd += OnPlayerTurnEnd;
            endTurnButton.onClick.AddListener(OnEndTurnClicked);
        }

        private void OnDestroy()
        {
            TurnManager.onTurnStart -= OnPlayerTurnStart;
            TurnManager.onTurnEnd -= OnPlayerTurnEnd;
            endTurnButton.onClick.RemoveListener(OnEndTurnClicked);
        }

        private void OnEndTurnClicked() => TurnManager.Instance.OnEndPlayerTurn();

        private void OnPlayerTurnStart(TurnManager.TurnState turn)
        {
            if (turn != TurnManager.TurnState.PLAYERTURN)
                return;

            endTurnButton.interactable = true;
        }
        
        private void OnPlayerTurnEnd(TurnManager.TurnState turn)
        {
            if (turn != TurnManager.TurnState.PLAYERTURN)
                return;

            endTurnButton.interactable = false;
        }
    }
}
