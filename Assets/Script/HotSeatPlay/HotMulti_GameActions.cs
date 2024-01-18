using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HotMulti_GameActions : MonoBehaviour
{
    public HotMulti_HealthManager healthManager; // HealthManager 참조 추가
    public HotMulti_TurnManager turnManager;
    public TextMeshProUGUI PlayText; // 매 턴 수행 페이즈 표시 UI
    public TextMeshProUGUI endGameText; // 게임 종료 시 승자를 표시할 UI
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
                // 막기에 대한 로직 추가
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
                // 막기에 대한 로직 추가
        }
    }

    public IEnumerator ExecuteActions(Card.CardType player1CardType, Card.CardType player2CardType)
    {

        if (turnManager.IsFirstPlayerTurn())
        {
            PerformPlayer1Action(player1CardType, player2CardType);
            PerformPlayer2Action(player2CardType, player1CardType);
            StartCoroutine(ShowMessage("플레이어1 선택 카드:" + player1CardType + "\n플레이어2 선택 카드" + player2CardType, 1));
            Debug.Log("플레이어1 선택 카드:" + player1CardType + "플레이어2 선택 카드" + player2CardType);
        }
        else
        {
            PerformPlayer2Action(player2CardType, player1CardType);
            PerformPlayer1Action(player1CardType, player2CardType);
            StartCoroutine(ShowMessage("플레이어2 선택 카드:" + player2CardType + "\n플레이어1 선택 카드:" + player1CardType, 1));
            Debug.Log("플레이어2 선택 카드:" + player2CardType + "플레이어1 선택 카드:" + player1CardType);
        }
        if (healthManager.player1Health <= 0 || healthManager.player2Health <= 0)
        {
            Debug.Log("게임 종료");
            string winner;
            if (turnManager.IsFirstPlayerTurn())
            {
                winner = healthManager.player1Health <= 0 ? "player1" : "player2";
            }
            else
            {
                winner = healthManager.player1Health <= 0 ? "player2" : "player1";
            }
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

