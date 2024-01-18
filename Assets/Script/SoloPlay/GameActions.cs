using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameActions : MonoBehaviour
{
    public HealthManager healthManager; // HealthManager 참조 추가
    public TurnManager turnManager;
    public TextMeshProUGUI PlayText; // 매 턴 수행 페이즈 표시 UI
    public TextMeshProUGUI endGameText; // 게임 종료 시 승자를 표시할 UI
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

        if (turnManager.IsFirstPlayerTurn())
        {
            PerformPlayerAction(playerCardType, cpuCardType);
            PerformCpuAction(cpuCardType, playerCardType);
            StartCoroutine(ShowMessage("플레이어 선택 카드:" + playerCardType + "\n컴퓨터 선택 카드" + cpuCardType, 1));
            Debug.Log("플레이어 선택 카드:" + playerCardType + "컴퓨터 선택 카드" + cpuCardType);
        }
        else
        {
            PerformCpuAction(cpuCardType, playerCardType);
            PerformPlayerAction(playerCardType, cpuCardType);
            StartCoroutine(ShowMessage("컴퓨터 선택 카드:" + cpuCardType + "\n플레이어 선택 카드:" + playerCardType, 1));
            Debug.Log("컴퓨터 선택 카드:" + cpuCardType + "플레이어 선택 카드:" + playerCardType);
        }
        if (healthManager.playerHealth <= 0 || healthManager.cpuHealth <= 0)
        {
            Debug.Log("게임 종료");
            
            string winner = healthManager.playerHealth <= 0 ? "CPU" : "Player";
            Debug.Log(winner + "의 승리!");
            endGameText.text = winner + " Wins!";
            turnManager.EndGame(); // 게임 종료 상태로 설정
            yield break; // 게임 종료
        }
        yield return new WaitForSeconds(1); // 동작 수행에 시간 지연

        // 다음 단계나 추가 로직
    }

    IEnumerator ShowMessage(string message, float delay)
    {
        PlayText.text = message;
        PlayText.color = new Color(PlayText.color.r, PlayText.color.g, PlayText.color.b, 1f); // 알파값 초기화
        yield return new WaitForSeconds(delay);

        // 메시지 사라짐 효과
        while (PlayText.color.a > 0.0f)
        {
            PlayText.color = new Color(PlayText.color.r, PlayText.color.g, PlayText.color.b, PlayText.color.a - Time.deltaTime);
            yield return null;
        }
        PlayText.text = "";
    }
}

