using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro ���ӽ����̽� �߰�

public class HotMulti_HealthManager : MonoBehaviour
{
    public int player1Health = 10;
    public int player2Health = 10;

    public TextMeshProUGUI p1HpText;
    public TextMeshProUGUI p2HpText;

    void Start()
    {
        UpdateHealthUI();
    }

    public void DealDamageToPlayer1(int damage)
    {
        player1Health -= damage;
        player1Health = Mathf.Clamp(player1Health, 0, 10);
        UpdateHealthUI();
    }

    public void DealDamageToPlayer2(int damage)
    {
        player2Health -= damage;
        player2Health = Mathf.Clamp(player2Health, 0, 10);
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        p1HpText.text = player1Health.ToString();
        p2HpText.text = player2Health.ToString();
    }

    // ���⿡ �ٸ� ���� ������ �߰��� �� �ֽ��ϴ�.
}
