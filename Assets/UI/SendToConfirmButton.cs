using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendToConfirmButton : MonoBehaviour
{
    [SerializeField] private SelectPanelChoice selectPanelChoice;
    [SerializeField] private ConfirmSelectChoice confirmSelectChoice;

    public void OnClick()
    {
        confirmSelectChoice.gameObject.SetActive(true);
        confirmSelectChoice.TakeSelectChoice(selectPanelChoice.selectChoice);
    }
}