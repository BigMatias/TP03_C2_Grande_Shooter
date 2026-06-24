using System;
using UnityEngine;

public class Granade : MonoBehaviour, IPooleable
{
    [Header("Explosion")]
    [SerializeField] private EnemyDataSO _enemyDataSo;
    [SerializeField] private LayerMask hitLayers;
    
    private Rigidbody rb;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private Vector3 CalculateLaunchVelocity(Vector3 origin, Vector3 target, float flightTime)
    {
        Vector3 distance = target - origin;
        Vector3 distanceXZ = new Vector3(distance.x, 0, distance.z);

        float Vxz = distanceXZ.magnitude / flightTime;
        float Vy = (distance.y / flightTime) + (0.5f * Mathf.Abs(Physics.gravity.y) * flightTime);

        return distanceXZ.normalized * Vxz + Vector3.up * Vy;
    }

    public void Init(Vector3 startPosition, Vector3 target, float flightTime)
    {
        rb.linearVelocity = CalculateLaunchVelocity(startPosition, target, flightTime); 
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _enemyDataSo.granadeRadius, hitLayers);

        foreach (Collider collider in colliders)
        {
            IDamageable target = collider.GetComponentInParent<IDamageable>();
            if (target != null && collider.gameObject.layer == (int)Layers.Player)
            {
                target.TakeDamage(_enemyDataSo.granadeDamage);
            }
        }

        ReturnToPool();
    }
    
    private void ReturnToPool()
    {
        if (PoolManager.Instance != null) PoolManager.Instance.Return(this);
        else Destroy(gameObject);
    }
    
    public bool IsActive => gameObject.activeSelf;
    
    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
