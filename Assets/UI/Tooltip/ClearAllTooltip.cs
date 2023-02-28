using System.Collections;
using System.Collections.Generic;
using Assets.EventSystem;
using TMPro;
using UnityEngine;

public class ClearAllTooltip : MonoBehaviour
{
    private TextMeshProUGUI[] allTooltipText;
    [SerializeField] private ExitTooltipEvent exitTooltipEvent;

    private void Awake()
    {
        allTooltipText = GetComponentsInChildren<TextMeshProUGUI>();
    }
    private void OnEnable()
    {
        exitTooltipEvent.AddListener(OnExitTooltip);
    }
    private void OnDisable()
    {
        exitTooltipEvent.RemoveListener(OnExitTooltip);
    }

    private void OnExitTooltip(object sender, EventParameters args)
    {
        foreach (TextMeshProUGUI tooltipTtext in allTooltipText)
        {
            tooltipTtext.text = "";
        }
    }
}
