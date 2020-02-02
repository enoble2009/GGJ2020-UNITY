using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomExplosion : MonoBehaviour
{

    private float timeToDelete = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeToDelete -= Time.deltaTime;
        if (timeToDelete <= 0f)
        {
            Destroy(this.gameObject);
        }
    }
}
