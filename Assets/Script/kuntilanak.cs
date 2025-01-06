using System.Collections;
using System.Diagnostics;
using Unity.Collections;

// using System.Numerics;
using UnityEngine;

public class Kuntilanak : MonoBehaviour
{
    public GameObject enemy;
    public GameObject player;
    public float spd = 60f;
    [SerializeField] private Animator animationController;
    private UnityEngine.Vector2 targetPosition;
    private UnityEngine.Vector2 defaultPosition;
    private UnityEngine.Vector2 _desirePosition;
    private UnityEngine.Vector2 _currentPosition;
    [SerializeField] private AudioSource audio;
    [SerializeField] private AudioClip jumpScare;

    private void OnEnable()
    {
        targetPosition = player.transform.position;
        defaultPosition=enemy.transform.position;
        _currentPosition=enemy.transform.position;
        _desirePosition= targetPosition;
        if ( _currentPosition.x>targetPosition.x )
        {
            _desirePosition.x = _desirePosition.x-5;
            StartCoroutine(MoveTowardsTarget());
        }else if ( _currentPosition.x<targetPosition.x )
        {
            _desirePosition.x = _desirePosition.x+5;
            StartCoroutine(MoveTowardsTarget());
        }else if ( _currentPosition.y>targetPosition.y )
        {
            _desirePosition.y = _desirePosition.y-5;
            StartCoroutine(MoveTowardsTarget());
        }else if ( _currentPosition.y<targetPosition.y )
        {
            _desirePosition.y = _desirePosition.y+5;
            StartCoroutine(MoveTowardsTarget());
        }
    }

    private System.Collections.IEnumerator MoveTowardsTarget()
    {
        yield return new WaitForSeconds(1);
        audio.PlayOneShot(jumpScare);
        while (UnityEngine.Vector2.Distance(enemy.transform.position, _desirePosition) > 0.1f)
        {
            enemy.transform.position = UnityEngine.Vector2.MoveTowards(
                enemy.transform.position,
                _desirePosition,
                spd * Time.fixedDeltaTime
            );

            yield return null;
        }
        animationController.SetBool("done",true);
        yield return new WaitForSeconds(2);
        enemy.transform.position=defaultPosition;
        enemy.SetActive(false);
    }
}
