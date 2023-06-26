using System.Collections;
using System.Collections.Generic;
using Assets.Inventory.Scrolls;
using Assets.Progression;
using Assets.Store;
using UnityEngine;

public class ScrollStockLoader : MonoBehaviour
{
    [SerializeField] private ProgressTracker progressTracker;
    [SerializeField] private ScrollStock scrollStock;
    [SerializeField] private DevProgress devProgress;
    [SerializeField] private RewardDatabase rewardDatabase;
    private static bool hasLoadedProgress;
    private void Awake()
    {
        if (progressTracker.loadDevProgress & !hasLoadedProgress & Application.isEditor)
        {
            scrollStock.CopyBase();
            hasLoadedProgress = true;
        }
        else if (!hasLoadedProgress)
        {
            if (SaveManager.HasSaveData())
            {
                // Load Save
            }
            else
            {
                scrollStock.CopyBase();
                hasLoadedProgress = true;
            }
        }
    }
}
