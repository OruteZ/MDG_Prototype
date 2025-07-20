using System;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("레퍼런스")]
    [SerializeField] private PlayerController player; // 플레이어
    [SerializeField] private Goal goal; // 골 지점   
    
    [SerializeField] private Canvas clearCanvas; // 클리어 UI 캔버스
    [SerializeField] private Canvas failCanvas; // 실패 UI 캔버스

    private bool isGameOver;

    void Awake()
    {
        // 싱글톤
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        // 초기화
        isGameOver = false;
        clearCanvas.gameObject.SetActive(false); // 클리어 UI 비활성화
        failCanvas.gameObject.SetActive(false); // 실패 UI 비활성화

        // 골 지점 이벤트 리스너 등록
        if (goal != null)
            goal.onClear.AddListener(GameClear);
    }

    public void GameClear()
    {
        if (isGameOver) return; // 이미 게임이 끝났다면 무시

        isGameOver = true;
        clearCanvas.gameObject.SetActive(true); // 클리어 UI 활성화
        Time.timeScale = 0f; // 게임 일시 정지

        // 추가 클리어 로직이 있다면 여기에 작성
        Debug.Log("게임 클리어!");
    }

    public void GameFail()
    {
        if (isGameOver) return; // 이미 게임이 끝났다면 무시

        isGameOver = true;
        failCanvas.gameObject.SetActive(true); // 실패 UI 활성화
        Time.timeScale = 0f; // 게임 일시 정지

        // 추가 실패 로직이 있다면 여기에 작성
        Debug.Log("게임 실패!");
    }

    public void BackToTitle()
    {
        // 타이틀 화면으로 돌아가기
        UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
    }
}