using TMPro;
using UnityEngine;

namespace TreasureHuntMiniGame.View
{
    public class CollectableView : MonoBehaviour
    {
        [SerializeField] private TMP_Text collectableName;
        [SerializeField] private int currentAmount;
        private string itemName; // Track the name separately

        public int CurrentAmount => currentAmount;

        public CollectableView InitializeCollectable(string name, int initialAmount = 0)
        {
            itemName = name;
            currentAmount = initialAmount;
            UpdateUI();

            return this;
        }

        public void UpdateCollectable(string name, int amount)
        {
            itemName = name;
            currentAmount = amount;
            UpdateUI();
        }

        public void IncrementAmount(int incrementBy)
        {
            currentAmount += incrementBy;
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (collectableName != null)
            {
                collectableName.text = $" {itemName} : {currentAmount}";
            }
            else
            {
                Debug.LogError("CollectableName TMP_Text reference is not set!");
            }
        }
    }
}