using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotMulti_CardActions : MonoBehaviour
{
    public static void Slash(HotMulti_HealthManager healthManager, Card.CardType opponentCardType, bool isPlayer)
    {
        if (opponentCardType != Card.CardType.Block)
        {
            if (isPlayer)
            {
                healthManager.DealDamageToPlayer2(3); // �÷��̾ CPU���� ����
            }
            else
            {
                healthManager.DealDamageToPlayer1(3); // CPU�� �÷��̾�� ����
            }
        }
    }

    // ���� ���⿡ ���� �޼��嵵 ���⿡ �߰�
    // ��:
    public static void Stab(HotMulti_HealthManager healthManager, bool isPlayer)
    {
        if (isPlayer)
        {
            healthManager.DealDamageToPlayer2(1); // �÷��̾ CPU���� ���
        }
        else
        {
            healthManager.DealDamageToPlayer1(1); // CPU�� �÷��̾�� ���
        }
    }

    public static void Block()
    {
        // ���� Ư���� ������ �ʿ� ���� �� ����
    }
}
