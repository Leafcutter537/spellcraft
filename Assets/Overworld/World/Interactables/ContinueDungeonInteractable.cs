using System.Collections;
using System.Collections.Generic;
using Assets.Dungeons;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueDungeonInteractable : Interactable
{
    [SerializeField] private string worldSceneName;
    [SerializeField] private TextMeshProUGUI panelText;
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private LoadedDungeon loadedDungeon;

    public override void ShowInteractPanel()
    {
        base.ShowInteractPanel();
        if (loadedDungeon.currentLevel >= loadedDungeon.loadedDungeonData.dungeonLevels.Count)
        {
            panelText.text = "You have completed the dungeon!";
            buttonText.text = "Exit";
        }
        else
        {
            panelText.text = "The path leads farther into the dungeon.";
            buttonText.text = "Continue";
        }
    }

    public void ContinueInDungeon()
    {
        if (loadedDungeon.currentLevel >= loadedDungeon.loadedDungeonData.dungeonLevels.Count)
        {
            SceneManager.LoadScene(worldSceneName);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

}
