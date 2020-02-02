using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsLayer : LayerController
{

    [SerializeField]
    private GameObject[] spawnObjects;
    [SerializeField]
    private float minY = 0.66f;
    [SerializeField]
    private float maxY = 2.75f;
    [SerializeField]
    private float minTimeToSpawn = 10f;
    [SerializeField]
    private float maxTimeToSpawn = 100f;

    private float timeToNewSpawn = 0f;

    protected override void UltraUpdate()
    {
        if (timeToNewSpawn <= 0 && Mathf.Abs(bgController.GetGeneralSpeed()) > 0f)
        {
            Instantiate<GameObject>(spawnObjects[Random.Range(0, spawnObjects.Length)], new Vector3(6f, Random.Range(minY, maxY), 0f), Quaternion.identity, this.transform);
            timeToNewSpawn = Random.Range(minTimeToSpawn, maxTimeToSpawn);
        }
        else
        {
            timeToNewSpawn -= Time.deltaTime;
        }
    }

}
