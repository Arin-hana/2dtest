using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class kunti_trigger : MonoBehaviour
{
    [SerializeField] private GameObject triggered;
    public GameObject facingPoint;
    private UnityEngine.Vector2 facingPointPosition;
    private UnityEngine.Vector2 targetPosition;
    private UnityEngine.Vector2 _desirePosition;
    public GameObject player;
    private bool _los;
    private float _offset;

    private void Start() {
        _los=false;
        _offset = 5;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        facingPointPosition = facingPoint.transform.position;
        targetPosition = player.transform.position;

        if (other.tag == "Player" && !triggered.activeSelf)
        {
            if (facingPointPosition.x > targetPosition.x)
            {
                _desirePosition = new Vector2(targetPosition.x + _offset, targetPosition.y);
            } else if (facingPointPosition.x < targetPosition.x)
            {
                _desirePosition = new Vector2(targetPosition.x - _offset,targetPosition.y);
            }
            else if (facingPointPosition.y < targetPosition.y)
            {
                _desirePosition = new Vector2(targetPosition.x, targetPosition.y - _offset);
            }
            else if (facingPointPosition.y > targetPosition.y)
            {
                _desirePosition = new Vector2(targetPosition.x,targetPosition.y+_offset);
            }
            else
            {
                Debug.Log("AKU STRESS WAK!");
            }
            triggered.transform.position = _desirePosition;
            triggered.SetActive(true);
        }
    }
    private void Update() {

    }
}