using System.Collections;
using System.Collections.Generic;
using Assets.Combat;
using Assets.Dungeons;
using Assets.Equipment;
using Assets.Progression;
using UnityEngine;

public class DungeonController : MonoBehaviour
{
    [SerializeField] private EnemyInteractable enemyInteractable;
    [SerializeField] private LoadedDungeon loadedDungeon;
    [SerializeField] private ProgressTracker progressTracker;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Vector2Int defeatedEnemyPosition;
    [SerializeField] private ChestInteractable chest;
    [SerializeField] private InventoryController inventoryController;
    private void Awake()
    {
        if (progressTracker.justDefeatedEnemy & progressTracker.justDefeatedEnemyIndex == -1)
        {
            progressTracker.justDefeatedEnemy = false;
            playerMovement.SetLocation(defeatedEnemyPosition);
            Destroy(enemyInteractable.gameObject);
            chest.rewardData = GetRandomRewardReplaceOwnedEquipment();
            loadedDungeon.currentLevel++;
        }
        else
        {
            enemyInteractable.enemyStats = GetRandomEnemyStats();
        }
    }

    private EnemyStats GetRandomEnemyStats()
    {
        DungeonLevel currentDungeonLevel = loadedDungeon.loadedDungeonData.dungeonLevels[loadedDungeon.currentLevel];
        int numChoices = currentDungeonLevel.possibleEnemies.Count;
        System.Random rand = new System.Random();
        float randomFloat = (float)rand.NextDouble();
        int i = 0;
        while (i < numChoices)
        {
            float lowerBound = ((float)i / numChoices);
            float upperBound = (float)(i + 1) / numChoices;
            if ((randomFloat > lowerBound) & (randomFloat < upperBound))
                return currentDungeonLevel.possibleEnemies[i];
            i++;
        }
        return currentDungeonLevel.possibleEnemies[numChoices - 1];
    }

    private RewardData GetRandomRewardReplaceOwnedEquipment()
    {
        while (true)
        {
            RewardData rewardData = GetRandomReward();
            if (!ContainsOwnedEquipment(rewardData))
                return rewardData;
        }
    }

    private RewardData GetRandomReward()
    {
        DungeonLevel currentDungeonLevel = loadedDungeon.loadedDungeonData.dungeonLevels[loadedDungeon.currentLevel];
        List<RewardData> rewardList = currentDungeonLevel.rewards.rewards;
        List<float> probabiliyOfReward = currentDungeonLevel.rewards.probabilityOfReward;
        int numChoices = rewardList.Count;
        int i = 0;
        float lowerBound = 0;
        float upperBound = 0;
        System.Random rand = new System.Random();
        float randomFloat = (float)rand.NextDouble();
        while (i < numChoices)
        {
            lowerBound = upperBound;
            upperBound += probabiliyOfReward[i];
            if (lowerBound < randomFloat & upperBound > randomFloat)
                return rewardList[i];
            i++;
        }
        return rewardList[numChoices - 1];
    }

    private bool ContainsOwnedEquipment(RewardData rewardData)
    {
        foreach (OwnedEquipmentData equipment in rewardData.equipmentRewards)
        {
            EquipmentSet equipmentSet = equipment.equipmentSet;
            EquipmentSlot equipmentSlot = equipment.equipmentSlot;
            if (inventoryController.GetIndexOfEquipment(equipmentSet, equipmentSlot) != -1)
                return true;
        }
        return false;
    }

}
