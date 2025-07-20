using UnityEngine;

public class Rotation : MonoBehaviour
{
    public float rotationSpeed = 100f; // 회전 속도
    
    private void Update()
    {
        // y축으로 등속도 회전
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}
