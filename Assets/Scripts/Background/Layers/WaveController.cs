using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{

    [SerializeField]
    private float vSpeed;
    [SerializeField]
    private float maxVPos;
    [SerializeField]
    private float minVPos;
    [SerializeField]
    private float yPos;

    [SerializeField]
    private bool movingUp = true;

    // Start is called before the first frame update
    void Start()
    {
        yPos = transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        yPos = (movingUp ? 1:-1) * Time.deltaTime * vSpeed + yPos;
        if (yPos >= maxVPos) movingUp = false;
        else if (yPos <= minVPos) movingUp = true;
        transform.localPosition = new Vector3(transform.localPosition.x, yPos, transform.localPosition.z);
    }
}
