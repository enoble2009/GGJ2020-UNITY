using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonItem : TimeRepairableItem
{

    private Color red1 = new Color(0.8863f, 0.713723f, 0.713723f);
    private Color red2 = new Color(0.78431f, 0.211765f, 0.211765f);
    private Color actualColor;
    [SerializeField]
    private float brokenAnimationSpeed = 2f;
    [SerializeField]
    private bool withAmmo = false;

    private bool hasAllMaterialsToBeFixed = false;
    [SerializeField]
    private float maxTimePressingRepair = 1f;
    private float timePressingRepair = 0f;
    [SerializeField]
    private float maxTimeShooting = 1f;
    private float timeShooting = 0f;

    [SerializeField]
    private CrewItem sailor;
    [SerializeField]
    private Animator ball;

    public void Broke()
    {
        InternalBroke();
    }

    protected override void AfterBroke()
    {
    }

    protected override void AfterFix()
    {
        spriteRenderer.color = Color.white;
    }

    protected override void WhileBroken()
    {
        spriteRenderer.color = Color.Lerp(red1, red2, Mathf.PingPong(Time.time, 1));
        actualColor = spriteRenderer.color;
    }

    protected override void WhilePerfect()
    {
        if (withAmmo && !sailor.IsBroken())
        {
            timeShooting += Time.deltaTime;
            if (timeShooting >= maxTimeShooting)
            {
                timeShooting = 0f;
                withAmmo = false;
                Broke();
            }
        }
        if (playerIsTouching && Input.GetKey(KeyCode.T))
        {
            if (itemState != STATE_BROKEN && (hasAllMaterialsToBeFixed || HasAllMaterials()))
            {
                hasAllMaterialsToBeFixed = true;
                GameController.Inst.player.SetState(PlayerController.STATE_WORKING);
                timePressingRepair += Time.deltaTime;
                if (timePressingRepair >= maxTimePressingRepair)
                {
                    timePressingRepair = 0f;
                    GameController.Inst.ThrowMaterials();
                    hasAllMaterialsToBeFixed = true;
                    withAmmo = true;
                    ball.Play("BallIdle");
                    Debug.Log("Cañón armado");
                }
            } else
            {
                //Debug.Log("El cañon está roto o le faltan materiales");
            }
        }
    }
}
