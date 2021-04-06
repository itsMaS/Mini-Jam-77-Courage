using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour, IDamagable
{
    protected Transform player { get => PlayerController.Instance.transform; }

    public UnityEvent onDeath;
    public virtual bool DealDamage(float amount)
    {
        return true;
    }
    protected virtual void Die()
    {
        onDeath.Invoke();
    }
}
