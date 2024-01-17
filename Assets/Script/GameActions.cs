using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameActions : MonoBehaviour
{
    public HealthManager healthManager; // HealthManager 참조 추가

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
                // 막기에 대한 로직 추가
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
                // 막기에 대한 로직 추가
        }
    }

    public IEnumerator ExecuteActions(Card.CardType playerCardType, Card.CardType cpuCardType)
    {
        PerformPlayerAction(playerCardType, cpuCardType);
        PerformCpuAction(cpuCardType, playerCardType);
        Debug.Log("플레이어 선택 카드:" + playerCardType + "컴퓨터 선택 카드:" + cpuCardType);
        yield return new WaitForSeconds(1); // 동작 수행에 시간 지연

        // 다음 단계나 추가 로직
    }
}

