using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Experimental.RestService;
using UnityEngine;

public class test_player_data : MonoBehaviour
{

    private static test_player_data instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            //agar player tidak dibuang
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
