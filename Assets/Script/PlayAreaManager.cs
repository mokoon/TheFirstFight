using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayAreaManager : MonoBehaviour
{
    public BoxCollider2D playAreaCollider;
    public TextMeshProUGUI messageText;
    public TurnManager turnManager;

    void Start()
    {
        messageText.text = "";
    }

    public void OnButtonClick()
    {
        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(playAreaCollider.bounds.center, playAreaCollider.bounds.size, 0f);
        bool cardFound = false;
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Card"))
            {
                Card card = hitCollider.gameObject.GetComponent<Card>();
                if (card != null)
                {
                    if (card.cardType == Card.CardType.Slash && turnManager.lastPlayerCardType == Card.CardType.Slash)
                    {
                        StartCoroutine(ShowMessage("이전턴에 베기를 사용하였습니다!", 1));
                        card.ReturnToInitialPosition(); // 카드를 초기 위치로 되돌림
                        return; // 메서드 종료
                    }
                    Debug.Log("카드 수행: " + card.cardType.ToString());
                    card.ReturnToInitialPosition();
                    cardFound = true;
                    turnManager.playerCardType = card.cardType;
                    //turnManager.ChangeTurn();
                    break;
                }
            }
        }

        if (cardFound)
        {
            turnManager.EndTurn();
        }
        else
        {
            StartCoroutine(ShowMessage("카드를 올려주세요!", 1)); // 카드가 없을 경우 메시지 표시
        }
    }

    IEnumerator ShowMessage(string message, float delay)
    {
        messageText.text = message;
        messageText.color = new Color(messageText.color.r, messageText.color.g, messageText.color.b, 1f); // 알파값 초기화
        yield return new WaitForSeconds(delay);

        // 메시지 사라짐 효과
        while (messageText.color.a > 0.0f)
        {
            messageText.color = new Color(messageText.color.r, messageText.color.g, messageText.color.b, messageText.color.a - Time.deltaTime);
            yield return null;
        }
        messageText.text = "";
    }
}
