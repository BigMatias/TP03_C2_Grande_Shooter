using UnityEngine;

public class MeleeHitbox : MonoBehaviour
{
    private float _damage;
    private Collider _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _collider.enabled = false;
    }

    public void Activate(float damage)
    {
        _damage = damage;
        _collider.enabled = true;
    }

    public void Deactivate()
    {
        _collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(_damage);
            Deactivate();
        }
    }
}
