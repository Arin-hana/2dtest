using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Camcoder : MonoBehaviour
{
    private GameObject entity;
    private Collider2D camera_wall;
    private float halfHeight;
    private float halfWidth;
    private Camera cimera;

    // private static Camcoder instance;

    // private void Awake()
    // {
    //     if (instance == null)
    //     {
    //         instance = this;
    //         DontDestroyOnLoad(gameObject);
    //     }
    //     else
    //     {
    //         Destroy(gameObject);
    //     }
    // }

    void Start(){
        entity = GameObject.FindGameObjectWithTag("Player");
        Transform creature = entity.transform;

        camera_wall = GameObject.FindGameObjectWithTag("wall for camera").GetComponent<TilemapCollider2D>();

        cimera = Camera.main;
        halfHeight = cimera.orthographicSize;
        halfWidth = cimera.aspect * halfHeight;
        transform.position = new Vector3(creature.position.x, creature.position.y, transform.position.z);
    }
    void LateUpdate()
    {
        
        entity = GameObject.FindGameObjectWithTag("Player");
        Transform creature = entity.transform;

        
        camera_wall = GameObject.FindGameObjectWithTag("wall for camera").GetComponent<TilemapCollider2D>();

        if (camera_wall == null){
            return;
        }

        float clampedX = Mathf.Clamp(creature.position.x, 
            camera_wall.bounds.min.x + halfWidth, 
            camera_wall.bounds.max.x - halfWidth);
        float clampedY = Mathf.Clamp(creature.position.y, 
            camera_wall.bounds.min.y + halfHeight, 
            camera_wall.bounds.max.y - halfHeight);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }
}
