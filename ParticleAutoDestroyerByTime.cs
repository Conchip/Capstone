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
        // 파티클이 재생중이 아니면 삭제
        if(particle.isPlaying == false)
        {
            Destroy(gameObject);
        }
    }
}
