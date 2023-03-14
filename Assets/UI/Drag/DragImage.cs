using Assets.EventSystem;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DragImage : MonoBehaviour
{
    [Header("Object Reference")]
    [SerializeField] private Image image;
    [SerializeField] private Image symbolImage;
    [Header("Event References")]
    [SerializeField] private StartDragEvent startDragEvent;
    private void OnEnable()
    {
        startDragEvent.AddListener(OnStartDrag);
    }
    private void OnDisable()
    {
        startDragEvent.RemoveListener(OnStartDrag);
    }
    private void OnStartDrag(object sender, EventParameters args)
    {
        SelectPanelChoice draggedChoice = sender as SelectPanelChoice;
        image.sprite = draggedChoice.selectChoice.icon;
        if (draggedChoice.selectChoice.secondaryIcon != null)
            symbolImage.sprite = draggedChoice.selectChoice.secondaryIcon;
    }
}
