using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Store
{
    public class StoreStockLoader : MonoBehaviour
    {
        [SerializeField] private StoreStock storeStock;
        [SerializeField] private ProgressTracker progressTracker;
        [SerializeField] private DevProgress devProgress;
        private static bool hasLoadedProgress;
        private void Awake()
        {
            if (progressTracker.loadDevProgress & !hasLoadedProgress & Application.isEditor)
            {
                storeStock.CopyBase();
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
                    storeStock.CopyBase();
                    hasLoadedProgress = true;
                }
            }
        }
    }
}
