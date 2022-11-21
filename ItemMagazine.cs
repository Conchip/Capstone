using System.Collections;
using UnityEngine;

public class ItemMagazine : ItemBase
{
    [SerializeField]
    private GameObject magEffectPrefab;
    [SerializeField]
    private int increaseMagazine = 2;
    [SerializeField]
    private float rotateSpeed = 50;


    private IEnumerator Start()
    {
        float y = transform.position.y;
        while (true)
        {
            // y�� ���� ȸ��
            transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);

            yield return null;
        }
    }
    public override void Use(GameObject entity)
    {
        entity.GetComponentInChildren<WeaponAR>().IncreaseMagazine(increaseMagazine);
        Instantiate(magEffectPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

}
