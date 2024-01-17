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
                    Debug.Log("ī�� ����: " + card.cardType.ToString());
                    card.ReturnToInitialPosition();
                    cardFound = true;
                    turnManager.ChangeTurn();
                    break;
                }
            }
        }

        if (!cardFound)
        {
            StartCoroutine(ShowMessage("ī�带 �÷��ּ���!", 1));
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
