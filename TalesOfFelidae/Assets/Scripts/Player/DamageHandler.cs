using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHandler : MonoBehaviour {

    [HideInInspector] public float currHearts;
    public float maxHearts;
    public bool isPlayer;

    public UnityEngine.UI.Image healthBar;

    void Awake()
    {
        if(isPlayer)
        {
            maxHearts = 20f;
        }
        
        currHearts = maxHearts;
    }

    public void RegenerateHealth()
    {
        HealDamage(5 * Time.deltaTime);
    }

    public void TakeDamage(float effectValue)
    {
        if (effectValue < 0) effectValue = 0;

        if (currHearts <= 0)
        {
            currHearts = 0;
            ExecuteDeath();
        }
    }

    public void HealDamage(float effectValue)
    {
        if (effectValue < 0) effectValue = 0;
        currHearts += effectValue;

        if (currHearts > maxHearts)
        {
            currHearts = maxHearts;
        }
    }

    public void ExecuteDeath()
    {

    }

}
