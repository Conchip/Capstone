using System.Collections;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    private MovementTransform movement;
    [SerializeField]
    private float bulletDistance = 50;  // 적의 공격 최대 발사거리
    [SerializeField]
    private int damage = 15;            // 발사체 공격력

    public void Setup(Vector3 position)
    {
        movement = GetComponent<MovementTransform>();

        StartCoroutine("OnMove", position);
    }
    
    private IEnumerator OnMove(Vector3 targetPosition)
    {
        Vector3 start = transform.position;

        movement.MoveTo((targetPosition - transform.position).normalized);

        while(true)
        {
            if(Vector3.Distance(transform.position, start)>=bulletDistance)
            {
                Destroy(gameObject);

                yield break;
            }

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            //Debug.Log("Player Hit");
            other.GetComponent<PlayerController>().TakeDamge(damage);
            Destroy(gameObject);
        }
    }
}
