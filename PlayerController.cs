using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Input KeyCodes")]
    [SerializeField]
    private KeyCode keyCodeRun = KeyCode.LeftShift; // �޸��� Ű
    [SerializeField]
    private KeyCode keyCodeJump = KeyCode.Space;    // ���� Ű
    [SerializeField]
    private KeyCode keyCodeReload = KeyCode.R;   // ź ������ Ű

    [Header("Audio Clips")]
    [SerializeField]
    private AudioClip audioClipWalk;    // ���� �� ����
    [SerializeField]
    private AudioClip audioClipRun; // �޸� �� ����
    [SerializeField]
    private AudioClip audioClipHit; // ���� ������ �� ����

    private RotateMouse rotateToMouse; // ���콺 �̵����� ī�޶� ȸ��
    private MovementCharacterController movement; // Ű���� �Է����� �÷��̾� �̵�, ����
    private Status status;  // �÷��̾��� ����
    private PlayerAnimatorController animator;  // �ִϸ��̼� ��� ����
    private AudioSource audioSource;    // ���� ��� ����
    private WeaponAR weapon;    // ���⸦ �̿��� ���� ����
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
        // �̵��� (�ȱ� ���� / �ٱ� ����)
        if(x != 0 || z != 0)
        {
            bool isRun = false;
            // ���̳� �ڷ� �̵��߿��� �޸��� �Ұ���
            if (z > 0) isRun = Input.GetKey(keyCodeRun);
            
            movement.MoveSpeed = isRun == true ? status.RunSpeed : status.WalkSpeed;
            animator.MoveSpeed = isRun == true ? 1 : 0.5f;
            audioSource.clip = isRun == true ? audioClipRun : audioClipWalk;

            // ����Ű �Է� ���δ� �� ������ Ȯ��
            // ������� ���� �ٽ� ��� ���� ���ϵ��� isPlaying���� üũ�ؼ� ���
            if (audioSource.isPlaying == false)
            {
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        // ���ڸ��� ��������
        else
        {
            movement.MoveSpeed = 0;
            animator.MoveSpeed = 0;

            // ������ �� ���尡 ��� ���̸� ����
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
