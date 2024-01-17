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
                healthManager.DealDamageToCPU(3); // 플레이어가 CPU에게 베기
            }
            else
            {
                healthManager.DealDamageToPlayer(3); // CPU가 플레이어에게 베기
            }
        }
    }

    // 찌르기와 막기에 대한 메서드도 여기에 추가
    // 예:
    public static void Stab(HealthManager healthManager, bool isPlayer)
    {
        if (isPlayer)
        {
            healthManager.DealDamageToCPU(1); // 플레이어가 CPU에게 찌르기
        }
        else
        {
            healthManager.DealDamageToPlayer(1); // CPU가 플레이어에게 찌르기
        }
    }

    public static void Block()
    {
        // 막기 특별한 동작이 필요 없을 수 있음
    }
}
