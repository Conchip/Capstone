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
            // �̱��� ������ ������Ʈ�� �Ҵ���� ������ 
            if(m_instance==null)
            {
                // ������ GameManager ������Ʈ�� ã�Ƽ� �Ҵ�
                m_instance = FindObjectOfType<GameManager>();
            }
            // �̱��� ������Ʈ ��ȯ
            return m_instance;
        }
    }
    private static GameManager m_instance;  // �̱��� �Ҵ�� static����
    public bool isGameOver { get; private set; }   // ���� ���� ����
    public bool isGameClear { get; private set; }  // ���� Ŭ���� ����

    private void Awake()
    {
        // ���� �̱��� ������Ʈ�� �� �ٸ� GameManager ������Ʈ�� �����ϸ� �ڽ��� �ı�
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
        // ���� Ŭ���� UI Ȱ��ȭ
        UIcontroller.instance.SetActiveGameClearUI(true);
    }
    public void GameOver()
    {
        // ���� ���� UI Ȱ��ȭ
        isGameOver = true;
        UIcontroller.instance.SetActiveGameOverUI(true);
    }
}
