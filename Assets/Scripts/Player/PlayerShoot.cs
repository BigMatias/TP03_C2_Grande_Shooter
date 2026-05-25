using System;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    
    [Header("Secondary Fire (Bomb)")]
    [SerializeField] private float m2Cooldown = 20f;
    [SerializeField] private float timeToHit = 1.5f;
    [SerializeField] private KeyCode toggleTrajectoryKey = KeyCode.T;

    [Header("Trajectory Cheat")]
    [SerializeField] private LineRenderer trajectoryLine; 
    [SerializeField] private int trajectoryResolution = 30;
    
    private float m2CooldownTimer = 0f;
    private bool showTrajectory = true;
    public event Action OnPlayerShootM1;
    public event Action OnPlayerShootM2;

    private void Update()
    {
        
        
    }

  /*  private void Shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = new Ray(activeCam.transform.position, activeCam.transform.forward);
            RaycastHit hit;
            Vector3 endPoint;

            laserLine.SetPosition(0, shootPoint.position);

            if (Physics.Raycast(ray, out hit, turretDataSO.M1ShootRange, hitLayers))
            {
                Debug.Log("Impacto en: " + hit.collider.name);
                endPoint = hit.point;

                IDamageable target = hit.collider.GetComponentInParent<IDamageable>();
                if (target != null)
                    target.TakeDamage(turretDataSO.M1Damage);
            }
            else
            {
                endPoint = ray.origin + ray.direction * turretDataSO.M1ShootRange;
            }

            laserLine.SetPosition(1, endPoint);
            onPlayerShootM1?.Invoke();
            StartCoroutine(ShootEffectSequence());
        }
    }

    private void HandleBombM2()
    {
        if (m2CooldownTimer > 0)
        {
            m2CooldownTimer -= Time.deltaTime;
        }

        if (Input.GetMouseButtonDown(1) && m2CooldownTimer <= 0)
        {
            m2CooldownTimer = m2Cooldown;

            BombProjectile bomb = PoolManager.Instance.Get<BombProjectile>();

            if (bomb != null)
            {
                bomb.transform.position = shootPoint.position;
                bomb.transform.rotation = shootPoint.rotation;

                Vector3 launchVelocity = CalculateLaunchVelocity(shootPoint.position, targetPoint, timeToHit);

                bomb.Launch(launchVelocity);

                onPlayerShootM2?.Invoke();
            }
        }
    }

    private void HandleTrajectoryCheat()
    {
        if (Input.GetKeyDown(toggleTrajectoryKey))
        {
            showTrajectory = !showTrajectory;
            trajectoryLine.enabled = showTrajectory;
        }

        if (!showTrajectory || m2CooldownTimer > 0)
        {
            if (trajectoryLine.enabled) trajectoryLine.enabled = false;
            return;
        }

        if (!trajectoryLine.enabled) trajectoryLine.enabled = true;
        DrawTrajectory();
    }


    private Vector3 CalculateLaunchVelocity(Vector3 origin, Vector3 target, float flightTime)
    {
        Vector3 distance = target - origin;
        Vector3 distanceXZ = distance;
        distanceXZ.y = 0;

        float sY = distance.y;
        float sXZ = distanceXZ.magnitude;
        float Vxz = sXZ / flightTime;

        float Vy = (sY / flightTime) + (0.5f * Mathf.Abs(Physics.gravity.y) * flightTime);

        Vector3 result = distanceXZ.normalized;
        result *= Vxz;
        result.y = Vy;

        return result;
    }

    private void DrawTrajectory()
    {
        Vector3 velocity = CalculateLaunchVelocity(shootPoint.position, targetPoint, timeToHit);
        trajectoryLine.positionCount = trajectoryResolution;

        float timeStep = timeToHit / (trajectoryResolution - 1);

        for (int i = 0; i < trajectoryResolution; i++)
        {
            float t = i * timeStep;

            Vector3 posXZ = shootPoint.position + new Vector3(velocity.x, 0, velocity.z) * t;
            float posY = shootPoint.position.y + (velocity.y * t) - (0.5f * Mathf.Abs(Physics.gravity.y) * t * t);

            Vector3 pointPosition = new Vector3(posXZ.x, posY, posXZ.z);
            trajectoryLine.SetPosition(i, pointPosition);
        }
    }
    private IEnumerator ShootEffectSequence()
    {
        laserLine.enabled = true;
        yield return new WaitForSeconds(turretDataSO.LaserDuration);
        laserLine.enabled = false;
    }*/
}
