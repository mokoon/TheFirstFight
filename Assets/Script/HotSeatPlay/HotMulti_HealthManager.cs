using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro 네임스페이스 추가

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

    // 여기에 다른 게임 로직을 추가할 수 있습니다.
}
