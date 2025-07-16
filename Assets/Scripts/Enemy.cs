using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class Enemy : MonoBehaviour
{
    [Header("배회 설정")]
    [SerializeField] private float moveSpeed = 3f;       // 이동 속도
    [SerializeField] private float wanderRadius = 5f;    // 배회 반경
    [SerializeField] private float wanderInterval = 2f;  // 목표 재설정 간격

    [Header("전투 설정")]
    [SerializeField] private int maxHealth = 50;         // 최대 체력
    [SerializeField] private int damageToPlayer = 10;    // 플레이어 접촉 데미지
    [SerializeField] private int damageFromProjectile = 20; // 투사체 데미지

    [Header("이벤트")]
    public UnityEvent onDeath;       // 사망 시

    [SerializeField] private int currentHealth;
    private Rigidbody2D rb;
    private float wanderTimer;
    private Vector3 wanderTarget;
    private Vector3 homePosition;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        homePosition = transform.position;
        currentHealth = maxHealth;  
        wanderTimer = wanderInterval; 
    }

    void Update()
    {
        Wander();
    }

    private void Wander()
    {
        wanderTimer += Time.deltaTime;
        if (wanderTimer >= wanderInterval ||
            Vector3.Distance(transform.position, wanderTarget) < 0.5f)
        {
            wanderTimer = 0f;
            Vector3 offset = Random.insideUnitSphere * wanderRadius;
            offset.y = 0f;
            wanderTarget = homePosition + offset;
        }

        Vector3 dir = (wanderTarget - transform.position).normalized;
        rb.linearVelocity = new Vector3(dir.x * moveSpeed, rb.linearVelocity.y, dir.z * moveSpeed);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        onDeath?.Invoke();
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            var player = col.gameObject.GetComponent<PlayerController>();
            if (player != null) player.TakeDamage(damageToPlayer);
        }
    }
}
