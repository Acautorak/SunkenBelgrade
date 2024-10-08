using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance { get; private set; }
    private int turnNumber = 1;
    private int roundNumber = 1;
    private bool isPlayerTurn = true;

    public event EventHandler OnTurnChanged;
    public event EventHandler OnRoundChanged;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Puko ti je TurnSystem singleton");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public void NextTurn()
    {
        turnNumber++;
        isPlayerTurn = !isPlayerTurn;

        OnTurnChanged?.Invoke(this, EventArgs.Empty);
    }

    public void NextRound()
    {
        roundNumber++;
        OnRoundChanged?.Invoke(this, EventArgs.Empty);
        if (roundNumber > UnitManager.Instance.GetUnitList().Count)
        {
            roundNumber = 1;
            NextTurn();
        }
    }

    public int GetTurnNumber()
    {
        return turnNumber;
    }

    public bool IsPlayerTurn()
    {
        return isPlayerTurn;
    }

    public int GetRoundNumber()
    {
        return roundNumber;
    }
}
