using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameActions : MonoBehaviour
{
    public HealthManager healthManager; // HealthManager ���� �߰�
    public TurnManager turnManager;
    public TextMeshProUGUI PlayText; // �� �� ���� ������ ǥ�� UI
    public TextMeshProUGUI endGameText; // ���� ���� �� ���ڸ� ǥ���� UI
    public void PerformPlayerAction(Card.CardType playerCardType, Card.CardType cpuCardType)
    {
        switch (playerCardType)
        {
            case Card.CardType.Slash:
                CardActions.Slash(healthManager, cpuCardType, true);
                break;
            case Card.CardType.Stab:
                CardActions.Stab(healthManager, true);
                break;
                // ���⿡ ���� ���� �߰�
        }
    }

    public void PerformCpuAction(Card.CardType cpuCardType, Card.CardType playerCardType)
    {
        switch (cpuCardType)
        {
            case Card.CardType.Slash:
                CardActions.Slash(healthManager, playerCardType, false);
                break;
            case Card.CardType.Stab:
                CardActions.Stab(healthManager, false);
                break;
                // ���⿡ ���� ���� �߰�
        }
    }

    public IEnumerator ExecuteActions(Card.CardType playerCardType, Card.CardType cpuCardType)
    {

        if (turnManager.IsFirstPlayerTurn())
        {
            PerformPlayerAction(playerCardType, cpuCardType);
            PerformCpuAction(cpuCardType, playerCardType);
            StartCoroutine(ShowMessage("�÷��̾� ���� ī��:" + playerCardType + "\n��ǻ�� ���� ī��" + cpuCardType, 1));
            Debug.Log("�÷��̾� ���� ī��:" + playerCardType + "��ǻ�� ���� ī��" + cpuCardType);
        }
        else
        {
            PerformCpuAction(cpuCardType, playerCardType);
            PerformPlayerAction(playerCardType, cpuCardType);
            StartCoroutine(ShowMessage("��ǻ�� ���� ī��:" + cpuCardType + "\n�÷��̾� ���� ī��:" + playerCardType, 1));
            Debug.Log("��ǻ�� ���� ī��:" + cpuCardType + "�÷��̾� ���� ī��:" + playerCardType);
        }
        if (healthManager.playerHealth <= 0 || healthManager.cpuHealth <= 0)
        {
            Debug.Log("���� ����");
            
            string winner = healthManager.playerHealth <= 0 ? "CPU" : "Player";
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

