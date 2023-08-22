using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TurnController : MonoBehaviour
    {
        [SerializeField] private Button endTurnButton;
        [SerializeField] private GameObject notDoneTurnCountParent;
        [SerializeField] private TextMeshProUGUI notDoneTurnCountText;

        private void Awake()
        {
            TurnManager.onTurnStart += OnPlayerTurnStart;
            TurnManager.onTurnEnd += OnPlayerTurnEnd;
            endTurnButton.onClick.AddListener(OnEndTurnClicked);

            TurnManager.onAllyDoneTurn += OnAllyDoneTurn;
        }

        private void OnDestroy()
        {
            TurnManager.onTurnStart -= OnPlayerTurnStart;
            TurnManager.onTurnEnd -= OnPlayerTurnEnd;
            endTurnButton.onClick.RemoveListener(OnEndTurnClicked);
            
            TurnManager.onAllyDoneTurn -= OnAllyDoneTurn;
        }

        private void OnEndTurnClicked() => TurnManager.Instance.OnEndPlayerTurn();

        private void OnPlayerTurnStart(TurnManager.TurnState turn)
        {
            if (turn != TurnManager.TurnState.PLAYERTURN)
                return;

            endTurnButton.interactable = false;
            
            notDoneTurnCountParent.SetActive(true);
            notDoneTurnCountText.text = BoardManager.Instance.alliesOnBoard.Count.ToString();
        }
        
        private void OnPlayerTurnEnd(TurnManager.TurnState turn)
        {
            if (turn != TurnManager.TurnState.PLAYERTURN)
                return;

            endTurnButton.interactable = false;
        }

        private void OnAllyDoneTurn(int numAlliesDone, int numAllies)
        {
            int numAlliesNotDone = numAllies - numAlliesDone;
            if (numAlliesNotDone <= 0)
            {
                notDoneTurnCountParent.SetActive(false);
                endTurnButton.interactable = true;
                return;
            }
            
            notDoneTurnCountText.text = numAlliesNotDone.ToString();
        }
    }
}
