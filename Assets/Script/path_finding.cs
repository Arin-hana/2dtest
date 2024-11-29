using UnityEngine;
//this shit suck ass
//imma make node based path finding
public class PathFinding : MonoBehaviour
{
    private LayerMask wallLayer; // LayerMask to identify walls
    private float checkRadius = 0.2f; // Size of the detection area
    private float checkDistance = 0.4f; // Distance to check for walls
    public float speed = 3f; // Movement speed

    private Rigidbody2D hitbox;
    private Vector2[] directions = { new(0,1), new(0,-1), new(1,0), new(-1,0), new(1,1), new(-1,-1), new(1,-1), new(-1,1) };
    private Vector2 currentDirection;
    private float i = 0f;

    private void Start(){
        // Cache the Rigidbody2D component
        hitbox = GetComponent<Rigidbody2D>();
        wallLayer = LayerMask.GetMask("the fucking wall");    
        currentDirection = directions[Random.Range(0, directions.Length)];
        i = Random.Range(1f, 2f);
        //random range in seconds to change direction
    }

    private void FixedUpdate(){
        //DO NOT USE WHILE IN UPDATE NOR FIXEDUPDATE, IT WILL FREEZE THE EDITOR
        //fucking fuck this shit takes long as fuck to figure out
        i -= Time.fixedDeltaTime;

        if (TheresAWall(currentDirection)){
            currentDirection = PickNewDirection();
        }
        else{
            // i--;
            hitbox.MovePosition(hitbox.position + currentDirection * speed * Time.fixedDeltaTime);
            
            // Debug.Log(i.ToString());
            if(i <= 0.2f){
                Debug.LogWarning("i = 0");
                currentDirection = PickNewDirection();
                hitbox.MovePosition(hitbox.position + currentDirection * speed * Time.fixedDeltaTime);
                i = Random.Range(1f, 2f);
                //dont forget to change this range too
            }
        }
    }


    private bool TheresAWall(Vector2 direction){
        // Calculate the position to check
        Vector2 checkPosition = (Vector2)transform.position + direction * checkDistance;

        // Check for a wall using OverlapCircle
        Collider2D collider = Physics2D.OverlapCircle(checkPosition, checkRadius, wallLayer);
        return collider != null; // Return true if there's a wall
    }

    private Vector2 PickNewDirection(){
        // Try a random direction until a clear path is found
        for (int i = 0; i < directions.Length; i++){
            Vector2 newDirection = directions[Random.Range(0, directions.Length)];
            if (!TheresAWall(newDirection)){
                return newDirection;
            }
        }

        // If no clear direction is found, stop moving (rare fallback case)
        return Vector2.zero;
    }


    private void OnDrawGizmos(){
        // Visualize the detection area in the Scene view
        Gizmos.color = Color.red;
        foreach (Vector2 direction in directions){
            Vector2 checkPosition = (Vector2)transform.position + direction * checkDistance;
            Gizmos.DrawWireSphere(checkPosition, checkRadius);
        }
    }
}
