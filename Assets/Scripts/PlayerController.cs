// PlayerController.cs
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 5f;             // 좌우 이동 속도
    public float jumpForce = 7f;             // 점프 힘

    [Header("투사체 설정")]
    public GameObject projectilePrefab;      // 발사할 투사체 프리팹
    public Transform projectileSpawnPoint;   // 투사체가 생성될 위치
    public float projectileSpeed = 10f;      // 투사체 발사 속도

    [Header("HP 설정")]
    public int maxHp = 100;                  // 최대 체력
    private int currentHp;
    public UnityEvent onHpChange;            // 체력 변경 이벤트

    public int CurrentHp => currentHp;

    private Rigidbody2D rb;
    private bool isGrounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHp = maxHp;                   // 체력 초기화
    }

    void Update()
    {
        HandleMovement();
        HandleJump();
        HandleAttack();
    }

    // 좌우 이동
    void HandleMovement()
    {
        float h = Input.GetAxis("Horizontal");
        Vector2 vel = rb.linearVelocity;
        vel.x = h * moveSpeed;
        rb.linearVelocity = vel;
    }

    // 점프
    void HandleJump()
    {
        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    // 공격 (투사체 발사) → 클릭한 지점을 향해 발사하도록 변경됨
    void HandleAttack()
    {
        if (Input.GetButtonDown("Fire1") && projectilePrefab != null)
        {
            // 1) 투사체 생성
            Vector3 spawnPos = projectileSpawnPoint.position;
            GameObject proj = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

            // 2) 마우스 위치를 월드 좌표로 변환
            Vector3 mouseScreen = Input.mousePosition;
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(mouseScreen);
            // Z축은 2D 플레이어와 동일하게 고정
            mouseWorld.z = spawnPos.z;

            // 3) 방향 벡터 계산 (정규화)
            Vector2 direction = ((Vector2)mouseWorld - (Vector2)spawnPos).normalized;

            // 4) Rigidbody2D에 속도 적용
            Rigidbody2D projRb2D = proj.GetComponent<Rigidbody2D>();
            if (projRb2D != null)
            {
                projRb2D.linearVelocity = direction * projectileSpeed;
            }
        }
    }
    
    public void TakeDamage(int amount)
    {
        currentHp -= amount;
        if (currentHp <= 0)
        {
            Die();
        }
        onHpChange?.Invoke(); // 체력 변경 이벤트 호출
    }

    // 사망 처리
    private void Die()
    {
        Destroy(gameObject);
    }

    // 바닥 충돌 체크
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
            isGrounded = true;
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground"))
            isGrounded = false;
    }
}
