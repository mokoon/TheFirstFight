using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HotMulti_TurnManager : MonoBehaviour
{
    public enum Turn
    {
        Player1,
        Player2,
        ActionPhase // ���� ���� ������ �߰�
    }
    //-------------------------------------------------------------------------------------
    //-------------------------------------------------------------------------------------
    public Turn currentTurn = Turn.Player1;
    public TextMeshProUGUI turnIndicator;
    public delegate void OnTurnChangeDelegate(Turn newTurn);
    public event OnTurnChangeDelegate onTurnChange;
    public HotMulti_GameActions gameActions; // Inspector���� �Ҵ�
    public Card.CardType Player1CardType = Card.CardType.Block; // �ʱⰪ ����
    public Card.CardType Player2CardType = Card.CardType.Block; // �ʱⰪ ����
    private Card.CardType lastCpuCardType = Card.CardType.Block; // �ʱⰪ ����
    public Card.CardType lastPlayer1CardType = Card.CardType.None;
    public Card.CardType lastPlayer2CardType = Card.CardType.None;
    private bool isFirstPlayerTurn = true; // ���İ� ������ ���� ����
    private bool isGameOver = false;

    void Start()
    {
        SetTurn(Turn.Player1);
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
            case Turn.Player1:
                turnIndicator.text = "�÷��̾�1 ��";
                Debug.Log("�÷��̾�1�� ���Դϴ�");
                break;
            case Turn.Player2:
                turnIndicator.text = "�÷��̾�2 ��";
                Debug.Log("�÷��̾�2�� ���Դϴ�");
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
            case Turn.Player1:
                if(isFirstPlayerTurn)
                    SetTurn(Turn.Player2);
                else SetTurn(Turn.ActionPhase);
                break;
            case Turn.Player2:
                if (isFirstPlayerTurn)
                    SetTurn(Turn.ActionPhase);
                else SetTurn(Turn.Player1);
                break;
            case Turn.ActionPhase:
                isFirstPlayerTurn = !isFirstPlayerTurn; // ���İ� ����
                if (isFirstPlayerTurn)
                    SetTurn(Turn.Player1);
                else SetTurn(Turn.Player2); // ���� ���� �� �÷��̾� ������ ���ư�
                break;
        }
    }


    public void SetTurn(Turn turn)
    {
        currentTurn = turn;
        UpdateTurnIndicator();
        onTurnChange?.Invoke(turn);

        if (currentTurn == Turn.Player1)
        {
            StartCoroutine(Player1Turn());
        }
        else if (currentTurn == Turn.Player2)
        {
            StartCoroutine(Player2Turn());
        }
        else if (currentTurn == Turn.ActionPhase)
        {
            StartCoroutine(ExecuteActions());
        }
    }

    IEnumerator Player1Turn()
    {
        Player1CardType = Card.CardType.None; // ī�� ���� �ʱ�ȭ
        yield return new WaitUntil(() => Player1CardType != Card.CardType.None); // �÷��̾ ī�带 ������ ������ ���

        // �÷��̾ ī�带 �����ϸ� �� ��ȯ ���� ����
        // ��: ChangeTurn(); �Ǵ� �÷��̾��� �ൿ ����
    }

    IEnumerator Player2Turn()
    {
        //playerCardType = Card.CardType.None; // ī�� ���� �ʱ�ȭ
        Player2CardType = Card.CardType.None; // ī�� ���� �ʱ�ȭ
        yield return new WaitUntil(() => Player2CardType != Card.CardType.None); // �÷��̾ ī�带 ������ ������ ���

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

        yield return StartCoroutine(gameActions.ExecuteActions(Player1CardType, Player2CardType));
        //Player1CardType = Card.CardType.None; // ī�� ���� �ʱ�ȭ
        //Player2CardType = Card.CardType.None; // ī�� ���� �ʱ�ȭ
        yield return new WaitForSeconds(1);
        ChangeTurn(); // ���� ���� ������ ���� �� �÷��̾� ������ ����
    }

    public void EndTurn()
    {
        lastPlayer1CardType = Player1CardType; // ������ �÷��̾� ī�� Ÿ�� ����
        lastPlayer2CardType = Player2CardType;
        ChangeTurn();
    }

    // ���İ� ���¸� ��ȯ�ϴ� �޼���
    public bool IsFirstPlayerTurn()
    {
        return isFirstPlayerTurn;
    }


}
