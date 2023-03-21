using System.Collections;
using System.Collections.Generic;
using Assets.Combat;
using Assets.Dungeons;
using Assets.Progression;
using UnityEngine;

public class DungeonInteractable : Interactable
{
    public DungeonData dungeonData;
    [SerializeField] private LoadedDungeon loadedDungeon;
    
    public override void ShowInteractPanel()
    {
        base.ShowInteractPanel();
        loadedDungeon.currentLevel = 0;
        loadedDungeon.loadedDungeonData = dungeonData;
    }
    
}
