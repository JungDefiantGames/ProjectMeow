using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class WeaponParticle : MonoBehaviour {

    public BaseWeapon weaponRef;
    private ParticleSystem bulletParticle;
    public ParticleSystem hitParticle;

    List<ParticleCollisionEvent> particleCollisionEvents;

    private void Start()
    {
        particleCollisionEvents = new List<ParticleCollisionEvent>();
        bulletParticle = GetComponent<ParticleSystem>();
        hitParticle = Instantiate(hitParticle, transform);
    }

    public void EmitBulletParticle()
    {
        Debug.Log("PEW");
        bulletParticle.Clear();
        bulletParticle.Emit(1);
    }

    public void OnParticleCollision(GameObject other)
    {
        int numCollisionEvents = bulletParticle.GetCollisionEvents(other, particleCollisionEvents);

        for(int i = 0; i < numCollisionEvents; i++)
        {
            if (particleCollisionEvents[i].colliderComponent.tag != tag)
            {
                Debug.Log("Boop");
                if (particleCollisionEvents[i].colliderComponent.GetComponent<PlayerPawn>() != null) {
                    particleCollisionEvents[i].colliderComponent.GetComponent<PlayerPawn>().TakeDamage(weaponRef.attackValue);
                }

                if (particleCollisionEvents[i].colliderComponent.GetComponent<BaseEnemy>() != null)
                {
                    particleCollisionEvents[i].colliderComponent.GetComponent<BaseEnemy>().TakeDamage(weaponRef.attackValue);
                }
            }

            hitParticle.Clear();
            hitParticle.transform.position = particleCollisionEvents[i].intersection;
            hitParticle.Emit(1);
        }
    }
}*/
