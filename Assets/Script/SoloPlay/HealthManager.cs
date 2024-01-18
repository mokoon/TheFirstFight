using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshPro 네임스페이스 추가

public class HealthManager : MonoBehaviour
{
    public int playerHealth = 10;
    public int cpuHealth = 10;

    public TextMeshProUGUI p1HpText;
    public TextMeshProUGUI p2HpText;

    void Start()
    {
        UpdateHealthUI();
    }

    public void DealDamageToPlayer(int damage)
    {
        playerHealth -= damage;
        playerHealth = Mathf.Clamp(playerHealth, 0, 10);
        UpdateHealthUI();
    }

    public void DealDamageToCPU(int damage)
    {
        cpuHealth -= damage;
        cpuHealth = Mathf.Clamp(cpuHealth, 0, 10);
        UpdateHealthUI();
    }

    void UpdateHealthUI()
    {
        p1HpText.text = playerHealth.ToString();
        p2HpText.text = cpuHealth.ToString();
    }

    // 여기에 다른 게임 로직을 추가할 수 있습니다.
}
