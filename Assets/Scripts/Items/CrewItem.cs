using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewItem : Item
{

    public override void Fix()
    {
        if (itemHealth >= maxItemHealth)
        {
            itemState = STATE_PERFECT;
            animator.SetBool("Sick", false);
            AfterFix();
        }
        if (itemHealth <= maxItemHealth) itemHealth++;
    }

    internal override void Broke()
    {
        itemHealth = 0;
        itemState = STATE_BROKEN;
        animator.SetBool("Sick", true);
    }
    
}
