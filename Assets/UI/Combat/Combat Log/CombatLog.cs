using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombatLog : MonoBehaviour
{
    [SerializeField] private CombatLogMessageEvent combatLogMessageEvent;
    [SerializeField] private int numMessages;
    private List<string> messages;
    [SerializeField] private TextMeshProUGUI combatLogText;

    private void Awake()
    {
        messages = new List<string>();
    }
    private void OnEnable()
    {
        combatLogMessageEvent.AddListener(OnCombatLogMessage);
    }
    private void OnDisable()
    {
        combatLogMessageEvent.RemoveListener(OnCombatLogMessage);
    }
    public void PrintMessages()
    {
        combatLogText.text = "";
        for (int i = 0; i < messages.Count; i++)
        {
            combatLogText.text += messages[i] + "\n\n";
        }
    }
    private void AddMessage(string messageString)
    {
        messages.Add(messageString);
        if (messages.Count > numMessages)
        {
            messages.RemoveAt(0);
        }
    }
    private void OnCombatLogMessage(object sender, CombatLogEventParameters args)
    {
        AddMessage(args.messageString);
        PrintMessages();
    }
}
