using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterItem : Item
{

    public void InternalBroke()
    {
        itemHealth = 0;
        itemState = STATE_BROKEN;
        spriteRenderer.enabled = true;
    }

    public override void Fix()
    {
        if (itemHealth >= maxItemHealth)
        {
            itemState = STATE_PERFECT;
            spriteRenderer.enabled = false;
            AfterFix();
        }
        if (itemHealth <= maxItemHealth) itemHealth++;
    }

    public void SuperFix()
    {
        itemHealth = maxItemHealth;
        itemState = STATE_PERFECT;
        spriteRenderer.enabled = false;
    }

    internal override void Broke()
    {
        InternalBroke();
    }
}
