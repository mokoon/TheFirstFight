using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HotMulti_GameActions : MonoBehaviour
{
    public HotMulti_HealthManager healthManager; // HealthManager ���� �߰�
    public HotMulti_TurnManager turnManager;
    public TextMeshProUGUI PlayText; // �� �� ���� ������ ǥ�� UI
    public TextMeshProUGUI endGameText; // ���� ���� �� ���ڸ� ǥ���� UI
    public void PerformPlayer1Action(Card.CardType player1CardType, Card.CardType player2CardType)
    {
        switch (player1CardType)
        {
            case Card.CardType.Slash:
                HotMulti_CardActions.Slash(healthManager, player2CardType, true);
                break;
            case Card.CardType.Stab:
                HotMulti_CardActions.Stab(healthManager, true);
                break;
                // ���⿡ ���� ���� �߰�
        }
    }

    public void PerformPlayer2Action(Card.CardType player2CardType, Card.CardType player1CardType)
    {
        switch (player2CardType)
        {
            case Card.CardType.Slash:
                HotMulti_CardActions.Slash(healthManager, player1CardType, false);
                break;
            case Card.CardType.Stab:
                HotMulti_CardActions.Stab(healthManager, false);
                break;
                // ���⿡ ���� ���� �߰�
        }
    }

    public IEnumerator ExecuteActions(Card.CardType player1CardType, Card.CardType player2CardType)
    {

        if (turnManager.IsFirstPlayerTurn())
        {
            PerformPlayer1Action(player1CardType, player2CardType);
            PerformPlayer2Action(player2CardType, player1CardType);
            StartCoroutine(ShowMessage("�÷��̾�1 ���� ī��:" + player1CardType + "\n�÷��̾�2 ���� ī��" + player2CardType, 1));
            Debug.Log("�÷��̾�1 ���� ī��:" + player1CardType + "�÷��̾�2 ���� ī��" + player2CardType);
        }
        else
        {
            PerformPlayer2Action(player2CardType, player1CardType);
            PerformPlayer1Action(player1CardType, player2CardType);
            StartCoroutine(ShowMessage("�÷��̾�2 ���� ī��:" + player2CardType + "\n�÷��̾�1 ���� ī��:" + player1CardType, 1));
            Debug.Log("�÷��̾�2 ���� ī��:" + player2CardType + "�÷��̾�1 ���� ī��:" + player1CardType);
        }
        if (healthManager.player1Health <= 0 || healthManager.player2Health <= 0)
        {
            Debug.Log("���� ����");
            string winner;
            if (turnManager.IsFirstPlayerTurn())
            {
                winner = healthManager.player1Health <= 0 ? "player1" : "player2";
            }
            else
            {
                winner = healthManager.player1Health <= 0 ? "player2" : "player1";
            }
            Debug.Log(winner + "�� �¸�!");
            endGameText.text = winner + " Wins!";
            turnManager.EndGame(); // ���� ���� ���·� ����
            yield break; // ���� ����
        }
        yield return new WaitForSeconds(1); // ���� ���࿡ �ð� ����

        // ���� �ܰ質 �߰� ����
    }

    IEnumerator ShowMessage(string message, float delay)
    {
        PlayText.text = message;
        PlayText.color = new Color(PlayText.color.r, PlayText.color.g, PlayText.color.b, 1f); // ���İ� �ʱ�ȭ
        yield return new WaitForSeconds(delay);

        // �޽��� ����� ȿ��
        while (PlayText.color.a > 0.0f)
        {
            PlayText.color = new Color(PlayText.color.r, PlayText.color.g, PlayText.color.b, PlayText.color.a - Time.deltaTime);
            yield return null;
        }
        PlayText.text = "";
    }
}

