using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance
    {
        get
        {
            // 싱글턴 변수에 오브젝트가 할당되지 않으면 
            if(m_instance==null)
            {
                // 씬에서 GameManager 오브젝트를 찾아서 할당
                m_instance = FindObjectOfType<GameManager>();
            }
            // 싱글턴 오브젝트 반환
            return m_instance;
        }
    }
    private static GameManager m_instance;  // 싱글턴 할당될 static변수
    public bool isGameOver { get; private set; }   // 게임 오버 상태
    public bool isGameClear { get; private set; }  // 게임 클리어 상태

    private void Awake()
    {
        // 씬에 싱글턴 오브젝트가 된 다른 GameManager 오브젝트가 존재하면 자신을 파괴
        if(instance != this)
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        isGameClear = false;
        isGameOver = false;
    }
   
    public void GameClear()
    {
        isGameClear = true;
        Debug.Log("Game Clear");
        // 게임 클리어 UI 활성화
        UIcontroller.instance.SetActiveGameClearUI(true);
    }
    public void GameOver()
    {
        // 게임 오버 UI 활성화
        isGameOver = true;
        UIcontroller.instance.SetActiveGameOverUI(true);
    }
}
