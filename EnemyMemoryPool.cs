using System.Collections;
using UnityEngine;

public class EnemyMemoryPool : MonoBehaviour
{
    [SerializeField]
    private Transform target;                   // ���� ��ǥ(�÷��̾�)
    [SerializeField]
    private Transform enemySpawner;             // ���� �����Ǵ� ��ġ ������
    [SerializeField]
    private GameObject enemySpawnPointPrefab;   // ���� �����ϱ� �� ���� ���� ��ġ�� �˷��ִ� ������
    [SerializeField]
    private GameObject enemyPrefab;             // �����Ǵ� �� ������
    [SerializeField]
    private float enemySpawnTime = 1;           // �� ���� �ֱ�
    [SerializeField]
    private float enemySpawnLatency = 1;        // Ÿ�� ���� �� ���� �����ϱ� ���� ��� �ð�
    [SerializeField]
    private float maxCount = 10;                // �� �ִ� ���� ��


    
    private MemoryPool spawnPointMemoryPool;    // �� ���� ��ġ�� �˷��ִ� ������Ʈ ����, Ȱ��/��Ȱ�� ����
    private MemoryPool enemyMemoryPool;         // �� ����, Ȱ��/��Ȱ�� ����

    private int numberOfEnemiesSpawnedAtOnce = 1;   // ���ÿ� �����Ǵ� ���� ����
    private Vector2Int spawnSize = new Vector2Int(30, 30);  // ���� ��ġ ���� ũ��

    private void Awake()
    {
        spawnPointMemoryPool = new MemoryPool(enemySpawnPointPrefab);
        enemyMemoryPool = new MemoryPool(enemyPrefab);

        StartCoroutine("SpawnTile");
    }

    private IEnumerator SpawnTile()
    {
        int currentNumber = 0;
        while(true)
        {
            if (currentNumber >= maxCount)
            {
                StopCoroutine("SpawnEnemy");
                break;
            }
            for (int i=0; i< numberOfEnemiesSpawnedAtOnce; ++i)
            {
                GameObject item = spawnPointMemoryPool.ActivePoolItem();

                item.transform.position = new Vector3(enemySpawner.position.x - Random.Range(-spawnSize.x * 0.49f, spawnSize.x * 0.49f), enemySpawner.position.y,
                                                        enemySpawner.position.z - Random.Range(-spawnSize.y * 0.49f, spawnSize.y * 0.49f));

                StartCoroutine("SpawnEnemy", item);
            }
            currentNumber++;
  
            yield return new WaitForSeconds(enemySpawnTime);
        }
    }
    
    private IEnumerator SpawnEnemy(GameObject point)
    {
        yield return new WaitForSeconds(enemySpawnLatency);

        // �� ������Ʈ�� �����ϰ�, ���� ��ġ�� point�� ��ġ�� ����
        GameObject item = enemyMemoryPool.ActivePoolItem();
        item.transform.position = point.transform.position;

        item.GetComponent<EnemyFSM>().Setup(target, this);

        // Ÿ�� ������Ʈ ��Ȱ��ȭ
        spawnPointMemoryPool.DeactivatePoolItem(point);
    }

    public void DeactivateEnemy(GameObject enemy)
    {
        enemyMemoryPool.DeactivatePoolItem(enemy);
    }
}
