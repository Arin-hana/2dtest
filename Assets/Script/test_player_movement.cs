using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class test_player_movement : MonoBehaviour
{
    
    public float speed = 5f;
    private Vector2 move;
    public Transform facing_point;
    public Rigidbody2D hitbox;
    private Animator anim;
    
    private void Awake(){
        anim = GetComponent<Animator>();
    }

    void Start(){
        facing_point.parent = null;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        /*
            secara basic kita membuat point untuk player berjalan, daripada menggerakan langsung player

            cepat lambat nya player bergerak tergantung dari speed yang ditentukan diatas

            killed path point
        */
        hitbox.MovePosition(hitbox.position + speed * Time.fixedDeltaTime * move);

        move.x = Input.GetAxisRaw("Horizontal");
        move.y = Input.GetAxisRaw("Vertical");
        
       (int x, int y) = (Mathf.CeilToInt(Mathf.Abs(move.x)), Mathf.CeilToInt(Mathf.Abs(move.y)));
        // horizontal = -X atau +x, you learned coordinate system on junior high, or surely you played minecraft
        // ubah -1 ke 1 tapi float lalu ubah ke int
        
        if(x !=0 ^ y !=0){
            anim.SetFloat("facingX", move.x);
            anim.SetFloat("facingY", move.y);
        }

        switch ((x,y))
            {
            // menggunakan switch case karena lebih gampang
            // and also because you cant if(a ^ a !=0)
            // wait im an idiot XOR AUTOMATICALLY SET IT TO FALSE, bloody hell
            // i blame mizu5
            // nevermind its faster and easier to use swich case
            //we be nyoomin diagonal
                
                case (1,0):
                        facing_point.position = transform.position + new Vector3(move.x, 0f, 0f);
                        //facing_point biar mudah interact

                        anim.SetBool("nyoom", true);
                    break;
                
                case (0,1):
                        facing_point.position = transform.position + new Vector3(0f, move.y, 0f);
                        anim.SetBool("nyoom", true);
                    break;
                
                case (1,1):
                        facing_point.position = transform.position + new Vector3(move.x, move.y, 0f);
                        anim.SetBool("nyoom", true);
                    break;
                
                default:
                    anim.SetBool("nyoom", false);
                    break;
            }
    }
}
