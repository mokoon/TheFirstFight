using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Card : MonoBehaviour
{
    private Vector3 initialPosition;
    private Vector3 screenPoint;
    private Vector3 offset;

    public float returnSpeed = 20.0f; // ī�尡 �ʱ� ��ġ�� ���ư��� �ӵ�. ����ڰ� ������ �� �ֽ��ϴ�.
    private BoxCollider2D cardCollider;
    private BoxCollider2D playAreaCollider;

    public string cardDescription; // ī�� ����
    public TextMeshProUGUI descriptionText; // ī�� ������ ǥ���� UI Text

    public enum CardType
    {
        None,  // ī�尡 ���õ��� �ʾ����� ��Ÿ��
        Slash, // ����
        Block, // ����
        Stab   // ���

    }

    public CardType cardType; // ī���� ������ �����ϴ� ����

    void Start()
    {
        initialPosition = transform.position; // ī���� �ʱ� ��ġ ����
        cardCollider = GetComponent<BoxCollider2D>(); // ī���� Collider ��������
        playAreaCollider = GameObject.Find("Play_area").GetComponent<BoxCollider2D>(); // Play_area�� Collider ��������
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
            if (coverage >= 0.5f) // ���� ������ 50% �̻��� ���
            {
                CenterCardOnPlayArea(); // Play_area�� �߾����� ī�� �̵�
            }
            else
            {
                ReturnToInitialPosition(); // �ʱ� ��ġ�� �ǵ��ư�
            }
        }
        else
        {
            ReturnToInitialPosition(); // �ʱ� ��ġ�� �ǵ��ư�
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
        return coverageArea / playArea; // ���� ���� ���� ���
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

    // �ǵ��ư��� �ӵ��� �����ϴ� �Լ� (��: UI �����̴��� ����)
    public void SetReturnSpeed(float speed)
    {
        returnSpeed = speed;
    }

    void OnMouseEnter()
    {
        // ���콺�� ī�� ���� ���� �� ���� �ؽ�Ʈ Ȱ��ȭ �� ���� ����
        descriptionText.text = cardDescription;
        descriptionText.gameObject.SetActive(true);
    }

    void OnMouseExit()
    {
        // ���콺�� ī�忡�� ����� �� ���� �ؽ�Ʈ ��Ȱ��ȭ
        descriptionText.gameObject.SetActive(false);
    }
}
