using System.Linq;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    private TurnState _turnState;
    public TurnState turnState
    {
        get => _turnState;

        private set
        {
            if (_turnState == value)
                return;
            
            if (value != TurnState.START)
            {
                onTurnEnd?.Invoke(_turnState);
            }
            Debug.Log($"Turn End: {_turnState} and Turn Start: {value}");
            _turnState = value;
            onTurnStart?.Invoke(_turnState);
        }
    }
    
    public enum TurnState { START, PLAYERTURN, ENEMYTURN, WAITING, LOSE, WIN }

    // note to self: "event" delegate types can only be invoked inside defined class (which we generally want)
    public delegate void TurnChangeEventHandler(TurnState turnState);
    public static event TurnChangeEventHandler onTurnStart;
    public static event TurnChangeEventHandler onTurnEnd;
    
    public delegate void AllyDoneTurn(int numAlliesDone, int numAllies);
    public static event AllyDoneTurn onAllyDoneTurn;
    
    public static TurnManager Instance { get; private set; }
    
    public void Awake()
    {
#region Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else if(Instance != this)
        {
            Destroy(this);
        }
#endregion
        
        BoardManager.OnBoardInitialized += StartGame;
    }
    
    void OnDestroy()
    {
#region Singleton
        Instance = null;
#endregion
    }
    
    private void StartGame()
    {
        // TODO: whatever we to happen before the player's first turn (after board is created)
        
        turnState = TurnState.START;

        PlayerTurn(); // TODO:!!!! putting this here as we have no start game logic yet
    }

    public void OnEndPlayerTurn()
    {
        // TODO: add waiting period with WAIT state
        EnemyTurn();
        
        PlayerTurn(); // TODO:!!!! this is until we have enemy AI logic for them to decide when the their turn ends
    }

    private void PlayerTurn()
    {
        turnState = TurnState.PLAYERTURN;
    }

    public void AllyFinishTurn()
    {
        int numAlliesDoneTurn = BoardManager.Instance.alliesOnBoard.Count(x => x.isDoneTurn);
        int numAllies = BoardManager.Instance.alliesOnBoard.Count;
        onAllyDoneTurn?.Invoke(numAlliesDoneTurn, numAllies);
    }

    private void EnemyTurn()
    {
        turnState = TurnState.ENEMYTURN;
    }
    
    private void Lose()
    {
        turnState = TurnState.LOSE;
    }
    
    private void Win()
    {
        turnState = TurnState.WIN;
    }
}
