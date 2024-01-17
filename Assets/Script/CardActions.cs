using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardActions : MonoBehaviour
{
    public static void Slash(HealthManager healthManager, Card.CardType opponentCardType, bool isPlayer)
    {
        if (opponentCardType != Card.CardType.Block)
        {
            if (isPlayer)
            {
                healthManager.DealDamageToCPU(3); // �÷��̾ CPU���� ����
            }
            else
            {
                healthManager.DealDamageToPlayer(3); // CPU�� �÷��̾�� ����
            }
        }
    }

    // ���� ���⿡ ���� �޼��嵵 ���⿡ �߰�
    // ��:
    public static void Stab(HealthManager healthManager, bool isPlayer)
    {
        if (isPlayer)
        {
            healthManager.DealDamageToCPU(1); // �÷��̾ CPU���� ���
        }
        else
        {
            healthManager.DealDamageToPlayer(1); // CPU�� �÷��̾�� ���
        }
    }

    public static void Block()
    {
        // ���� Ư���� ������ �ʿ� ���� �� ����
    }
}
