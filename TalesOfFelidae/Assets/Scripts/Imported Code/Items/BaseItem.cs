using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseItem : ScriptableObject {

    public string itemName;
    public int itemID;
    public ItemType itemType;

	// Use this for initialization
	//public virtual void OnUse (PlayerPawn pc) {
    //}
	
}

public enum ItemType { Consumable, Furniture, Wallpaper, Outfit, Weapon, Tool, Sidearm, Trinket }
