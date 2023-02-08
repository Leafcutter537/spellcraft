using System.Collections;
using System.Collections.Generic;
using Assets.Combat;
using UnityEngine;

public class PlayerStatPanel : StatPanel
{
    [Header("References")]
    [SerializeField] private PlayerInstance playerInstance;

    public override void ShowStatInfo()
    {
        ShowStatInfo(playerInstance.characterName, playerInstance.currentHP, playerInstance.maxHP, playerInstance.currentMP, playerInstance.maxMP);
    }
}
