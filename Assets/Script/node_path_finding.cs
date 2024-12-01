using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEngine;

public class node_path_finding : MonoBehaviour
{
    private GameObject[] nodes;
    private GameObject self;
    private Vector2[] directions = { new(0,1), new(0,-1), new(1,0), new(-1,0) };
    private float checkRadius = 0.2f; // Size of the detection area
    private float checkDistance = 0.4f; // Distance to check for walls
    private float speed = 3f; // Movement speed
    private LayerMask wall; // LayerMask to identify walls
    private Vector2 move;
    private Vector2 direction;
    private Rigidbody2D hitbox;
    private Queue<Vector2> next_steps;
    private GameObject left_behind;
    private bool walking = false;
    private bool thinking = false;

    private void Start()
    {
        self = this.gameObject;
        hitbox = GetComponent<Rigidbody2D>();
        wall = LayerMask.GetMask("the fucking wall");   
        nodes = GameObject.FindGameObjectsWithTag("node");
        next_steps = new Queue<Vector2>();
        Path();
    }

    private void Path(){
        if (nodes.Length == 0){
            return;
        }

        // compare self position to node
        // find the closest node
        var closestNode = nodes
            .Where(node => node != left_behind)
            .OrderBy(node => Vector2.Distance(node.transform.position, self.transform.position))
            .FirstOrDefault();
        
        if (closestNode == null)
            return;

        // compare nearest node to others
        var road_to_walk = nodes
            .Where(node => node != left_behind)
            .OrderBy(node => Vector2.Distance(node.transform.position, closestNode.transform.position))
            .Take(5)
            .Select(node => (Vector2)node.transform.position)
            .Where(where_node => !Trapped_behind((Vector2)self.transform.position, where_node));
    

        Debug.DrawLine(self.transform.position, closestNode.transform.position, Color.red);
        next_steps = new Queue<Vector2>(road_to_walk);
        //mizuki....
    }

    private void FixedUpdate(){
        if(!walking && next_steps.Count > 0){
            StartCoroutine(Path_of_thorns());
        }
        else if(!walking && next_steps.Count == 0){
            StartCoroutine(Thinking_about_life_choices());
        }
    }
    //mizuki...
    private IEnumerator Path_of_thorns(){
         walking = true;

        while (next_steps.Count > 0){
            Vector2 target = next_steps.Dequeue();

            // Move towards the target position
            while (Vector2.Distance(self.transform.position, target) > 0.5f){
                // if (Theres_a_Wall(direction)){
                //     direction = New_direction();
                // }
                // else{
                    direction = (target - (Vector2)self.transform.position).normalized;
                // }

                hitbox.MovePosition(hitbox.position + direction * speed * Time.fixedDeltaTime);

                Debug.DrawLine(self.transform.position, target, Color.red);

                yield return new WaitForFixedUpdate();
            }
            left_behind = nodes.FirstOrDefault(node => Vector2.Distance(node.transform.position, target) < 0.1f);
        }

        walking = false;
    }

    private IEnumerator Thinking_about_life_choices(){
        thinking = true;
        yield return new WaitForSecondsRealtime(Random.Range(2f,4f));
        thinking = false;
        Path();
    }

    private bool Trapped_behind(Vector2 start, Vector2 end){
        Vector2 direction = (end - start).normalized;
        float distance = Vector2.Distance(start, end);
        
        Debug.DrawLine(start, end, Color.red);

        // i cast magic line to find nodes
        RaycastHit2D hit = Physics2D.Raycast(start, direction, distance, wall);

        return hit.collider != null; 
        //dang theres a wall
    }

    private bool Theres_a_Wall(Vector2 direction){
        // Calculate the position to check
        Vector2 checkPosition = (Vector2)transform.position + direction * checkDistance;

        // Check for a wall using OverlapCircle
        Collider2D collider = Physics2D.OverlapCircle(checkPosition, checkRadius, wall);
        return collider != null; // Return true if there's a wall
    }
    private Vector2 New_direction(){
        // Try a random direction until a clear path is found
        for (int i = 0; i < directions.Length; i++){
            Vector2 newDirection = directions[Random.Range(0, directions.Length)];
            if (!Theres_a_Wall(newDirection)){
                return newDirection;
            }
        }

        // If no clear direction is found, stop moving (rare fallback case)
        return Vector2.zero;
    }

    private void OnDrawGizmos(){
        Gizmos.color = Color.red;
        if (nodes != null){
            foreach (GameObject node in nodes){
                Gizmos.DrawWireSphere(node.transform.position, 0.2f);
            }
        }

        foreach (Vector2 direction in directions){
            Vector2 checkPosition = (Vector2)transform.position + direction * checkDistance;
            Gizmos.DrawWireSphere(checkPosition, checkRadius);
        }

        if (next_steps != null && next_steps.Count > 0){
            Gizmos.color = Color.green;
            foreach (Vector2 position in next_steps){
                Gizmos.DrawWireSphere(position, 0.3f);
            }
        }
    }
}
