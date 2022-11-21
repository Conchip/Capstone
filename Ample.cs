using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Ample : MonoBehaviour
{
    public event Action AmpleGetEvent = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.GameClear();
            this.gameObject.SetActive(false);
        }
    }
}
