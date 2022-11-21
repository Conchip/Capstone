using UnityEngine;

public class ParticleAutoDestroyerByTime : MonoBehaviour
{
    private ParticleSystem particle;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    private void Update()
    {
        // ��ƼŬ�� ������� �ƴϸ� ����
        if(particle.isPlaying == false)
        {
            Destroy(gameObject);
        }
    }
}