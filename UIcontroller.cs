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
    private KeyCode keyCodeMenu = KeyCode.Escape;   // 정지 메뉴 키
    [SerializeField]
    private KeyCode keyCodeRestart = KeyCode.R;    // 재시작 키
    [SerializeField]
    private KeyCode keyCodeQuit = KeyCode.Q;   // 게임 종료 키

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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 저장된 씬 재시작 GameManger로 싱글턴 오브젝트를 제어해 동일한 오브젝트 파괴
    }
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 게임 종료
# endif
    }
    public void SetActiveGameOverUI(bool active)
    {
        Time.timeScale = 0; // 시간을 멈춘뒤
        OverUI.SetActive(active); // 게임 오버 UI 활성화
    }
    public void SetActiveGameClearUI(bool active)
    {
        Time.timeScale = 0; // 시간을 멈춘뒤
        ClearUI.SetActive(active); // 게임 클리어 UI 활성화
    }
}
