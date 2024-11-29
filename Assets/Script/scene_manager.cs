using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class scene_manager : MonoBehaviour
{
    public LayerMask wall;
    public LayerMask trigger_area;
    /*
        load scene berikutnya
        cari trigger
        cari lokasi yang bukan dinding atau area trigger
        teleport player ke lokasi tersebut
    */
    public void Geto_sinuAidi_to_turiga(int sceneId, string triggerName){
        StartCoroutine(How_the_portal_gun_work(sceneId, triggerName));
    }

    private IEnumerator How_the_portal_gun_work(int sceneId, string triggerName){
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneId, LoadSceneMode.Single);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone){
            if (asyncLoad.progress >= 0.9f){
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }

        yield return null;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null){
            Debug.LogWarning("Player not found in the scene.");
            yield break;
        }

        GameObject trigger = GameObject.Find(triggerName);
        if (trigger == null){
            Debug.LogWarning("Trigger not found.");
            yield break;
        }

        Vector2 triggerPos = trigger.transform.position;
        Vector2[] directions = new Vector2[]{
            triggerPos + Vector2.right,
            triggerPos + Vector2.left,
            triggerPos + Vector2.up,
            triggerPos + Vector2.down
        };

        foreach (Vector2 position in directions){
            Collider2D collider = Physics2D.OverlapPoint(position, wall);
            if (collider == null){
                Collider2D tri_area = Physics2D.OverlapPoint(position, trigger_area);
                if(tri_area == null){
                    player.transform.position = position;
                    break;
                }
            }
        }
    }
}
