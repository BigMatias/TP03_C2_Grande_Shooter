using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FsmManager : MonoBehaviour, IPooleable, IDamageable
{
    [Header(("References"))]
    [SerializeField] private EnemyDataSO enemyDataSO;
    [SerializeField] private GameObject particleSpawnPoint;
    [SerializeField] private MeleeHitbox meleeHitbox;
    [SerializeField] private NpcType npcType;
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerChannelSO playerChannel;
    [SerializeField] private NavMeshAgent agent;
    [Header("Weapon references")]
    [SerializeField] private GameObject gun;
    [SerializeField] private Transform gunShootPoint;
    [SerializeField] private GameObject granade;
    [SerializeField] private Transform granadeShootPoint;
    [SerializeField] private LineRenderer lineRenderer;
    public EnemyDataSO EnemyData => enemyDataSO;
    public Transform gunShootPointTransform => gunShootPoint;
    public Transform playerTargetTransform => playerTarget.transform;

    public event Action onDie;
    public event Action<float, float> onDamage;
    
    public event Action onEnemyPunch;
    public event Action onEnemyShoot;
    public event Action onEnemyThrewGranade;

    private float shootCdAux;
    private float throwGranadeCdAux;
    private bool isDead = false;
    
    private Coroutine enemyDiedCoroutine;
    private Coroutine meleeAttackCoroutine;
    
    private Transform playerTarget;
    private List<StateBase> states = new List<StateBase>();
    private StateBase currentState;
    private HealthSystem healthSystem;
    private Collider collider;
    private Rigidbody rb;
    
    private void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        collider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
        healthSystem.onDie += HealthSystem_onDie;
        healthSystem.onDamage += HealthSystem_OnDamage;
        
        agent.enabled = false;
        agent.updateRotation = false;
        
        states.Add(new StateIdle());
        states.Add(new StateWalk());
        states.Add(new StateDie());
        states.Add(new StateShoot());           
        states.Add(new StateMelee());   
        states.Add(new StateThrowGranade());   
        foreach (StateBase state in states)
            state.Initialize(animator, this, agent);

        currentState = FindState(StateType.Idle);
    }

    private void Update()
    {
        CheckPlayerPosition();
        HandleRotation();
        if (currentState!=null)
            currentState.OnUpdate();
    }

    private void OnDestroy()
    {
        healthSystem.onDie -= HealthSystem_onDie;
        healthSystem.onDamage -= HealthSystem_OnDamage;
        
        StopAllCoroutines();  
    }
    
    private void HealthSystem_OnDamage(float current, float total)
    {
        onDamage?.Invoke(current, total);
    }
    
    private void HealthSystem_onDie()
    {
        onDie?.Invoke();
        SwitchState(StateType.Die);
        isDead = true;
        collider.enabled = false;
        switch (npcType)
        {
            case NpcType.ZombieMelee:
                ScoreManager.Instance.AddScore(enemyDataSO.pointsGivenMelee);
                break;
            case NpcType.ZombieRanged:
                ScoreManager.Instance.AddScore(enemyDataSO.pointsGivenRanged);
                break;
            case NpcType.ZombieGranadier:
                ScoreManager.Instance.AddScore(enemyDataSO.pointsGivenGranadier);
                break;
        }
        if (enemyDiedCoroutine  == null)
            enemyDiedCoroutine = StartCoroutine(WaitAndDeactivate());
    }

    private void HandleRotation()
    {
        if (isDead) return;
        if (playerTarget == null) return;

        Vector3 dir = playerTarget.position - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.LookRotation(dir);
    }
    
    public void SwitchState(StateType targetState)
    {
        SwitchState(FindState(targetState));
    }

    public void SwitchState(StateBase targetState)
    {
        if (currentState == targetState)
        {
            return;
        }

        currentState.OnExit();
        currentState = targetState;
        currentState.OnEnter();
    }

    public StateBase FindState(StateType stateToFind)
    {
        foreach (StateBase state in states)
            if (state.stateType == stateToFind)
                return state;

        return null;
    }

    public void CheckPlayerPosition()
    {
        float distance = Vector3.Distance(playerTarget.transform.position, transform.position);

        if (isDead) return;
        
        if (npcType == NpcType.ZombieRanged && distance < enemyDataSO.gunRange)
            SwitchState(StateType.Shoot);
        else if (npcType == NpcType.ZombieMelee && distance < enemyDataSO.meleeRange)
            SwitchState(StateType.Punch);
        else if  (npcType == NpcType.ZombieGranadier && distance < enemyDataSO.granadeRange)
            SwitchState(StateType.ThrowGranade);
        else
        {
            if (npcType == NpcType.ZombieRanged) 
                gun.gameObject.SetActive(false);
            SwitchState(StateType.Walking);
        }
    }

    public void Shoot()
    {
        shootCdAux -= Time.deltaTime;
        gun.gameObject.SetActive(true);
        if (shootCdAux <= 0)
        {
            onEnemyShoot?.Invoke();
               
            Bullet bullet = PoolManager.Instance.Get<Bullet>();

            bullet.transform.position = gunShootPoint.position;
            bullet.transform.rotation = Quaternion.identity;
            
            bullet.Init(gunShootPoint.position, playerTarget.transform.position + new Vector3(0, 1, 0), enemyDataSO.gunBulletSpeed);
            shootCdAux = enemyDataSO.gunShootCooldown;
            rb.linearVelocity = Vector3.zero;
        }
    }
    
    public void Punch()
    {
        onEnemyPunch?.Invoke();
        if (meleeAttackCoroutine == null)
            meleeAttackCoroutine = StartCoroutine(MeleeAttack());
    }
    
    public void ThrowGranade()
    {
        throwGranadeCdAux -= Time.deltaTime;
        
        if (throwGranadeCdAux <= 0)
        {
            onEnemyThrewGranade?.Invoke();
            
            Granade granade = PoolManager.Instance.Get<Granade>();

            granade.transform.position = granadeShootPoint.position;
            granade.transform.rotation = Quaternion.identity;
            
            granade.Init(granadeShootPoint.position, playerTarget.transform.position, enemyDataSO.granadeFlightTime);
            throwGranadeCdAux = enemyDataSO.granadeCooldown;
            rb.linearVelocity = Vector3.zero;
        }
    }

    public void TakeDamage(float amount)
    {
        healthSystem.TakeDamage(amount);
        DeathParticles particles = PoolManager.Instance.Get<DeathParticles>();
        if (particles != null)
            particles.transform.position = particleSpawnPoint.transform.position;
    }
    
    private IEnumerator WaitAndDeactivate()
    {
        yield return new WaitForSeconds(2f);
        ReturnToPool();
    }
    
    private IEnumerator MeleeAttack()
    {
        yield return new WaitForSeconds(enemyDataSO.meleeActivateHitboxTime);
        meleeHitbox.Activate(enemyDataSO.meleeDamage);
        yield return new WaitForSeconds(0.2f); 
        meleeHitbox.Deactivate();
        meleeAttackCoroutine = null;
    }
    
    private void ReturnToPool()
    {
        if (PoolManager.Instance != null) PoolManager.Instance.Return(this);
        else Destroy(gameObject);
    }
    
    public bool IsActive => gameObject.activeSelf;
    
    public void Activate()
    {
        playerTarget = playerChannel.PlayerTransform;
        agent.enabled = true;
        agent.Warp(transform.position);
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 5f, NavMesh.AllAreas))
            agent.Warp(hit.position);
        gameObject.SetActive(true);
    }
    
    public void Deactivate()
    {
        AudioManager.Instance?.UnregisterEnemy(this);
        agent.enabled = false;
        gameObject.SetActive(false);
    }
}
