using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator animator;
    

    public float runSpeed = 40f;

    float horizontalMove = 0f;
    float verticalMove = 0f;

    bool isCarry = false;
    bool isTired = false;
    bool Defeat = false;
    bool isClimb = false;
    bool isWork = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        verticalMove = Input.GetAxisRaw("Vertical") * runSpeed;

        animator.SetFloat("VerticalSpeed", Mathf.Abs(verticalMove));


    }

    /*public void setIsStairOrWorking(bool state)
    {
        isStairOrWorking = state;
        animator.SetBool("IsStairOrWorking", state);
    }*/

    public void setIsClimb(bool state)
    {
        isClimb = state;
        animator.SetBool("IsClimb", state);
    }

    public void setIsWork(bool state)
    {
        isWork = state;
        animator.SetBool("IsWork", state);
    }

    public void setIsCarry(bool state)
    {
        isCarry = state;
        animator.SetBool("IsCarry", state);
    }

    public void setIsTired(bool state)
    {
        isTired = state;
        animator.SetBool("IsTired", state);
    }

    public void setDefeat(bool state)
    {
        Defeat = state;
        animator.SetBool("Defeat", state);
    }
}
