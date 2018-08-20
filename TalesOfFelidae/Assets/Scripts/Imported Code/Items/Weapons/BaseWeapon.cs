using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
[CreateAssetMenu(menuName = "Weapons/BaseWeapon")]
public class BaseWeapon : BaseItem {

    public float attackValue;
    public float attackRate;
    public float attackRange;
    public GameObject weaponPrefab;
    public int handsRequired;

    public override void OnUse(PlayerPawn pc) {

        InventoryHandler pcInv = pc.owner.inventoryHandler;

        if(pcInv.equippedWeapon != null)
        {
            for(int i = 0; i < pc.weaponBone.childCount; i++)
            {
                if(pc.weaponBone.GetChild(i).GetComponent<WeaponParticle>() != null) Destroy(pc.weaponBone.GetChild(i).gameObject);
            }
        }

        pcInv.equippedWeapon = this;
        Instantiate(weaponPrefab, pc.weaponBone);
        pcInv.weaponParticle = pcInv.equippedWeapon.weaponPrefab.GetComponent<WeaponParticle>();
    }

}*/
