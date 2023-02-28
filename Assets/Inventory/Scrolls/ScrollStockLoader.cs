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
    private static bool hasLoadedDevProgress;
    private void Awake()
    {
        if (progressTracker.loadDevProgress & !hasLoadedDevProgress & Application.isEditor)
        {
            scrollStock.CopyBase();
            hasLoadedDevProgress = true;
        }
    }
}
