using System;
using UnityEngine;

public class Bullet : MonoBehaviour, IPooleable
{
    [SerializeField] private EnemyDataSO enemyDataSO;

    private Vector3 startPos;
    private Vector3 targetPos;

    private float speed; 
    private float distance;
    private float traveled;
    private float bulletLifeTimeAux;

    private void Start()
    {
        bulletLifeTimeAux = enemyDataSO.gunBulletLifetime;
    }

    void Update()
    {
        float step = speed * Time.deltaTime;
        traveled += step;

        float t = traveled / distance;

        if (t >= 1f)
        {
            transform.position = targetPos;
            ReturnToPool();
            return;
        }

        transform.position = Vector3.Lerp(startPos, targetPos, t);

        bulletLifeTimeAux -= Time.deltaTime;

        if (bulletLifeTimeAux <= 0)
        {
            bulletLifeTimeAux = enemyDataSO.gunBulletLifetime;
            ReturnToPool();
        }
    }

    public void Init(Vector3 start, Vector3 target, float speed)
    {
        startPos = start;
        targetPos = target;
        this.speed = speed;

        transform.position = startPos;

        Vector3 dir = (targetPos - startPos).normalized;

        if (dir != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(dir);

        distance = Vector3.Distance(startPos, targetPos);
        traveled = 0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == (int)Layers.Player)
        {
            IDamageable targetHealth = other.gameObject.GetComponent<IDamageable>();
            targetHealth.TakeDamage(enemyDataSO.gunDamage);
        }
        ReturnToPool();
    }

    public bool IsActive =>  gameObject.activeSelf;
    
    public void Activate()
    {
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
    
    private void ReturnToPool()
    {
        if (PoolManager.Instance != null) PoolManager.Instance.Return(this);
        else Destroy(gameObject);
    }
}