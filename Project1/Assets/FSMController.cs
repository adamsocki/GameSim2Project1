using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMController : MonoBehaviour
{

    public GameObject player;
    public GameObject fog;

    public enum State
    {
        Idle,
        Walking,
        Attacking,
        AttackingWhileWalking,
        ActivelyMoving,
        KilledByFog,
        PushingWhileWalking,
        PushingWhileIdle
    }

    public enum FogState
    {
        Idle,
        Approaching
    }
    public State state;
    public FogState fogState;
    public bool playerAttacking;
    public bool playerAlive;
    public Vector2 playerSpeed;
    public bool playerInPushTrigger;
    public bool inAttackSpace;
    public bool levelTransitioning;

    // Start is called before the first frame update
    void Start()
    {
        state = State.Idle;
        fogState = FogState.Idle;
        playerAlive = true;
    }


    public void MovePlayer()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");
        Vector3 moveDir = new Vector3();

        switch (state)
        {
            case State.Idle:
            {
                // play idle animation
                player.GetComponent<Animator>().SetBool("isWalking", false);
                player.GetComponent<Animator>().SetBool("isPushing", false);
                // state to walking
                if (moveX != 0 || moveY != 0)
                {
                    if (!levelTransitioning)
                    {
                        state = State.Walking;
                    }
                }
                if (inAttackSpace && Input.GetKey("space"))
                {
                    state = State.Attacking;
                    player.GetComponent<Animator>().SetBool("isAttacking", true);
                }
                else
                {
                    player.GetComponent<Animator>().SetBool("isAttacking", false);
                }
                if (playerInPushTrigger && Input.GetKey("space"))
                {
                    state = State.PushingWhileIdle;
                }
            }
                break;

            case State.Walking:
            {
                // flip direction of player
                if (moveX < 0)
                {
                    player.GetComponent<SpriteRenderer>().flipX = true;
                }
                else if (moveX > 0)
                {
                    player.GetComponent<SpriteRenderer>().flipX = false;
                }
                // play walking animation
                player.GetComponent<Animator>().SetBool("isWalking", true);
                player.GetComponent<Animator>().SetBool("isPushing", false);
                // state to idle
                if (moveX == 0 && moveY == 0)
                {
                    state = State.Idle;
                }
                
                if (inAttackSpace && Input.GetKey("space"))
                {
                    state = State.AttackingWhileWalking;
                }

                if (playerInPushTrigger && Input.GetKey("space"))
                {
                    state = State.PushingWhileWalking;
                }
                
                // set to move player
                moveDir.x = moveX * playerSpeed.x;
                moveDir.y = moveY * playerSpeed.y;
            }
                break;

            case State.Attacking:
            {
                player.GetComponent<Animator>().SetBool("isAttacking", true);
                if (Input.GetKeyUp("space"))
                {
                    state = State.Idle;
                    player.GetComponent<Animator>().SetBool("isAttacking", false);
                }

                if (moveX != 0 || moveY != 0)
                {
                    state = State.AttackingWhileWalking;
                }
              
            }
                break;

            case State.AttackingWhileWalking:
            {
                player.GetComponent<Animator>().SetBool("isWalking", false);
                player.GetComponent<Animator>().SetBool("isAttacking", true);
                if (Input.GetKeyUp("space"))
                {
                    state = State.Idle;
                    player.GetComponent<Animator>().SetBool("isAttacking", false);
                }
                if (moveX != 0 && moveY != 0)
                {
                    state = State.Attacking;
                }
                // flip direction of player
                if (moveX < 0)
                {
                    player.GetComponent<SpriteRenderer>().flipX = true;
                }
                else if (moveX > 0)
                {
                    player.GetComponent<SpriteRenderer>().flipX = false;
                }
                // set to move player
                moveDir.x = moveX * playerSpeed.x;
                moveDir.y = moveY * playerSpeed.y;
            }
                break;

            case State.KilledByFog:
            {
                playerAlive = false;
                player.GetComponent<Animator>().SetBool("isWalking", false);
                player.GetComponent<Animator>().SetBool("isAttacking", false);
                player.GetComponent<Animator>().SetTrigger("isDead");
                    
            } break;

            case State.ActivelyMoving:
            {
                player.GetComponent<Animator>().SetBool("isAttacking", false);
                player.GetComponent<Animator>().SetBool("isWalking", true);
                moveDir.y = 0;
                moveDir.x = 3;
            } break;

            case State.PushingWhileIdle:
            {
                player.GetComponent<Animator>().SetBool("isWalking", false);
                player.GetComponent<Animator>().SetBool("isAttacking", false);
                player.GetComponent<Animator>().SetBool("isPushing", true);
                if (Input.GetKeyUp("space"))
                {
                    state = State.Idle;
                }
                //player.GetComponent<Animator>().
                if (moveX != 0)
                {
                    state = State.PushingWhileWalking;
                }
            } break;

            case State.PushingWhileWalking:
            {
                player.GetComponent<Animator>().SetBool("isWalking", false);
                player.GetComponent<Animator>().SetBool("isAttacking", false);
                player.GetComponent<Animator>().SetBool("isPushing", true);
                
                if (Input.GetKeyUp("space"))
                {
                    state = State.Walking;
                }
                //player.GetComponent<Animator>().
                if (moveX == 0)
                {
                    state = State.PushingWhileIdle;
                }
                moveDir.x = moveX * playerSpeed.x;
                //moveDir.y = moveY * playerSpeed.y;
            } break;
        }
        
        
        // move player controller
        if (playerAlive)
        {
            CharacterController c = player.GetComponent<CharacterController>();
            c.Move(moveDir * Time.deltaTime);
        }
       
    }

    public void MoveFog()
    {
        switch (fogState)
        {
            case FogState.Idle:
            {
                if (fog.GetComponent<KillerFogAttributes>().fogTriggered)
                {
                    fogState = FogState.Approaching;
                }
            }
                break;
            case FogState.Approaching:
            {
                // fog advancing movement
                fog.GetComponent<Transform>().position += (Vector3.right * Time.deltaTime);
            }
                break;
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        
        
        
    }
}
