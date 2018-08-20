using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAbility : ScriptableObject {

    public float rechargeTime;

    public virtual void Initialize() { }
    public virtual void TriggerAbility() { }

}
