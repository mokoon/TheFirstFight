using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameActions : MonoBehaviour
{
    public HealthManager healthManager; // HealthManager ���� �߰�

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
        PerformPlayerAction(playerCardType, cpuCardType);
        PerformCpuAction(cpuCardType, playerCardType);
        Debug.Log("�÷��̾� ���� ī��:" + playerCardType + "��ǻ�� ���� ī��:" + cpuCardType);
        yield return new WaitForSeconds(1); // ���� ���࿡ �ð� ����

        // ���� �ܰ質 �߰� ����
    }
}

