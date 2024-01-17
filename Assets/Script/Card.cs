using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    private Vector3 initialPosition;
    private Vector3 screenPoint;
    private Vector3 offset;

    public float returnSpeed = 20.0f; // 카드가 초기 위치로 돌아가는 속도. 사용자가 조절할 수 있습니다.
    private BoxCollider2D cardCollider;
    private BoxCollider2D playAreaCollider;

    public string cardDescription; // 카드 설명
    public TextMeshProUGUI descriptionText; // 카드 설명을 표시할 UI Text

    public enum CardType
    {
        None,  // 카드가 선택되지 않았음을 나타냄
        Slash, // 베기
        Block, // 막기
        Stab   // 찌르기

    }

    public CardType cardType; // 카드의 종류를 저장하는 변수

    void Start()
    {
        initialPosition = transform.position; // 카드의 초기 위치 저장
        cardCollider = GetComponent<BoxCollider2D>(); // 카드의 Collider 가져오기
        playAreaCollider = GameObject.Find("Play_area").GetComponent<BoxCollider2D>(); // Play_area의 Collider 가져오기
    }
    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
    }

    void OnMouseDrag()
    {
        Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
        transform.position = cursorPosition;
    }

    void OnMouseUp()
    {
        if (IsCardOverPlayArea())
        {
            float coverage = CalculateCoverage();
            if (coverage >= 0.5f) // 덮인 영역이 50% 이상인 경우
            {
                CenterCardOnPlayArea(); // Play_area의 중앙으로 카드 이동
            }
            else
            {
                ReturnToInitialPosition(); // 초기 위치로 되돌아감
            }
        }
        else
        {
            ReturnToInitialPosition(); // 초기 위치로 되돌아감
        }
    }

    private bool IsCardOverPlayArea()
    {
        return cardCollider.bounds.Intersects(playAreaCollider.bounds);
    }

    private float CalculateCoverage()
    {
        if (!cardCollider.bounds.Intersects(playAreaCollider.bounds))
        {
            return 0f;
        }

        Bounds intersection = new Bounds();
        Vector3 maxIntersection = Vector3.Min(cardCollider.bounds.max, playAreaCollider.bounds.max);
        Vector3 minIntersection = Vector3.Max(cardCollider.bounds.min, playAreaCollider.bounds.min);
        intersection.SetMinMax(minIntersection, maxIntersection);

        float coverageArea = intersection.size.x * intersection.size.y;
        float playArea = playAreaCollider.bounds.size.x * playAreaCollider.bounds.size.y;
        return coverageArea / playArea; // 덮인 영역 비율 계산
    }

    private void CenterCardOnPlayArea()
    {
        transform.position = playAreaCollider.bounds.center;
    }

    public void ReturnToInitialPosition()
    {
        StartCoroutine(ReturnToStartPosition());
    }

    IEnumerator ReturnToStartPosition()
    {
        while (Vector3.Distance(transform.position, initialPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, initialPosition, returnSpeed * Time.deltaTime);
            yield return null;
        }
    }

    // 되돌아가는 속도를 조절하는 함수 (예: UI 슬라이더와 연결)
    public void SetReturnSpeed(float speed)
    {
        returnSpeed = speed;
    }

    void OnMouseEnter()
    {
        // 마우스가 카드 위에 있을 때 설명 텍스트 활성화 및 내용 설정
        descriptionText.text = cardDescription;
        descriptionText.gameObject.SetActive(true);
    }

    void OnMouseExit()
    {
        // 마우스가 카드에서 벗어났을 때 설명 텍스트 비활성화
        descriptionText.gameObject.SetActive(false);
    }
}
