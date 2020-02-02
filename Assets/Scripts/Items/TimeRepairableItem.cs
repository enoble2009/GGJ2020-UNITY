using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimeRepairableItem : MonoBehaviour
{
    protected const int STATE_PERFECT = 0;
    protected const int STATE_BROKEN = 1;

    [SerializeField]
    private float maxTimeToRecover;

    [SerializeField]
    protected int itemState;
    [SerializeField]
    private Sprite perfectSprite;
    [SerializeField]
    private Sprite brokenSprite;

    [SerializeField]
    protected SpriteRenderer spriteRenderer;
    [SerializeField]
    protected bool playerIsTouching;
    [SerializeField]
    protected int[] materialsToFix = new int[] { 4, 5 };

    private float timeToRecover;



    // Start is called before the first frame update
    void Start()
    {
        itemState = STATE_PERFECT;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsBroken())
        {
            WhileBroken();
        } else
        {
            WhilePerfect();
        }
        if (timeToRecover >= 0f)
        {
            timeToRecover -= Time.deltaTime;
            if (timeToRecover <= 0)
            {
                itemState = STATE_PERFECT;
                spriteRenderer.sprite = perfectSprite;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player") playerIsTouching = true;
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player") playerIsTouching = false;
    }

    public bool IsBroken()
    {
        return itemState == STATE_BROKEN;
    }

    protected void InternalBroke()
    {
        
        itemState = STATE_BROKEN;
        timeToRecover = maxTimeToRecover;
        spriteRenderer.sprite = brokenSprite;
    }

    protected bool HasAllMaterials()
    {
        foreach (int material in materialsToFix)
        {
            if (!GameController.Inst.findMaterial(material)) return false;
        }
        return true;
    }


    protected abstract void AfterFix();
    protected abstract void AfterBroke();
    protected abstract void WhileBroken();
    protected abstract void WhilePerfect();

}
