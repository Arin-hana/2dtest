using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class trigger_random : MonoBehaviour
{
    private int rand = 0;
    public int chance = 1;
    [SerializeField] private GameObject triggered;
    private void OnTriggerEnter2D(Collider2D other) {
            if(other.tag=="Player"){
            rand = UnityEngine.Random.Range(1,100);
            if(rand<=chance){
                triggered.SetActive(true);
                chance = 1;
            }
            else
            {
                chance++;
                Debug.Log("Chance mu = "+chance+"%");
            }
        }
    }
}
