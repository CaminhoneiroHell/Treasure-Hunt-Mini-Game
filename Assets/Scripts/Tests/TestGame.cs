using System.Collections.Generic;
using QFSW.QC;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TestGame : MonoBehaviour
{
    [Header("HUD References")] 
    [SerializeField] private TMP_Text contextualMessages;
    [SerializeField] private string[] gameUpdateText; 
    
    [Header("Collectables References")] 
    [SerializeField] private Transform contentHUD;
    [SerializeField] private GameObject collectableEntryView;

    [Header("Game References")] 
    [SerializeField] private Transform contentGame;
    [SerializeField] private GameObject chestEntryView;

    // public void DisplayEntries(IReadOnlyList<LeaderboardEntry> entries)
    // {
    //     ClearEntries();
    //
    //     foreach (var entry in entries)
    //     {
    //         CreateEntryView(entry);
    //     
    //         // Highlight player entry
    //         if (entry.playerName == "YOU!!!")
    //         {
    //             var entryView = entryViews[entryViews.Count - 1];
    //             entryView.SetHighlighted(true);
    //         }
    //     }
    // }

    // public void ClearEntries()
    // {
    //     foreach (var entryView in entryViews)
    //     {
    //         if (entryView != null && entryView.gameObject != null)
    //             Destroy(entryView.gameObject);
    //     }
    //     entryViews.Clear();
    // }
    

    [Command]
    private void UpdateContextHUD(int textType)    
    {
        if(textType > 2 || textType < 0)
            Debug.Log($"Error, text type is only (0) to clear text, (1) Success Message and (2) GameOver Message");
        else
            contextualMessages.text = gameUpdateText[textType];
    }

    [Command]
    private void CreateCollectableView(int num)    
    {
        for(int i = 0; i < num; i++)
            Instantiate(collectableEntryView, contentHUD);
    }
    
    [Command]
    private void CreateChestView(int num)
    {
        for(int i = 0; i < num; i++) 
            Instantiate(chestEntryView, contentGame);
    }
}