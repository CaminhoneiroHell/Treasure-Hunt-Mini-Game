using System;
using UnityEngine;

// ChestModel.cs
public class ChestModel
{
    public string Id { get; private set; }
    public ChestState State { get; private set; }
    public bool IsWinning { get; private set; }
    public float OpeningProgress { get; private set; }
    
    public event Action<ChestModel> OnStateChanged;
    public event Action<float> OnProgressChanged;
    
    public ChestModel(string id, bool isWinning)
    {
        Id = id;
        IsWinning = isWinning;
        State = ChestState.Closed;
    }
    
    public void SetState(ChestState newState)
    {
        if (State == newState) return;
        
        State = newState;
        OnStateChanged?.Invoke(this);
    }
    
    public void UpdateOpeningProgress(float progress)
    {
        OpeningProgress = Mathf.Clamp01(progress);
        OnProgressChanged?.Invoke(OpeningProgress);
    }

    public void SetWinning(bool b)
    {
        Debug.Log("WIN!");
    }
}

public enum ChestState
{
    Closed,
    Opening,
    Opened,
    Winning
}