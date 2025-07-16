using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class Goal : MonoBehaviour
{
    public UnityEvent onClear; // 클리어 시

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            Clear();
    }

    private void Clear()
    {
        onClear?.Invoke();
        // 추가 클리어 로직 있으면 여기에
    }
}