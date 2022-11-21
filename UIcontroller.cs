using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UIcontroller : MonoBehaviour
{
    public static UIcontroller instance
    {
        get
        {
            if(m_instance == null)
            {
                m_instance = FindObjectOfType<UIcontroller>();
            }
            return m_instance;
        }
    }
    private static UIcontroller m_instance;

    [Header("UI")]
    [SerializeField]
    public GameObject PauseUI;
    [SerializeField]
    public GameObject ClearUI;
    [SerializeField]
    public GameObject OverUI;

    [Header("Input Key")]
    [SerializeField]
    private KeyCode keyCodeMenu = KeyCode.Escape;   // ���� �޴� Ű
    [SerializeField]
    private KeyCode keyCodeRestart = KeyCode.R;    // ����� Ű
    [SerializeField]
    private KeyCode keyCodeQuit = KeyCode.Q;   // ���� ���� Ű

    private bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        PauseUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(keyCodeMenu))
        {
            paused = !paused;
        }
        if(paused)
        {
            PauseUI.SetActive(true);
            Time.timeScale = 0;
        }
        if(!paused)
        {
            PauseUI.SetActive(false);
            Time.timeScale = 1f;
        }
        if(Input.GetKeyDown(keyCodeRestart))
        {
            Restart();
        }
        else if(Input.GetKeyDown(keyCodeQuit))
        {
            Quit();
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // ����� �� ����� GameManger�� �̱��� ������Ʈ�� ������ ������ ������Ʈ �ı�
    }
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // ���� ����
# endif
    }
    public void SetActiveGameOverUI(bool active)
    {
        Time.timeScale = 0; // �ð��� �����
        OverUI.SetActive(active); // ���� ���� UI Ȱ��ȭ
    }
    public void SetActiveGameClearUI(bool active)
    {
        Time.timeScale = 0; // �ð��� �����
        ClearUI.SetActive(active); // ���� Ŭ���� UI Ȱ��ȭ
    }
}
