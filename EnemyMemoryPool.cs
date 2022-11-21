using System.Collections;
using UnityEngine;

public class EnemyMemoryPool : MonoBehaviour
{
    [SerializeField]
    private Transform target;                   // 적의 목표(플레이어)
    [SerializeField]
    private Transform enemySpawner;             // 적이 생성되는 위치 기준점
    [SerializeField]
    private GameObject enemySpawnPointPrefab;   // 적이 등장하기 전 적의 등장 위치를 알려주는 프리펩
    [SerializeField]
    private GameObject enemyPrefab;             // 생성되는 적 프리펩
    [SerializeField]
    private float enemySpawnTime = 1;           // 적 생성 주기
    [SerializeField]
    private float enemySpawnLatency = 1;        // 타일 생성 후 적이 등장하기 까지 대기 시간
    [SerializeField]
    private float maxCount = 10;                // 적 최대 생성 수


    
    private MemoryPool spawnPointMemoryPool;    // 적 등장 위치를 알려주는 오브젝트 생성, 활성/비활성 관리
    private MemoryPool enemyMemoryPool;         // 적 생성, 활성/비활성 관리

    private int numberOfEnemiesSpawnedAtOnce = 1;   // 동시에 생성되는 적의 숫자
    private Vector2Int spawnSize = new Vector2Int(30, 30);  // 스폰 위치 범위 크기

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

        // 적 오브젝트를 생성하고, 적의 위치를 point의 위치로 선정
        GameObject item = enemyMemoryPool.ActivePoolItem();
        item.transform.position = point.transform.position;

        item.GetComponent<EnemyFSM>().Setup(target, this);

        // 타일 오브젝트 비활성화
        spawnPointMemoryPool.DeactivatePoolItem(point);
    }

    public void DeactivateEnemy(GameObject enemy)
    {
        enemyMemoryPool.DeactivatePoolItem(enemy);
    }
}
