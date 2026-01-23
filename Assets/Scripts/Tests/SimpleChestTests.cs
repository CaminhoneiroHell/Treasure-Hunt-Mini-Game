using UnityEngine;
using DG.Tweening;
using QFSW.QC;

public class SimpleChestTests : MonoBehaviour
{
    public RectTransform lid;
    public float openTime = 3f;
    public float cancelWindow = 2.8f;
    
    private bool isOpening = false;
    private bool isOpen = false;
    private float openingStartTime;
    
    [Command]
    public void Open()
    {
        if (isOpen || isOpening) return;
        
        isOpening = true;
        isOpen = false;
        openingStartTime = Time.time;
        
        lid.DOKill();
        lid.DOSizeDelta(new Vector2(lid.sizeDelta.x, 40f), openTime)
            .SetEase(Ease.OutElastic, 1.2f)
            .OnComplete(() => {
                isOpening = false;
                isOpen = true;
                Debug.Log("Chest is Open");
            });
    }
    
    [Command]
    public void Close()
    {
        if (!isOpen && !isOpening) return;
        
        lid.DOKill();
        lid.DOSizeDelta(new Vector2(lid.sizeDelta.x, 0f), 0.4f)
            .SetEase(Ease.InBack)
            .OnComplete(() => {
                isOpening = false;
                isOpen = false;
                Debug.Log("Chest is Closed");
            });
    }
    
    [Command]
    public void CancelOpen()
    {
        if (!isOpening || Time.time - openingStartTime > cancelWindow)
        {
            Debug.Log("Too late to cancel!");
            return;
        }
        
        float progress = (Time.time - openingStartTime) / openTime;
        float cancelTime = Mathf.Lerp(0.1f, 0.3f, progress);
        
        lid.DOKill();
        lid.DOSizeDelta(new Vector2(lid.sizeDelta.x, 0f), cancelTime)
            .SetEase(Ease.InQuad)
            .OnComplete(() => {
                isOpening = false;
                isOpen = false;
            });
    }
}