using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("투사체 설정")]
    [SerializeField] private int damage = 20;     // 데미지
    [SerializeField] private float lifeTime = 5f; // 수명

    void Start()
    {
        // 일정 시간 후 자동 삭제
        Destroy(gameObject, lifeTime);
    }

    // 충돌 시 처리
    void OnTriggerEnter2D(Collider2D other)
    {
        // 충돌 대상이 Enemy 태그라면 데미지 적용
        if (other.gameObject.CompareTag("Enemy"))
        {
            var enemy = other.gameObject.GetComponent<Enemy>();
            if (enemy != null)
                enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}