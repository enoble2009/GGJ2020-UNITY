using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EphemeralObject : MonoBehaviour
{

    private const int STATE_START = 0;
    private const int STATE_VISIBLE = 1;

    private int state;

    // Start is called before the first frame update
    void Awake()
    {
        state = STATE_START;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnBecameVisible()
    {
        state = STATE_VISIBLE;
    }

    void OnBecameInvisible()
    {
        if (state == STATE_VISIBLE)
            Destroy(gameObject);
    }

}
