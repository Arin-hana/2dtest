using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//WARNING it has the tendency of camping and absolutely bricking it self
//please help
//fix needed

/*
=========================================================

NODE PATH FINDING, by me ArinHana

in essence it create list of gameobjects, then use their position for transform to walk to
it does this by using raycast to find gameobjects
its fine if transform cant get to end_point because itll just make a new list

*/

public class node_path_finding : MonoBehaviour
{
    private GameObject[] points;
    private GameObject wall;
    private GameObject cameral_wall; //somehow i didnt think itll hit it
    private Transform self;
    private GameObject end_point;
    private float speed = 50f;
    bool walking_now = false;
    List<GameObject> items;
    private void Start(){
        points = GameObject.FindGameObjectsWithTag("node");
        //get any gameobject that have the tag node
        if(points.Length == 0){
            Debug.LogWarning("no nodes found, please put 'node' tag on nodes gameobject");
            return;
        }

        self = this.transform;
        items = new List<GameObject>();
        wall = GameObject.FindGameObjectWithTag("wall normal");
        cameral_wall = GameObject.FindGameObjectWithTag("wall for camera");
        speed *= Time.fixedDeltaTime;
        //change the speed from the private float
    }

    private void FixedUpdate(){
        if(!walking_now){
            StartCoroutine(Walking());
        }
    }

    private IEnumerator Walking(){
        walking_now = true;

        end_point = points[Random.Range(0,points.Length)];
        //pick random node

        end_point.GetComponent<Renderer>().material.color = new Color(0,0,0);
        //DEBUG color the node to bunny girl (black)
        // Debug.LogWarning(end_point.name);
        
        List<GameObject> steps = approximate_end_point_direction();
        foreach(GameObject step in steps){
            //go to each gameobject position
            // Debug.Log(step.name);

            while(Vector2.Distance(self.position, step.transform.position) > 0.2f){
                self.position = Vector2.MoveTowards(self.position, step.transform.position, Time.fixedDeltaTime * speed); 
                yield return null;
            } 
        } 
            
        end_point.GetComponent<Renderer>().material.color = new Color(255,255,255);
        //DEBUG reset color after `self` reach `end_point`

        walking_now = false;  
    }

    private List<GameObject> approximate_end_point_direction(){
        GameObject current = self.gameObject;
        GameObject end = end_point;
        RaycastHit2D hit;
        
        Vector2 vec2_true_direction = (end.transform.position - current.transform.position).normalized;
        //the direction from end to current

        Vector2 rounded = new Vector2(Mathf.Round(vec2_true_direction.x), Mathf.Round(vec2_true_direction.y));
        //rounded direction for 8 way move

        Vector2[] rounded_split = new Vector2[]{
            new(rounded.x, 0),
            new(0, rounded.y)
        };
        //if theres a wall in rounded

        Vector2[] directions = new Vector2[]{
            new Vector2(1,0),
            new Vector2(1,1),
            new Vector2(0,1),
            new Vector2(-1,1),
            new Vector2(-1,0),
            new Vector2(-1,-1),
            new Vector2(0,-1),
            new Vector2(1,-1)
        };
        //if theres a wall in split

        items.Clear();
        items.Add(current);
        if(end_point != null || self != null){

            while(!GameObject.ReferenceEquals(current, end)){
                //using == doesnt work
                //RefrenceEquals, as the name suggest, does a is the same as b

                // Debug.LogWarning("current "+current.name);
                // Debug.Log("true direction "+vec2_true_direction);
                // Debug.Log("rounded "+rounded);

                hit = Physics2D.Raycast(current.transform.position, rounded);

                if(GameObject.ReferenceEquals(hit.collider.gameObject, end)){
                    current = hit.collider.gameObject;
                    // Debug.LogWarning("refrence==end "+current);

                    vec2_true_direction = (end.transform.position - current.transform.position).normalized;

                    rounded = new Vector2(Mathf.Round(vec2_true_direction.x), Mathf.Round(vec2_true_direction.y));
                    
                    rounded_split = new Vector2[]{
                        new(rounded.x, 0),
                        new(0, rounded.y)
                    };
                    //too lazy

                    items.Add(current);
                    break;
                }
                if(!GameObject.ReferenceEquals(hit.collider.gameObject, wall) || GameObject.ReferenceEquals(hit.collider.gameObject, cameral_wall)){
                    current = hit.collider.gameObject;
                    // Debug.LogWarning("refrence!=wall "+current);

                    
                    vec2_true_direction = (end.transform.position - current.transform.position).normalized;

                    rounded = new Vector2(Mathf.Round(vec2_true_direction.x), Mathf.Round(vec2_true_direction.y));

                    rounded_split = new Vector2[]{
                        new(rounded.x, 0),
                        new(0, rounded.y)
                    };

                    items.Add(current);
                }
                if(GameObject.ReferenceEquals(hit.collider.gameObject, wall) || GameObject.ReferenceEquals(hit.collider.gameObject, cameral_wall)){
                    foreach(var splt in rounded_split){
                        if(splt == Vector2.zero)continue;

                        hit = Physics2D.Raycast(current.transform.position, splt);
                        
                        if(GameObject.ReferenceEquals(hit.collider.gameObject, wall) || GameObject.ReferenceEquals(hit.collider.gameObject, cameral_wall)) continue;

                        if(!GameObject.ReferenceEquals(hit.collider.gameObject, wall) || GameObject.ReferenceEquals(hit.collider.gameObject, cameral_wall)){
                            current = hit.collider.gameObject;
                            // Debug.Log("split dir "+splt);
                            // Debug.LogWarning("refrence==wall split "+current);

                            
                            vec2_true_direction = (end.transform.position - current.transform.position).normalized;

                            rounded = new Vector2(Mathf.Round(vec2_true_direction.x), Mathf.Round(vec2_true_direction.y));

                            rounded_split = new Vector2[]{
                                new(rounded.x, 0),
                                new(0, rounded.y)
                            };

                            items.Add(current);
                            break;
                        }

                    }

                    
                    foreach(var dir in directions){
                        if(dir == Vector2.zero)continue;

                        hit = Physics2D.Raycast(current.transform.position, dir);

                        if(GameObject.ReferenceEquals(hit.collider.gameObject, wall) || GameObject.ReferenceEquals(hit.collider.gameObject, cameral_wall)) continue;
                        
                        if(items.Contains(hit.collider.gameObject)) continue;

                        if(!GameObject.ReferenceEquals(hit.collider.gameObject, wall) || GameObject.ReferenceEquals(hit.collider.gameObject, cameral_wall)){
                            current = hit.collider.gameObject;
                            // Debug.Log("dir "+dir);
                            // Debug.LogWarning("refrence==wall dir "+current);

                            
                            vec2_true_direction = (end.transform.position - current.transform.position).normalized;

                            rounded = new Vector2(Mathf.Round(vec2_true_direction.x), Mathf.Round(vec2_true_direction.y));

                            rounded_split = new Vector2[]{
                                new(rounded.x, 0),
                                new(0, rounded.y)
                            };

                            items.Add(current);
                            break;
                        }

                    }
                    // continue;
                    
                }
                else{
                    Debug.LogWarning("cant find end_point");
                    break;
                }

            }

        }
        return items;
    }

}