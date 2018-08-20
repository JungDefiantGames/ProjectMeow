using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitController : MonoBehaviour {

    [HideInInspector] public DamageHandler damageHandler;
    [HideInInspector] public InventoryHandler inventoryHandler;
}
