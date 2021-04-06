using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swarm : Enemy 
{
    float damageAccum;
    [SerializeField] float damageFalloff;
    [SerializeField] float glowScaler = 1;
    [SerializeField] float deathThreshold;
    [SerializeField] float baseSpeed;
    [SerializeField] GameObject DeathParticle;

    public override bool DealDamage(float amount)
    {
        damageAccum += amount;
        if(damageAccum >= deathThreshold)
        {
            Die();
        }
        return base.DealDamage(amount);
    }
    protected override void Die()
    {
        base.Die();
        Destroy(gameObject);
        Instantiate(DeathParticle, transform.GetChild(0).transform.position, Quaternion.identity)
            .GetComponent<ParticleSystemRenderer>().material.SetFloat("_glow", 1 + (damageAccum * glowScaler));
    }

    [SerializeField]
    private ParticleSystemRenderer rend;

    private MaterialPropertyBlock propertyBlock;

    // Start is called before the first frame update
    void Update()
    {
        damageAccum = Mathf.Max(damageAccum - damageFalloff * Time.deltaTime, 0);

        rend.material.SetFloat("_glow", 1 + (damageAccum * glowScaler));

        transform.position = Vector3.MoveTowards(transform.position, player.position, baseSpeed * Time.deltaTime);
    }
}
