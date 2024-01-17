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


    void Start()
    {
        SetTurn(Turn.Player);
    }

    void UpdateTurnIndicator()
    {
        switch (currentTurn)
        {
            case Turn.Player:
                turnIndicator.text = "�÷��̾� ��";
                break;
            case Turn.CPU:
                turnIndicator.text = "��ǻ�� ��";
                break;
            case Turn.ActionPhase:
                turnIndicator.text = "���� ���� ��";
                break;
        }
    }

    public void ChangeTurn()
    {
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
                SetTurn(Turn.Player); // ���� ���� �� �÷��̾� ������ ���ư�
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
        ChangeTurn(); // CPU �� ���� �� ���� ���� ������� ����
    }

    private void PerformCPUTurn()
    {
        // CPU �ൿ ���� �� ����
    }

    IEnumerator ExecuteActions()
    {
        // ���� ���� ������ ����
        // ��: �÷��̾�� CPU�� ���ÿ� ���� ��� ó��

        yield return new WaitForSeconds(1); // ���� ���࿡ �ð� ����

        ChangeTurn(); // ���� ���� ������ ���� �� �÷��̾� ������ ����
    }
}
