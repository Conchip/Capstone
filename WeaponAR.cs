using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class AmmoEvent : UnityEngine.Events.UnityEvent<int, int> { }
[System.Serializable]
public class MagazineEvent  :   UnityEngine.Events.UnityEvent<int> { }

public class WeaponAR : MonoBehaviour
{
    [HideInInspector]
    public AmmoEvent onAmmoEvent = new AmmoEvent();
    [HideInInspector]
    public MagazineEvent onMagazineEvent = new MagazineEvent();

    [Header("Shoot Effects")]
    [SerializeField]
    private GameObject muzzleFlashEffect;     // �ѱ� ȭ�� ����Ʈ on/off

    [Header("Spawn Points")]
    [SerializeField]
    private Transform casingSpawnPoint;     // ź�� ���� ��ġ
    [SerializeField]
    private Transform bulletSpawnPoint;     // źȯ ���� ��ġ

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip       audioClipTakeOutWeaopn;   // ���� ���� �����Ŭ��
    [SerializeField]
    private AudioClip audioClipShoot;   // ��� ����
    [SerializeField]
    private AudioClip audioClipReload;  // ������ ����


    [Header("Weapon Setting")]
    [SerializeField]
    private WeaponSetting weaponSetting;    // ���� ����

    [Header("Aim UI")]
    [SerializeField]
    private Image imageAim;     // default/Aim mode �� ���� Aim �̹��� Ȱ��ȭ/��Ȱ��ȭ

    private float lastAttackTime = 0;   // ������ �߻�ð� üũ�뵵
    private bool isReload = false;      // ������ ���� Ȯ��
    private bool isAttack = false;      // ���� ���� üũ�뵵
    private bool isModeChange = false;  // ��� ��ȯ ���� üũ�뵵
    private float defaultModeFOV = 60;  // �⺻ ��忡���� ī�޶� FOV��
    private float aimModeFOV = 30;      // AIM��忡���� ī�޶� FOV��
    
    private AudioSource audioSource;                  // ���� ��� ������Ʈ
    private PlayerAnimatorController animator;  // �ִϸ��̼� ��� ����
    private CasingMemoryPool casingMemoryPool;  // ź�� ���� �� Ȱ��/��Ȱ�� ����
    private ImpactMemoryPool impactMemoryPool;  // ���� ȿ�� ���� �� Ȱ��/��Ȱ�� ����
    private Camera mainCamera;      // ����(źȯ) �߻�

    // �ܺο��� �ʿ��� ������ �����ϱ� ���� ������ Get Property's
    public WeaponName WeaponName => weaponSetting.weaponName;
    public int CurrentMagazine => weaponSetting.currentMagazine;
    public int MaxMagazine => weaponSetting.maxMagazine;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponentInParent<PlayerAnimatorController>();
        casingMemoryPool = GetComponent<CasingMemoryPool>();
        impactMemoryPool = GetComponent<ImpactMemoryPool>();
        mainCamera = Camera.main;

