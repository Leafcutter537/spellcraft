using TMPro;
using UnityEngine;

public class TooltipWarning : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI warningText;
    [SerializeField] private TooltipWarningEvent tooltipWarningEvent;

    private void OnEnable()
    {
        tooltipWarningEvent.AddListener(OnTooltipWarning);
    }
    private void OnDisable()
    {
        tooltipWarningEvent.RemoveListener(OnTooltipWarning);
    }
    private void OnTooltipWarning(object sender, TooltipWarningEventParameters args)
    {
        warningText.text = args.warningMessage;
    }
}
