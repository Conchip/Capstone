using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Input KeyCodes")]
    [SerializeField]
    private KeyCode keyCodeRun = KeyCode.LeftShift; // 달리기 키
    [SerializeField]
    private KeyCode keyCodeJump = KeyCode.Space;    // 점프 키
    [SerializeField]
    private KeyCode keyCodeReload = KeyCode.R;   // 탄 재장전 키

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip audioClipWalk;    // 걷을 때 사운드
    [SerializeField]
    private AudioClip audioClipRun; // 달릴 때 사운드
    [SerializeField]
    private AudioClip audioClipHit; // 공격 당했을 때 사운드

    private RotateMouse rotateToMouse; // 마우스 이동으로 카메라 회전
    private MovementCharacterController movement; // 키보드 입력으로 플레이어 이동, 점프
    private Status status;  // 플레이어의 정보
    private PlayerAnimatorController animator;  // 애니메이션 재생 제어
    private AudioSource audioSource;    // 사운드 재생 제어
    private WeaponAR weapon;    // 무기를 이용한 공격 제어
    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        rotateToMouse = GetComponent<RotateMouse>();
        movement = GetComponent<MovementCharacterController>();
        status = GetComponent<Status>();
        animator = GetComponent<PlayerAnimatorController>();
        audioSource = GetComponent<AudioSource>();
        weapon = GetComponentInChildren<WeaponAR>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRotate();
        UpdateMove();
        UpdateJump();
        UpdateWeaponAction();
    }

    private void UpdateRotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotateToMouse.UpdateRotate(mouseX, mouseY);
    }
    private void UpdateMove()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        // 이동중 (걷기 상태 / 뛰기 상태)
        if(x != 0 || z != 0)
        {
            bool isRun = false;
            // 옆이나 뒤로 이동중에는 달리기 불가능
            if (z > 0) isRun = Input.GetKey(keyCodeRun);
            
            movement.MoveSpeed = isRun == true ? status.RunSpeed : status.WalkSpeed;
            animator.MoveSpeed = isRun == true ? 1 : 0.5f;
            audioSource.clip = isRun == true ? audioClipRun : audioClipWalk;

            // 방향키 입력 여부는 매 프레임 확인
            // 재생중일 때는 다시 재생 하지 못하도록 isPlaying으로 체크해서 재생
            if (audioSource.isPlaying == false)
            {
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        // 제자리에 멈췄을때
        else
        {
            movement.MoveSpeed = 0;
            animator.MoveSpeed = 0;

            // 멈췄을 때 사운드가 재생 중이면 정지
            if(audioSource.isPlaying == true)
            {
                audioSource.Stop();
            }
        }
        movement.MoveTo(new Vector3(x, 0, z));
    }
    private void UpdateJump()
    {
        if (Input.GetKeyDown(keyCodeJump))
        {
            movement.Jump();
        }
    }
    private void UpdateWeaponAction()
    {
        if(Input.GetMouseButtonDown(0))
        {
            weapon.StartWeaponAction();
        }
        else if(Input.GetMouseButtonUp(0))
        {
            weapon.StopWeaponAction();
        }

        if(Input.GetMouseButtonDown(1))
        {
            weapon.StartWeaponAction(1);
        }
        else if(Input.GetMouseButtonUp(1))
        {
            weapon.StopWeaponAction(1);
        }

        if(Input.GetKeyDown(keyCodeReload))
        {
            weapon.StartReload();
        }
    }

    public void TakeDamge(int damage)
    {
        bool isDie = status.DecreaseHP(damage);
        if(isDie == false)
        {
            audioSource.PlayOneShot(audioClipHit);
        }
        if(isDie == true)
        {
            GameManager.instance.GameOver();
        }
    }
}
