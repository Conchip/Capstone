using System.Collections;
using UnityEngine;

public class ItemMedkit : ItemBase
{
    [SerializeField]
    private GameObject hpEffectPrefab;
    [SerializeField]
    private int increaseHP = 50;
    [SerializeField]
    private float moveDistance = 0.2f;
    [SerializeField]
    private float pingpongSpeed = 0.5f;
    [SerializeField]
    private float rotateSpeed = 50;
    // Start is called before the first frame update
    private IEnumerator Start()
    {
        float y = transform.position.y;
        while (true)
        {
            // y�� ���� ȸ��
            transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
            // ó�� ��ġ�� ��ġ�� �������� y��ġ�� ���� �̵�
            Vector3 position = transform.position;
            position.y = Mathf.Lerp(y, y + moveDistance, Mathf.PingPong(Time.time * pingpongSpeed, 1));
            transform.position = position;

            yield return null;
        }
    }
    public override void Use(GameObject entity)
    {
        entity.GetComponent<Status>().IncreaseHP(increaseHP);
        Instantiate(hpEffectPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
