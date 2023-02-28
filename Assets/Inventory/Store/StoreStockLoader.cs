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
        private static bool hasLoadedDevProgress;
        private void Awake()
        {
            if (progressTracker.loadDevProgress & !hasLoadedDevProgress & Application.isEditor)
            {
                storeStock.CopyBase();
                hasLoadedDevProgress = true;
            }
        }
    }
}
