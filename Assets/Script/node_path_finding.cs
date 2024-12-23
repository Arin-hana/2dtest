using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class node_path_finding : MonoBehaviour
{
    private GameObject[] points;
    private Transform self;
    private Transform end_of_path;
    private Transform first_step;
    private float speed = 50f;
    bool pushing_trough = false;
    private void Start(){
        points = GameObject.FindGameObjectsWithTag("node");
        //get any gameobject that have the tag node
        if(points.Length == 0){
            Debug.LogError("no nodes found");
            return;
        }

        self = this.transform;
        speed *= Time.fixedDeltaTime;
    }

    private void FixedUpdate(){
        if(!pushing_trough == true){
            StartCoroutine(path_of_thorns());
        }
    }

    private IEnumerator path_of_thorns(){
        //setup path

        pushing_trough = true;

        end_of_path = points[Random.Range(0, points.Length)].transform;
        //final position of the transform
        end_of_path.gameObject.GetComponent<Renderer>().material.color = new Color(0,0,0);
        if(points.Length == 0) yield break;

        first_step = little_things_called_hobby(self.position);
        //start position
        first_step.gameObject.GetComponent<Renderer>().material.color = new Color(255,0,0);
        if(first_step == null){
            Debug.LogError("no nodes that is reachable");
            pushing_trough = false;
            yield break;
        }

        List<Transform> once_in_a_dream = how_will_the_story_goes(first_step, end_of_path);
        //find nodes in between start and end

        foreach(Transform vision in once_in_a_dream){
            while(Vector2.Distance(self.position, vision.position) > 0.2f){
                self.position = Vector2.MoveTowards(self.position, vision.position, Time.fixedDeltaTime * speed);
                yield return null;
                //move to each node
            }
            vision.gameObject.GetComponent<Renderer>().material.color = new Color(255,255,255);
        } 
        end_of_path.gameObject.GetComponent<Renderer>().material.color = new Color(255,255,255);
        first_step.gameObject.GetComponent<Renderer>().material.color = new Color(255,255,255);
        pushing_trough = false;
    }

    private Transform little_things_called_hobby(Vector2 something_dear){
        //find the nearest node from self

        Transform reachable = null;
        float near = float.MaxValue;

        foreach(GameObject point in points){

            float distance = Vector2.Distance(something_dear, point.transform.position);

            if(uncertain_future(something_dear, point.transform.position)) continue;
            //ignore if behind wall

            if(distance < near){
                near = distance;
                reachable = point.transform;
            }
        }

        return reachable;
    }

    private List<Transform> how_will_the_story_goes(Transform first_page, Transform ending){
        //find nodes in between start and end

        List<Transform> pages = new List<Transform>();
        Transform current_page = first_page;

        while(current_page != ending){
            pages.Add(current_page);
            Transform future = unwavering_steps(current_page.position, pages);
            //find nodes that isnt already in list

            if(future == null){
                Debug.LogWarning("i should just give up");
                break;
            }
            current_page = future;
            current_page.gameObject.GetComponent<Renderer>().material.color = new Color(0,255,0);
        }

        pages.Add(ending);
        return pages;

    }

    private Transform unwavering_steps(Vector2 fear_of_change, List<Transform> stuck_in_a_loop){
        //find nodes that isnt in the list from function above

        Transform reachable = null;
        float near = float.MaxValue;

        foreach(GameObject point in points){
            Transform tomorrow = point.transform;

            if(stuck_in_a_loop.Contains(tomorrow)) continue;
            //ignore if already in list
            
            if(uncertain_future(fear_of_change, tomorrow.position)) continue;
            //ignore if behind wall

            float distance = Vector2.Distance(fear_of_change, tomorrow.position);


            if(distance < near){
                near = distance;
                reachable = point.transform;
            }
        }
        return reachable;
    }

    private bool uncertain_future(Vector2 past, Vector2 present){
        //does inbetween node a and node b, a wall?

        RaycastHit2D hit = Physics2D.Linecast(past, present);
        if(hit){
            // Debug.Log(hit.collider.gameObject.name);
            if(hit.collider.gameObject.name == "wall"){
                return true;
            }
        }
        return false;
    }
}
