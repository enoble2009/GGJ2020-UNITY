using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{

    [SerializeField]
    private LayerController[] layers = new LayerController[4];
    [SerializeField]
    private float generalSpeed;


    // Start is called before the first frame update
    void Start()
    {
        if (GameController.Inst != null)
            generalSpeed = GameController.Inst.getBoatSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.Inst != null)
        {
            generalSpeed = GameController.Inst.getBoatSpeed();
        }
    }

    public float GetGeneralSpeed()
    {
        return generalSpeed;
    }

    public void SetSpeed(float speed)
    {
        generalSpeed = speed;
    }
}
