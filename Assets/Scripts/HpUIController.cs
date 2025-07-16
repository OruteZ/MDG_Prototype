using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HpUIController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Slider hpSlider;    // HP 슬라이더
    [SerializeField] private Text hpText;        // HP 텍스트

    [Header("플레이어")]
    [SerializeField] private PlayerController player; // PlayerController 참조

    void Start()
    {
        if (player == null)
            player = FindObjectOfType<PlayerController>();

        // 슬라이더 최대값 설정 및 초기 업데이트
        hpSlider.maxValue = player.maxHp;
        UpdateUI();

        // 체력 변경 이벤트 리스너 등록
        player.onHpChange.AddListener(UpdateUI);
        UpdateUI();
    }

    void OnDestroy()
    {
        player.onHpChange.RemoveListener(UpdateUI);
    }

    // UI 갱신
    private void UpdateUI()
    {
        hpSlider.value = player.CurrentHp;
        if (hpText != null)
            hpText.text = $"{player.CurrentHp}/{player.maxHp}";
    }
}