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
    public GameActions gameActions; // Inspector에서 할당
    public Card.CardType playerCardType = Card.CardType.Block; // 초기값 설정
    private Card.CardType lastCpuCardType = Card.CardType.Block; // 초기값 설정
    public Card.CardType lastPlayerCardType = Card.CardType.None;
    private bool isFirstPlayerTurn = true; // 선후공 추적을 위한 변수
    private bool isGameOver = false;

    void Start()
    {
        SetTurn(Turn.Player);
    }

    public void EndGame()
    {
        isGameOver = true;
        // 필요한 경우 추가적인 게임 종료 로직 구현
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    void UpdateTurnIndicator()
    {
        switch (currentTurn)
        {
            case Turn.Player:
                turnIndicator.text = "플레이어 턴";
                Debug.Log("플레이어의 턴입니다");
                break;
            case Turn.CPU:
                turnIndicator.text = "컴퓨터 턴";
                Debug.Log("컴퓨터의 턴입니다");
                break;
            case Turn.ActionPhase:
                turnIndicator.text = "동작 수행 중";
                Debug.Log("동작 수행 페이즈 입니다");
                break;
        }
    }

    public void ChangeTurn()
    {
        if (isGameOver) return;
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
                isFirstPlayerTurn = !isFirstPlayerTurn; // 선후공 교대
                SetTurn(Turn.Player); // 동작 수행 후 플레이어 턴으로 돌아감
                break;
        }
    }


    public void SetTurn(Turn turn)
    {
        currentTurn = turn;
        UpdateTurnIndicator();
        onTurnChange?.Invoke(turn);

        if (currentTurn == Turn.Player)
        {
            StartCoroutine(PlayerTurn());
        }
        else if (currentTurn == Turn.CPU)
        {
            StartCoroutine(CPUTurn());
        }
        else if (currentTurn == Turn.ActionPhase)
        {
            StartCoroutine(ExecuteActions());
        }
    }

    IEnumerator PlayerTurn()
    {
        //playerCardType = Card.CardType.None; // 카드 선택 초기화
        yield return new WaitUntil(() => playerCardType != Card.CardType.None); // 플레이어가 카드를 선택할 때까지 대기

        // 플레이어가 카드를 선택하면 턴 전환 로직 실행
        // 예: ChangeTurn(); 또는 플레이어의 행동 실행
    }


    IEnumerator CPUTurn()
    {
        yield return new WaitForSeconds(1);
        PerformCPUTurn();
        ChangeTurn(); // CPU 턴 종료 후 동작 수행 페이즈로 변경
    }


    private void PerformCPUTurn()
    {
        List<Card.CardType> possibleActions = new List<Card.CardType> { Card.CardType.Slash, Card.CardType.Block, Card.CardType.Stab };
        if (lastCpuCardType == Card.CardType.Slash)
        {
            possibleActions.Remove(Card.CardType.Slash);
        }

        int randomIndex = Random.Range(0, possibleActions.Count);
        Card.CardType chosenAction = possibleActions[randomIndex];

        // 선택된 행동 저장 및 처리
        lastCpuCardType = chosenAction;
        // 예: 선택된 카드 타입에 따른 로직 실행
    }


    IEnumerator ExecuteActions()
    {
        // 동작 수행 페이즈 로직
        // 예: 플레이어와 CPU의 선택에 따른 결과 처리

        yield return StartCoroutine(gameActions.ExecuteActions(playerCardType, lastCpuCardType));
        playerCardType = Card.CardType.None; // 카드 선택 초기화
        yield return new WaitForSeconds(1);
        ChangeTurn(); // 동작 수행 페이즈 종료 후 플레이어 턴으로 변경
    }

    public void EndTurn()
    {
        lastPlayerCardType = playerCardType; // 마지막 플레이어 카드 타입 저장
        ChangeTurn();
    }

    // 선후공 상태를 반환하는 메서드
    public bool IsFirstPlayerTurn()
    {
        return isFirstPlayerTurn;
    }
}
