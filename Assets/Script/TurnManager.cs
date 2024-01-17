using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public enum Turn
    {
        Player,
        CPU,
        ActionPhase // 동작 수행 페이즈 추가
    }

    public Turn currentTurn = Turn.Player;
    public TextMeshProUGUI turnIndicator;
    public delegate void OnTurnChangeDelegate(Turn newTurn);
    public event OnTurnChangeDelegate onTurnChange;


    void Start()
    {
        SetTurn(Turn.Player);
    }

    void UpdateTurnIndicator()
    {
        switch (currentTurn)
        {
            case Turn.Player:
                turnIndicator.text = "플레이어 턴";
                break;
            case Turn.CPU:
                turnIndicator.text = "컴퓨터 턴";
                break;
            case Turn.ActionPhase:
                turnIndicator.text = "동작 수행 중";
                break;
        }
    }

    public void ChangeTurn()
    {
        // 턴 순서에 따라 다음 턴 설정
        switch (currentTurn)
        {
            case Turn.Player:
                SetTurn(Turn.CPU);
                break;
            case Turn.CPU:
                SetTurn(Turn.ActionPhase);
                break;
            case Turn.ActionPhase:
                SetTurn(Turn.Player); // 동작 수행 후 플레이어 턴으로 돌아감
                break;
        }
    }

    public void SetTurn(Turn turn)
    {
        currentTurn = turn;
        UpdateTurnIndicator();
        onTurnChange?.Invoke(turn);

        if (currentTurn == Turn.CPU)
        {
            StartCoroutine(CPUTurn());
        }
        else if (currentTurn == Turn.ActionPhase)
        {
            StartCoroutine(ExecuteActions());
        }
    }

    IEnumerator CPUTurn()
    {
        yield return new WaitForSeconds(1);
        PerformCPUTurn();
        ChangeTurn(); // CPU 턴 종료 후 동작 수행 페이즈로 변경
    }

    private void PerformCPUTurn()
    {
        // CPU 행동 결정 및 수행
    }

    IEnumerator ExecuteActions()
    {
        // 동작 수행 페이즈 로직
        // 예: 플레이어와 CPU의 선택에 따른 결과 처리

        yield return new WaitForSeconds(1); // 동작 수행에 시간 지연

        ChangeTurn(); // 동작 수행 페이즈 종료 후 플레이어 턴으로 변경
    }
}
