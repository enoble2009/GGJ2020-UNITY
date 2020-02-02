using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{

    protected const int STATE_PERFECT = 0;
    protected const int STATE_BROKEN = 1;

    [SerializeField]
    protected int itemState;
    [SerializeField]
    protected int maxItemHealth = 3;
    [SerializeField]
    protected int[] materialsToFix;

    protected int itemHealth;
    protected SpriteRenderer spriteRenderer;
    protected Animator animator;
    protected Animator ownAnimator;

    private bool playerIsTouching = false;
    private bool hasAllMaterialsToBeFixed = false;

    private float maxTimePressingRepair = 1f;
    private float timePressingRepair = 0f;

    void Start()
    {
        itemState = STATE_PERFECT;
        itemHealth = maxItemHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        ownAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (ownAnimator != null)
            if (itemState == STATE_BROKEN) ownAnimator.SetBool("Broken", true);
            else if (itemState == STATE_PERFECT) ownAnimator.SetBool("Broken", false);

        if (playerIsTouching && Input.GetKey(KeyCode.T))
        {
            if (itemState == STATE_BROKEN && (hasAllMaterialsToBeFixed || HasAllMaterials()))
                GameController.Inst.player.SetState(PlayerController.STATE_WORKING);
            timePressingRepair += Time.deltaTime;
            if (timePressingRepair >= maxTimePressingRepair)
            {
                timePressingRepair = 0f;
                ProcessingRepair();
            }
        }
        else if (playerIsTouching && Input.GetKeyUp(KeyCode.T))
        {
            

            timePressingRepair = 0f;
            ProcessingRepair();
        }
    }

    void ProcessingRepair()
    {
        if (itemState == STATE_BROKEN && (hasAllMaterialsToBeFixed || HasAllMaterials()))
        {
            GameController.Inst.ThrowMaterials();
            hasAllMaterialsToBeFixed = true;

            Fix();
            Debug.Log("Arreglando (" + itemHealth + "/" + maxItemHealth + ")");

            
        }
        else
        {
            // ERROR: No tiene los materiales suficientes o no está roto.
            //Debug.Log("No está roto o no tenés los materiales");
        }
    }

    protected void AfterFix()
    {
        hasAllMaterialsToBeFixed = false;
    }

    private bool HasAllMaterials()
    {
        foreach (int material in materialsToFix)
        {
            if (!GameController.Inst.findMaterial(material)) return false;
        }
        return true;
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player") playerIsTouching = true;
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player") playerIsTouching = false;
    }

    internal abstract void Broke();
    public abstract void Fix();

    public bool IsBroken()
    {
        return itemState == STATE_BROKEN;
    }
}
