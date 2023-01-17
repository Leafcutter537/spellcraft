using Assets.EventSystem;
using Assets.Inventory.Runes;
using UnityEngine;
using UnityEngine.UI;

public class RuneDragImage : MonoBehaviour
{

    [Header("Image Reference")]
    [SerializeField] private Image runeImage;  
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
        RuneEventParameters runeEventParams = args as RuneEventParameters;
        if (runeEventParams != null)
        {
            runeImage.sprite = runeEventParams.rune.icon;
        }
    }
}
