using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHandler : MonoBehaviour {

    public List<BaseAbility> abilityList;
    public List<float> abilityTimerList;

    public void InitializePowerList()
    {
        abilityTimerList = new List<float>();

        for (int i = 0; i < abilityList.Count; i++)
        {
            if (abilityList[i] != null)
            {
                abilityList[i].Initialize();
                abilityList.Insert(i, abilityList[i]);
                abilityTimerList.Insert(i, abilityList[i].rechargeTime);
            }
        }
    }

    public void UpdateAbilityTimers()
    {
        for(int i = 0; i < abilityTimerList.Count; i++)
        {
            abilityTimerList[i] -= Time.deltaTime;
        }
    }
}
