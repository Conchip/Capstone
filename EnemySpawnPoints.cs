using System.Collections;
using UnityEngine;

public class EnemySpawnPoints : MonoBehaviour
{
    [SerializeField]
    private float fadeSpeed = 4;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void OnEnable()
    {
        StartCoroutine("OnFadeEffect"); // �޸�Ǯ�� ����� ������Ʈon
    }

    private void OnDisable()
    {
        StopCoroutine("OnFadeEffect");  // �޸�Ǯ�� ����� ������Ʈ off
    }
    private IEnumerator OnFadeEffect()
    {
        while (true)
        {
            Color color = meshRenderer.material.color;
            color.a = Mathf.Lerp(1, 0, Mathf.PingPong(Time.time * fadeSpeed, 1));
            meshRenderer.material.color = color;

            yield return null;
        }
    }
}
