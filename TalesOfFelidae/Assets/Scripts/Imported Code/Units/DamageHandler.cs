using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHandler : MonoBehaviour {

    [HideInInspector] public float currentHitPoints;
    public float maximumHitPoints;
    [HideInInspector] public float currentShieldPoints;
    public float maximumShieldPoints;
    public bool isPlayer;

    public UnityEngine.UI.Image healthBar;
    public UnityEngine.UI.Image shieldBar;

    void Awake()
    {
        if(isPlayer)
        {
            maximumHitPoints = 100f * (1 + (GameStateManager.GetInstance().currentGameData.pcBody * 0.025f));
            maximumShieldPoints = 50f * (1 + (GameStateManager.GetInstance().currentGameData.pcTech * 0.04f));
        }
        
        currentHitPoints = maximumHitPoints;
        currentShieldPoints = maximumShieldPoints;
    }

    public void RegenerateHealth()
    {
        HealDamage(5 * Time.deltaTime);
    }

    public void TakeDamage(float effectValue)
    {
        if (effectValue < 0) effectValue = 0;
        if (currentShieldPoints > 0)
        { 
            currentShieldPoints -= effectValue;
        }
        else if (currentShieldPoints <= 0)
        {
            float leftoverDamage = -(currentShieldPoints - effectValue);
            currentShieldPoints = 0;
            currentHitPoints -= leftoverDamage;
        }

        if (currentHitPoints <= 0)
        {
            currentHitPoints = 0;
            ExecuteDeath();
        }
    }

    public void HealDamage(float effectValue)
    {
        if (effectValue < 0) effectValue = 0;
        currentHitPoints += effectValue;

        if (currentHitPoints > maximumHitPoints)
        {
            currentHitPoints = maximumHitPoints;
        }
    }

    public void HealShield(float effectValue)
    {
        if (effectValue < 0) effectValue = 0;
        currentShieldPoints += effectValue;

        if (currentShieldPoints > maximumShieldPoints)
        {
            currentShieldPoints = maximumShieldPoints;
        }
    }

    public void ExecuteDeath()
    {

    }

}
