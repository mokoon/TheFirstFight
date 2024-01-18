using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static HotMulti_TurnManager;

public class HotMulti_PlayAreaManager : MonoBehaviour
{
    public BoxCollider2D playAreaCollider;
    public TextMeshProUGUI messageText;
    public HotMulti_TurnManager turnManager;

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
                    switch (turnManager.currentTurn)
                    {
                        case Turn.Player1:
                            {
                                if (card.cardType == Card.CardType.Slash && turnManager.lastPlayer1CardType == Card.CardType.Slash)
                                {
                                    StartCoroutine(ShowMessage("�����Ͽ� ���⸦ ����Ͽ����ϴ�!", 1));
                                    card.ReturnToInitialPosition(); // ī�带 �ʱ� ��ġ�� �ǵ���
                                    return; // �޼��� ����
                                }
                                Debug.Log("ī�� ����: " + card.cardType.ToString());
                                card.ReturnToInitialPosition();
                                cardFound = true;
                                turnManager.Player1CardType = card.cardType;
                                //turnManager.ChangeTurn();
                                break;
                            }
                        case Turn.Player2:
                            {
                                if (card.cardType == Card.CardType.Slash && turnManager.lastPlayer2CardType == Card.CardType.Slash)
                                {
                                    StartCoroutine(ShowMessage("�����Ͽ� ���⸦ ����Ͽ����ϴ�!", 1));
                                    card.ReturnToInitialPosition(); // ī�带 �ʱ� ��ġ�� �ǵ���
                                    return; // �޼��� ����
                                }
                                Debug.Log("ī�� ����: " + card.cardType.ToString());
                                card.ReturnToInitialPosition();
                                cardFound = true;
                                turnManager.Player2CardType = card.cardType;
                                //turnManager.ChangeTurn();
                                break;
                            }
                    }
                }
            }
        }

        if (cardFound)
        {
            turnManager.EndTurn();
        }
        else
        {
            StartCoroutine(ShowMessage("ī�带 �÷��ּ���!", 1)); // ī�尡 ���� ��� �޽��� ǥ��
        }
    }

    IEnumerator ShowMessage(string message, float delay)
    {
        messageText.text = message;
        messageText.color = new Color(messageText.color.r, messageText.color.g, messageText.color.b, 1f); // ���İ� �ʱ�ȭ
        yield return new WaitForSeconds(delay);

        // �޽��� ����� ȿ��
        while (messageText.color.a > 0.0f)
        {
            messageText.color = new Color(messageText.color.r, messageText.color.g, messageText.color.b, messageText.color.a - Time.deltaTime);
            yield return null;
        }
        messageText.text = "";
    }
}
