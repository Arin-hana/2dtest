
using UnityEngine;

public class path_finding : MonoBehaviour
{
    private LayerMask wallLayer; // Layer to identify walls
    private float checkRadius = 0.2f; // Size of the detection area
    private float checkDistance = 1f; // Distance to check for walls
    private GameObject self;
    private Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
    public float speed = 0.001f;

    void FixedUpdate()
    {
        self = this.gameObject;
        Rigidbody2D hitbox = self.GetComponent<Rigidbody2D>();

        Vector2 walkdirecton = directions[Random.Range(0, directions.Length)];
        
        for(int i = 0; i <= Random.Range(1,10);i--){
            if(i != 1){
                if (!Theres_a_wall(walkdirecton))
                {    
                    hitbox.MovePosition(hitbox.position + speed * Time.fixedDeltaTime * walkdirecton);  
                }
                else{
                    walkdirecton = new Vector2(0,0);
                }
            }
        }
    }

    private bool Theres_a_wall(Vector2 direction)
    {
        wallLayer = LayerMask.NameToLayer("the fucking wall");
        Vector2 checkPosition = (Vector2)transform.position + direction * checkDistance;
        Collider2D collider = Physics2D.OverlapCircle(checkPosition, checkRadius, wallLayer);
        return collider != null; 
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
    //     foreach (Vector2 direction in directions)
    //     {
    //         Vector2 checkPosition = (Vector2)transform.position + direction * checkDistance;
    //         Gizmos.DrawWireSphere(checkPosition, checkRadius);
    //     }
    // }
    
}
