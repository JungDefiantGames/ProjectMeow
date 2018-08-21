using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryHandler : MonoBehaviour {

    BaseItem[] EquipSlots = new BaseItem[6];        //0 is Primary, 1 is Secondary, 2 is Outfit, 3/4/5 is Trinket
    BaseItem[] BackpackSlots = new BaseItem[12];    //Use Array.Resize() to change its size with storage property; drops any excess items

    public void AddToInventory(BaseItem newItem)
    {

    }

    public void DropFromInventory(int itemSlotID)
    {
    }

    public void EquipToSlot(int packSlotID)
    {
        BaseItem itemToSwap;

        switch (BackpackSlots[packSlotID].itemType)
        {
            case ItemType.Consumable:
                itemToSwap = EquipSlots[1];
                EquipSlots[1] = BackpackSlots[packSlotID];
                BackpackSlots[packSlotID] = itemToSwap;
                break;
            case ItemType.Outfit:
                itemToSwap = EquipSlots[2];
                EquipSlots[2] = BackpackSlots[packSlotID];
                BackpackSlots[packSlotID] = itemToSwap;
                break;
            case ItemType.Sidearm:
                itemToSwap = EquipSlots[1];
                EquipSlots[1] = BackpackSlots[packSlotID];
                BackpackSlots[packSlotID] = itemToSwap;
                break;
            case ItemType.Tool:
                itemToSwap = EquipSlots[0];
                EquipSlots[0] = BackpackSlots[packSlotID];
                BackpackSlots[packSlotID] = itemToSwap;
                break;
            default: break;
        }
    }

    public void UnequipFromSlot(int equipSlotID)
    {
        for (int i = 0; i < BackpackSlots.Length; i++)
        {
            if (BackpackSlots[i] == null)
            {
                BackpackSlots[i] = EquipSlots[i];
                EquipSlots[i] = null;
                break;
            }
        }
    }

    public void DestroyItem(BaseItem itemToBeDestroyed)
    {
    }

}
