using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//REDUNDANT

public class path_point : MonoBehaviour
{   
    public float speed = 5f;
    private Vector2 move;
    public Transform player;
    public Rigidbody2D hitbox;
    private bool zoomin;
    
    // Update is called once per frame
    void FixedUpdate()
    {
        /*
            secara basic kita membuat point untuk player berjalan, daripada menggerakan langsung player

            cepat lambat nya player bergerak tergantung dari speed yang ditentukan diatas
        */
        
        move.x = Input.GetAxisRaw("Horizontal");
        move.y = Input.GetAxisRaw("Vertical");

        // transform.position = Vector3.MoveTowards(transform.position, path_point.position, speed*Time.deltaTime);

        if(Vector3.Distance( transform.position, player.position) <= .005f){
            hitbox.MovePosition(hitbox.position + speed * Time.fixedDeltaTime * move);
        }
        // cek jarak antara player dan point

            // if(Mathf.Abs(move.x) == 1f){
            // 


            //     if(!Physics2D.OverlapCircle(point.position + new Vector3(move.x, 0f, 0f), 0.2f, collision)){
            //     // cek collision sebelum menggerakan player ke point

            //         anim.SetFloat("facingX", move.x);
            //         anim.SetFloat("facingY", move.y);
            //         point.position += new Vector3(move.x, 0f, 0f);
            //         anim.SetBool("nyoom", true);
            //     }
            // }
            // else if(Mathf.Abs(move.y) == 1f){
            // // menggunakan else if untuk mencegah gerakan diagonal

            //     if(!Physics2D.OverlapCircle(point.position + new Vector3(0f, move.y, 0f), 0.2f, collision)){    
                    
            //         anim.SetFloat("facingX", move.x);
            //         anim.SetFloat("facingY", move.y);
            //         point.position += new Vector3(0f, move.y, 0f);
            //         anim.SetBool("nyoom", true);
            //     }
            // }
            // else{
            //     anim.SetBool("nyoom", false);
            // }

        

    }
}