        // ó���� ���޵Ǵ� źâ ���� �ִ�� ����
        weaponSetting.currentMagazine = weaponSetting.maxMagazine;
        // ó���� ���޵Ǵ� ź ���� �ִ�� ����
        weaponSetting.currentAmmo = weaponSetting.maxAmmo;
    }
    private void OnEnable()
    {
        PlaySound(audioClipTakeOutWeaopn);  // ���� ���� �����Ŭ�� ���
        muzzleFlashEffect.SetActive(false); // �ѱ� ȭ�� ����Ʈ ������Ʈ ��Ȱ��ȭ

        // ���Ⱑ Ȱ��ȭ �� �� �ش� ������ źâ ������ ����
        onMagazineEvent.Invoke(weaponSetting.currentMagazine);
        // ���Ⱑ Ȱ��ȭ �� �� �ش� ������ ź �� ������ ����
        onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);

        ResetVariables();
    }

    public void StartWeaponAction(int type=0)
    {
        // ������ �߿��� ���� �׼� �Ұ���
        if (isReload == true) return;
        // ��� ��ȯ�߿��� ���� �׼� �Ұ���
        if (isModeChange == true) return;
       if(type == 0)
        {
            // ���콺 ��Ŭ��(��� ����)
            if(weaponSetting.isAutomaticAttack == true)
            {
                StartCoroutine("OnAttackLoop");
            }
            // �ܹ�
            else
            {
                OnAttack();
            }
        }
       // ���콺 ��Ŭ�� (��� ��ȯ)
       else
        {
            // ���� ���� ���� ��� ��ȯ �Ұ���
            if (isAttack == true) return;

            StartCoroutine("OnModeChange");
        }
    }
    public void StopWeaponAction(int type=0)
    {
        // ���콺 ��Ŭ�� (��� ����)
        if(type == 0)
        {
            isAttack = false;
            StopCoroutine("OnAttackLoop");
        }
    }

    public void StartReload()
    {
        // ���� ������ ���̰ų� źâ ���� 0�̸� ������ �Ұ���
        if (isReload == true || weaponSetting.currentMagazine <=0) return;

        // ���� �׼� ���߿� 'R'Ű�� ���� �������� �õ��� ���� �׼� ���� �� ������
        StopWeaponAction();

        StartCoroutine("OnReload");
    }
    private IEnumerator OnAttackLoop()
    {
        while(true)
        {
            OnAttack();

            yield return null;
        }
    }
    public void OnAttack()
    {
        if(Time.time - lastAttackTime > weaponSetting.attackRate)
        {
            // �޸��� �߿��� ���ݺҰ���
            if(animator.MoveSpeed> 0.9f)
            {
                return;
            }
            // �����ֱⰡ �Ǿ�� ���� �Ҽ��ֵ��� �ϱ� ���� ���� �ð� ����
            lastAttackTime = Time.time;
            // ź���� ������ ��� �Ұ���
            if (weaponSetting.currentAmmo <= 0)
            {
                return;
            }
            // ���� �� currentAmmo 1 ����, ź �� UI ������Ʈ
            weaponSetting.currentAmmo--;
            onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);

            // ���� �ִϸ��̼� ���(��忡 ���� AimFire / Fire �ִϸ��̼� ���)
            // animator.Play("Shoot",-1, 0);
            string animation = animator.AimModeIs == true ? "AimFire" : "Fire";
            animator.Play(animation, -1, 0);
            // �ѱ� ȭ�� ����Ʈ ���
            if(animator.AimModeIs==false) StartCoroutine("OnMuzzleFlashEffect");
            // ��� ���� ���
            PlaySound(audioClipShoot);
            // ź�� ����
            casingMemoryPool.SpawnCasing(casingSpawnPoint.position, transform.right);

            // ������ �߻��� ���ϴ� ��ġ ���� (+Impact Effect)
            TwoStepRaycast();
        }
    }

    private void TwoStepRaycast()
    {
        Ray ray;
        RaycastHit hit;
        Vector3 targetPoint = Vector3.zero;

        // ȭ���� �߾� ��ǥ(AIm �������� Raycast ����)
        ray = mainCamera.ViewportPointToRay(Vector2.one * 0.5f);
        // ���� ��Ÿ�(attackDistance)�ȿ� �ε����� ������Ʈ�� ������ targetPoint�� ������ �ε��� ��ġ
        if(Physics.Raycast(ray, out hit, weaponSetting.attackDistance))
        {
            targetPoint = hit.point;
        }
        // ���� ��Ÿ� �ȿ� �ε����� ������Ʈ�� ������ targetPoint�� �ִ� ��Ÿ� ��ġ
        else
        {
            targetPoint = ray.origin + ray.direction * weaponSetting.attackDistance;
        }
        Debug.DrawRay(ray.origin, ray.direction * weaponSetting.attackDistance, Color.red);

        // ù��° Raycast�������� ����� targetPoint�� ��ǥ �������� ����
        // �ѱ��� ������������ �Ͽ� Raycast����
        Vector3 attackDirection = (targetPoint - bulletSpawnPoint.position).normalized;
        if(Physics.Raycast(bulletSpawnPoint.position, attackDirection, out hit, weaponSetting.attackDistance))
        {
            impactMemoryPool.SpawnImpact(hit);
            
            if(hit.transform.CompareTag("ImpactEnemy"))
            {
                hit.transform.GetComponent<EnemyFSM>().TakeDamage(weaponSetting.damage);
            }
            else if(hit.transform.CompareTag("InteractionObject"))
            {
                hit.transform.GetComponent<DestructObject>().TakeDamage(weaponSetting.damage);
            }
        }
        Debug.DrawRay(bulletSpawnPoint.position, attackDirection * weaponSetting.attackDistance, Color.blue);
    }

    private IEnumerator OnModeChange()
    {
        float current = 0;
        float percent = 0;
        float time = 0.35f;

        animator.AimModeIs = !animator.AimModeIs;
        imageAim.enabled = !imageAim.enabled;

        float start = mainCamera.fieldOfView;
        float end = animator.AimModeIs == true ? aimModeFOV : defaultModeFOV;

        isModeChange = true;

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;
            // mode�� ���� ī�޶��� Fov�� ����
            mainCamera.fieldOfView = Mathf.Lerp(start, end, percent);

            yield return null;
        }
        isModeChange = false;
    }
    private void ResetVariables()
    {
        isReload = false;
        isAttack = false;
        isModeChange = false;
    }
    private IEnumerator OnMuzzleFlashEffect()
    {
        muzzleFlashEffect.SetActive(true);
        yield return new WaitForSeconds(weaponSetting.attackRate * 0.3f);
        muzzleFlashEffect.SetActive(false);
    }

    private IEnumerator OnReload()
    {
        isReload = true;
        // ������ �ִϸ��̼�, ���� ���
        animator.OnReload();
        PlaySound(audioClipReload);

        while(true)
        {
            // ���尡 ������� �ƴϰ�, ���� �ִϸ��̼��� Movement�̸�
            // ������ �ִϸ��̼�(����) ����� ����Ǿ��ٴ� ��
            if (audioSource.isPlaying == false && (animator.CurrentAnimationIs("Movement") || animator.CurrentAnimationIs("AimFirePose")))
            {
                isReload = false;

                // ���� źâ ���� 1 ���ҽ�Ű��, �ٲ� źâ ������ Text UI�� ������Ʈ
                weaponSetting.currentMagazine--;
                onMagazineEvent.Invoke(weaponSetting.currentMagazine);
                // ���� ź ���� �ִ�� �����ϰ�, �ٲ� ź �� ������  Text UI�� ������Ʈ
                weaponSetting.currentAmmo = weaponSetting.maxAmmo;
                onAmmoEvent.Invoke(weaponSetting.currentAmmo, weaponSetting.maxAmmo);

                yield break;
            }
            yield return null;
        }
    }

    private void PlaySound(AudioClip clip)
    {
        audioSource.Stop();         // ������ ������� ���� ����
        audioSource.clip = clip;    // ���ο� ���� clip ��ü
        audioSource.Play();         // ���� ���
    }

    public void IncreaseMagazine(int magazine)
    {
        weaponSetting.currentMagazine = CurrentMagazine + magazine > MaxMagazine ? MaxMagazine : CurrentMagazine + magazine;
        onMagazineEvent.Invoke(CurrentMagazine);
    }
}
