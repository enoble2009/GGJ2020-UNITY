using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairableItem : Item
{
    [SerializeField]
    protected Sprite perfectSprite;
    [SerializeField]
    protected Sprite brokenSprite;



    public void InternalBroke()
    {
        itemHealth = 0;
        itemState = STATE_BROKEN;
        spriteRenderer.sprite = brokenSprite;
    }

    public override void Fix()
    {
        if (itemHealth >= maxItemHealth)
        {
            itemState = STATE_PERFECT;

           

            spriteRenderer.sprite = perfectSprite;
            AfterFix();

        }
        if (itemHealth <= maxItemHealth) itemHealth++;
    }

    public void SuperFix()
    {
        itemHealth = maxItemHealth;
        itemState = STATE_PERFECT;
        spriteRenderer.sprite = perfectSprite;
    }

    internal override void Broke()
    {
        InternalBroke();
    }

}
