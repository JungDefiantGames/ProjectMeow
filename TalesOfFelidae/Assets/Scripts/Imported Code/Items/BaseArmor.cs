using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseArmor : BaseItem {

    public float defenseValue;

    public virtual void OnEquip() { }
    public virtual void OnUnequip() { }

}
