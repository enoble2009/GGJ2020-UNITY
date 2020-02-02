using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyExplosions : MonoBehaviour
{

    [SerializeField]
    public GameObject explosionTemplate;

    [SerializeField]
    private float timeToNewExplosion = 0f;
    [SerializeField]
    public float minTimeExplosion = 2f;
    [SerializeField]
    public float maxTimeExplosion = 10f;

    [SerializeField]
    public float minX;
    [SerializeField]
    public float maxX;
    [SerializeField]
    public float minY;
    [SerializeField]
    public float maxY;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timeToNewExplosion <= 0f)
        {
            timeToNewExplosion = UnityEngine.Random.Range(minTimeExplosion, maxTimeExplosion);
            CreateExplosion();
        }
        if (timeToNewExplosion > 0f)
        {
            timeToNewExplosion -= Time.deltaTime;
        }
    }

    private void CreateExplosion()
    {
        float xPos = UnityEngine.Random.Range(minX, maxX);
        float yPos = UnityEngine.Random.Range(minY, maxY);
        GameObject e = Instantiate<GameObject>(explosionTemplate, new Vector3(xPos, yPos, 0f), Quaternion.identity, this.transform);
        float scaleXY = UnityEngine.Random.Range(1, 3);
        e.transform.localScale = new Vector3(scaleXY, scaleXY, 1f);
    }
}
