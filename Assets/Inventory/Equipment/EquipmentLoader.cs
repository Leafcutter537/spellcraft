using System.Collections.Generic;
using Assets.Equipment;
using UnityEngine;

public class EquipmentLoader : MonoBehaviour
{
    [SerializeField] private InventoryController inventoryController;
    [SerializeField] private DevEquipment devEquipment;
    [SerializeField] private EquipmentStatDatabase equipmentStatDatabase;
    private static bool hasLoadedDevInventory;
    private void Awake()
    {
        if (inventoryController.loadDevEquipmentInventory == true & !hasLoadedDevInventory & Application.isEditor)
        {
            inventoryController.ownedEquipment = CreateEquipment(devEquipment.ownedEquipmentData);
            inventoryController.equippedEquipmentIndices = new List<int>();
            foreach (OwnedEquipmentData equipped in devEquipment.equippedEquipment)
            {
                int equipmentIndex = inventoryController.GetIndexOfEquipment(equipped.equipmentSet, equipped.equipmentSlot);
                if (equipmentIndex != -1)
                    inventoryController.equippedEquipmentIndices.Add(equipmentIndex);
            }
            hasLoadedDevInventory = true;
        }
    }

    private List<EquipmentPiece> CreateEquipment(List<OwnedEquipmentData> dataList)
    {
        List<EquipmentPiece> returnList = new List<EquipmentPiece>();
        foreach (OwnedEquipmentData data in dataList)
        {
            returnList.Add(new EquipmentPiece(data, equipmentStatDatabase));
        }
        return returnList;
    }
}
