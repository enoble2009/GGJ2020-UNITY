using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMaterial : MonoBehaviour
{
    public const int TYPE_WOOD = 0;
    public const int TYPE_WATER = 1;
    public const int TYPE_ROPE = 2;
    public const int TYPE_CLOTH = 3;
    public const int TYPE_AMMO = 4;
    public const int TYPE_POWDER = 5;
    public const int TYPE_MEDICINE = 6;
    public const int TYPE_RUM = 7;
    public const int TYPE_NAIL = 8;

    private int mType;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setMaterialType(int type)
    {
        mType = type;
    }

    public int getMaterialType()
    {
        return mType;
    }
}
