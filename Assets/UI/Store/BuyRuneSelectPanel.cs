using System.Collections;
using System.Collections.Generic;
using Assets.Store;
using UnityEngine;

public class BuyRuneSelectPanel : SelectPanel
{
    [SerializeField] private StoreStock storeStock;
    protected override void GetInventory()
    {
        itemList = storeStock.GetRuneStoreStock();
    }
}
