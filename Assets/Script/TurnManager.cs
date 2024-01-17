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
        ActionPhase // ���� ���� ������ �߰�
    }

    public Turn currentTurn = Turn.Player;
    public TextMeshProUGUI turnIndicator;
    public delegate void OnTurnChangeDelegate(Turn newTurn);
    public event OnTurnChangeDelegate onTurnChange;
    public GameActions gameActions; // Inspector���� �Ҵ�
    public Card.CardType playerCardType = Card.CardType.Block; // �ʱⰪ ����
    private Card.CardType lastCpuCardType = Card.CardType.Block; // �ʱⰪ ����
    public Card.CardType lastPlayerCardType = Card.CardType.None;
    private bool isFirstPlayerTurn = true; // ���İ� ������ ���� ����
    private bool isGameOver = false;

    void Start()
    {
        SetTurn(Turn.Player);
    }

    public void EndGame()
    {
        isGameOver = true;
        // �ʿ��� ��� �߰����� ���� ���� ���� ����
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
                turnIndicator.text = "�÷��̾� ��";
                Debug.Log("�÷��̾��� ���Դϴ�");
                break;
            case Turn.CPU:
                turnIndicator.text = "��ǻ�� ��";
                Debug.Log("��ǻ���� ���Դϴ�");
                break;
            case Turn.ActionPhase:
                turnIndicator.text = "���� ���� ��";
                Debug.Log("���� ���� ������ �Դϴ�");
                break;
        }
    }

    public void ChangeTurn()
    {
        if (isGameOver) return;
        // �� ������ ���� ���� �� ����
        switch (currentTurn)
        {
            case Turn.Player:
                SetTurn(Turn.CPU);
                break;
            case Turn.CPU:
                SetTurn(Turn.ActionPhase);
                break;
            case Turn.ActionPhase:
                isFirstPlayerTurn = !isFirstPlayerTurn; // ���İ� ����
                SetTurn(Turn.Player); // ���� ���� �� �÷��̾� ������ ���ư�
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
        //playerCardType = Card.CardType.None; // ī�� ���� �ʱ�ȭ
        yield return new WaitUntil(() => playerCardType != Card.CardType.None); // �÷��̾ ī�带 ������ ������ ���

        // �÷��̾ ī�带 �����ϸ� �� ��ȯ ���� ����
        // ��: ChangeTurn(); �Ǵ� �÷��̾��� �ൿ ����
    }


    IEnumerator CPUTurn()
    {
        yield return new WaitForSeconds(1);
        PerformCPUTurn();
        ChangeTurn(); // CPU �� ���� �� ���� ���� ������� ����
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

        // ���õ� �ൿ ���� �� ó��
        lastCpuCardType = chosenAction;
        // ��: ���õ� ī�� Ÿ�Կ� ���� ���� ����
    }


    IEnumerator ExecuteActions()
    {
        // ���� ���� ������ ����
        // ��: �÷��̾�� CPU�� ���ÿ� ���� ��� ó��

        yield return StartCoroutine(gameActions.ExecuteActions(playerCardType, lastCpuCardType));
        playerCardType = Card.CardType.None; // ī�� ���� �ʱ�ȭ
        yield return new WaitForSeconds(1);
        ChangeTurn(); // ���� ���� ������ ���� �� �÷��̾� ������ ����
    }

    public void EndTurn()
    {
        lastPlayerCardType = playerCardType; // ������ �÷��̾� ī�� Ÿ�� ����
        ChangeTurn();
    }

    // ���İ� ���¸� ��ȯ�ϴ� �޼���
    public bool IsFirstPlayerTurn()
    {
        return isFirstPlayerTurn;
    }
}
