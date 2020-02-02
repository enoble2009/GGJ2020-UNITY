using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public const int STATE_IDLE = 0;
    public const int STATE_WALKING = 1;
    public const int STATE_CLIMBING = 2;
    public const int STATE_FALLING = 3;
    public const int STATE_ROPE = 4;
    public const int STATE_CLIMB_IDLE = 5;
    public const int STATE_WORKING = 6;
    public const int STATE_TIRED = 7;
    public const int STATE_LOSE = 8;


    public const int SUBSTATE_ON_MATERIAL = 0;
    public const int SUBSTATE_OFF_MATERIAL = 1;

    private bool collecting = false;
    [SerializeField]
    private bool canMove = true;

    private PlayerAnimation PAnimation;

    [SerializeField]
    private int playerState;
    [SerializeField]
    private int formerState;

    [SerializeField]
    private int subplayerState;

    [SerializeField]
    private float maxWalkSpeed = 2f;
    [SerializeField]
    private float maxClimbSpeed = 2f;

    [SerializeField]
    private float walkSpeed = 9f;
    [SerializeField]
    private float walkInLadderSpeed = 7f;
    [SerializeField]
    private float climbSpeed = 1.5f;
    [SerializeField]
    private float fatigue = 100f;
    [SerializeField]
    private float Maxfatigue = 100f;
    [SerializeField]
    private float fatigueSpeed = 3f;
    [SerializeField]
    private float fatigueRegen = 20f;


    [SerializeField]
    private bool inLadder = false;
    [SerializeField]
    private bool onGround = true;

    private Rigidbody2D rb2d;
    [SerializeField]
    private Collider2D foots;

    [SerializeField]
    private Vector2 test_velocity;

    [SerializeField]
    private SpriteRenderer bodyRenderer;
    private bool flip = false;
    private Transform hands;
    private Vector3 handsNormal = new Vector3(0.07f, -0.02f, 0f), handsFlip = new Vector3(-0.07f, -0.02f, 0f);

    private float hideAlert = 0f;

    private void Awake()
    {
        if (GameController.Inst != null) GameController.Inst.setPlayer(this);
    }

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        hands = transform.Find("Hands");

        PAnimation = GetComponentInChildren<PlayerAnimation>();

        SetState(STATE_IDLE);
        SetSubState(SUBSTATE_OFF_MATERIAL);
    }

    void Update()
    {
        if (hideAlert > 0f)
        {
            hideAlert -= Time.deltaTime;
            if (hideAlert <= 0f)
            {
                Transform alert = transform.Find("Alert");
                alert.GetComponent<SpriteRenderer>().color = new Color(0f,0f,0f,0f);
            }
        }

        CalculateVelocity_force();

        if ((GetState() == STATE_WALKING) && (Input.GetAxisRaw("Horizontal") == 0))
        {
            SetState(STATE_IDLE);
        }

        if (GetState() == STATE_ROPE)
        {
            rb2d.velocity = new Vector2(2f, 1f);
        }

        if (Input.GetKeyUp(KeyCode.R))
        {

            AudioManager.Inst.Play("agarrar_item");
            dropMaterials();
        }

        if (Input.GetKeyUp(KeyCode.T))
        {
            if (GameController.Inst.findMaterial(CMaterial.TYPE_RUM))
            {
                drinkRum();

                //prueba

                dropMaterials();

            }

            
        }

        if ((fatigue <= 0) && (GetState() != STATE_TIRED))
        {
            formerState = GetState();
            SetState(STATE_TIRED);
        }

        if (GetState() == STATE_TIRED)
        {
            if (fatigue >= Maxfatigue)
            {
                fatigue = Maxfatigue;
                SetState(formerState);
            }
            else
            {
                fatigue += Time.deltaTime * fatigueRegen;
            }


        }
        else
        {
            if (GetState() == STATE_WALKING)
            {
                fatigue -= Time.deltaTime * fatigueSpeed;
            }

        }

        if ((GetState()== STATE_WORKING) && (Input.GetKeyUp(KeyCode.T)))
        {
            SetState(STATE_IDLE);
        }

    }

    public void AddMaterialToHands(CMaterial mat, int i)
    {
        if (i == 0) mat.transform.parent = hands.transform.Find("FirstItem").transform;
        if (i == 1) mat.transform.parent = hands.transform.Find("SecondItem").transform;
        mat.transform.localPosition = Vector3.zero;
    }

    public void ThrowMaterialsFromHands()
    {
        if (hands.transform.Find("FirstItem").childCount > 0)
            Destroy(hands.transform.Find("FirstItem").GetChild(0).gameObject);
        if (hands.transform.Find("SecondItem").childCount > 0)
            Destroy(hands.transform.Find("SecondItem").GetChild(0).gameObject);
    }

    public void dropMaterials()
    {
        GameController.Inst.ThrowMaterials();
        collecting = false;
        PAnimation.setIsCarry(false);
        ThrowMaterialsFromHands();
    }


    void CalculateVelocity_force()
    {
        if ((Input.GetAxisRaw("Horizontal") < 0) && canMove)
        {
            flip = true;
            bodyRenderer.flipX = true;
            hands.localPosition = handsFlip;

            

            SetState(STATE_WALKING);
        }
        else if ((Input.GetAxisRaw("Horizontal") > 0) && canMove)
        {
            flip = false;
            bodyRenderer.flipX = false;
            hands.localPosition = handsNormal;

            

            SetState(STATE_WALKING);
        }
        

        if ((onGround && Input.GetAxis("Horizontal") != 0) && canMove)// Walking on ground
        {
            rb2d.AddForce(Vector2.right * WalkSpeedUntilMax());
        }
        if (inLadder)
        {
            if (!onGround)
            {
                if (((GetState() == STATE_CLIMBING) && (Input.GetAxis("Vertical") == 0)) && canMove)
                {
                    SetState(STATE_CLIMB_IDLE);
                }

                if (((GetState() == STATE_CLIMB_IDLE) && (Input.GetAxis("Vertical") != 0)) && canMove)
                {
                    SetState(STATE_CLIMBING);
                }


                if ((Input.GetAxis("Horizontal") != 0) && canMove)// Walking in ladder (no ground)
                {
                    rb2d.velocity = new Vector2(walkInLadderSpeed * Input.GetAxis("Horizontal") * Time.deltaTime, rb2d.velocity.y);
                }
                else
                {
                    rb2d.velocity = new Vector2(0f, rb2d.velocity.y);
                    
                }
            }
            if ((Input.GetAxis("Vertical") != 0) && canMove)  // Climbing in ladder
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, ClimbSpeedUntilMax());
                SetState(STATE_CLIMBING);
            }
            else
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);
            }
        }
        test_velocity = rb2d.velocity;
    }

    internal void SendAlert(Vector3 disasterPosition)
    {
        Vector3 direction = Vector3.Normalize(disasterPosition - this.transform.position);
        Vector3 origin = this.transform.position;

        Transform alert = transform.Find("Alert");
        alert.position = origin + direction/3;
        alert.GetComponent<SpriteRenderer>().color = Color.white;

        hideAlert = 2f;
    }

    private float ClimbSpeedUntilMax()
    {
        float ySpeed = Input.GetAxis("Vertical") * climbSpeed;
        return Mathf.Abs(ySpeed) > maxClimbSpeed ? maxClimbSpeed * Mathf.Sign(ySpeed) : ySpeed;
    }

    private float WalkSpeedUntilMax()
    {
        float xSpeed = Input.GetAxis("Horizontal") * walkSpeed;
        return Mathf.Abs(xSpeed) > maxWalkSpeed ? maxWalkSpeed * Mathf.Sign(xSpeed) : xSpeed;
    }


    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Ladder")) inLadder = !col.GetComponent<LadderItem>().IsBroken();
        else if (col.CompareTag("Rope")) SetState(STATE_ROPE);

        UpdateGravity();
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Ladder")) inLadder = false;
        if (onGround) SetState(STATE_IDLE);
        if (!onGround) SetState(STATE_FALLING);

        UpdateGravity();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (onGround) SetState(STATE_IDLE);
    }
    private void OnCollisionStay2D(Collision2D col)
    {
        if (col.collider.CompareTag("Floor")) onGround = true;

        UpdateGravity();
    }
    private void OnCollisionExit2D(Collision2D col)
    {
        if (col.collider.CompareTag("Floor")) onGround = false;

        UpdateGravity();
    }


    private void UpdateGravity()
    {
        if (inLadder)
        {
            if (!onGround)
            {
                if (Input.GetAxis("Vertical") != 0) rb2d.gravityScale = 1;
                else rb2d.gravityScale = 0;
            }
        } else 
        {
            if (onGround) rb2d.gravityScale = 1;
            if (!onGround) rb2d.gravityScale = 3;
        }
    }

    public void SetState(int state)
    {
        /*if (this.playerState == STATE_WALKING)
        {
            AudioManager.Inst.Stop("pasos_caminando");
        }*/

        this.playerState = state;
        this.foots.enabled = ((this.playerState != STATE_CLIMBING) && (this.playerState != STATE_CLIMB_IDLE));

        if ((state == STATE_WALKING))
        {
            //AudioManager.Inst.Play("pasos_caminando");
        }





        if ((state == STATE_CLIMBING) || (state == STATE_CLIMB_IDLE))
        {
            PAnimation.setIsClimb(true);
        }
        else
        {
            PAnimation.setIsClimb(false);
        }

        if (state == STATE_WORKING)
        {
            //AudioManager.Inst.Play("reparando");
            PAnimation.setIsWork(true);
        }
        else
        {
            PAnimation.setIsWork(false);
        }


        if (state == STATE_TIRED)
        {
            canMove = false;
            PAnimation.setIsTired(true);
        }

        else if (state == STATE_LOSE)
        {
            canMove = false;
            PAnimation.setDefeat(true);
            // PAnimation.setIsTired(true);
        }

        else
        {
            canMove = true;
            PAnimation.setIsTired(false);
        }

        


    }

    public int GetState()
    {
        return this.playerState;
    }

    public void SetSubState(int state)
    {
        this.subplayerState = state;
        
    }

    public int GetSubState()
    {
        return this.subplayerState;
    }

    public bool isCollecting()
    {
        return collecting;
    }

    public void setCollecting(bool newStatus)
    {
        collecting = newStatus;
        PAnimation.setIsCarry(newStatus);
    }



    /** DEPRECATED **/
    private void CalculateVelocity_noforce()
    {
        Vector2 speed = Vector2.zero;
        if (Input.GetAxis("Horizontal") != 0)
        {
            speed += Vector2.right * Input.GetAxis("Horizontal") * walkSpeed;
        }
        if (inLadder)
        {
            if (Input.GetAxis("Vertical") != 0)
            {
                speed += Vector2.up * Input.GetAxis("Vertical") * climbSpeed;
                rb2d.gravityScale = 0;
            }
        }

        rb2d.velocity = speed;
    }


    public float getFatigue()
    {
        return fatigue;
    }

    public float getMaxFatigue()
    {
        return Maxfatigue; ;
    }

    public void drinkRum()
    {
        if (GameController.Inst.findMaterial(CMaterial.TYPE_RUM))
        {
            GameController.Inst.removeItemFromList(CMaterial.TYPE_RUM);
        }

        fatigue = Maxfatigue;

    }


}
