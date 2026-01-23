using UnityEngine;
using UnityEngine.UI;

public class ChestView : MonoBehaviour
{
    [SerializeField] private Image _chestImage;
    [SerializeField] private Button _chestButton;
    [SerializeField] private Slider _progressSlider;
    [SerializeField] private GameObject _winningEffect;
    
    private ChestModel _model;
    
    public void Initialize(ChestModel model)
    {
        _model = model;
        
        _model.OnStateChanged += HandleStateChanged;
        
        UpdateVisuals();
    }
    
    private void HandleStateChanged(ChestModel model)
    {
        UpdateVisuals();
    }
    
    private void UpdateVisuals()
    {
        // Update sprite based on state
        _progressSlider.gameObject.SetActive(_model.State == ChestState.Opening);
        _winningEffect.SetActive(_model.State == ChestState.Winning);
        
        // Enable/disable interaction
        _chestButton.interactable = _model.State == ChestState.Closed;
    }
}