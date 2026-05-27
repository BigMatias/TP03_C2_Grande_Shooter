using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    private HealthSystem healthSystem;

    public event Action onDie;
    public event Action<float, float> onDamage;
    
    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        healthSystem.onDie += OnDie;
    }

    private void OnDestroy()
    {
        healthSystem.onDie -= OnDie;
    }
    
    private void OnDie()
    {
        onDie?.Invoke();
        gameObject.SetActive(false);
        ScoreManager.Instance.AddScore(10);
        DeathParticles particles = PoolManager.Instance.Get<DeathParticles>();
        if (particles != null)
            particles.transform.position = transform.position;
    }
    
    
    public void TakeDamage(float amount)
    {
        healthSystem.TakeDamage(amount);    
    }
}
