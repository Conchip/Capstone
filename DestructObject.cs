using UnityEngine;

public class DestructObject : MonoBehaviour
{
    [Header("DestructObject")]
    [SerializeField]
    private GameObject destructObjectPices;

    private bool isDestroyed = false;

    [Header("InteractionObject")]
    [SerializeField]
    protected int maxHP = 200;
    protected int currentHP;

    private void Awake()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        if(currentHP <= 0 && isDestroyed == false)
        {
            isDestroyed = true;

            Instantiate(destructObjectPices, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
