using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Exit : MonoBehaviour
{
    [SerializeField]
    private GameObject AmplePrefab;
    

    private Transform AmplePositions; 
    void Start()
    {
          GameObject Ample = Instantiate(AmplePrefab, AmplePositions.position, Quaternion.identity);
          Ample.GetComponent<Ample>().AmpleGetEvent += AmpleGetCallBack;
    }
    private void AmpleGetCallBack()
    {
        GameManager.instance.GameClear();
    }
}