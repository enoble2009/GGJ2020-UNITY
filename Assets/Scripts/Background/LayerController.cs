using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LayerController : MonoBehaviour
{

    [SerializeField]
    private float speed;
    [SerializeField]
    private bool regenerate;
    [SerializeField]
    private bool byBGSpeed;
    [SerializeField]
    private Transform[] firstItems = new Transform[1];
    [SerializeField]
    private Transform[] secondItems = new Transform[1];
    [SerializeField]
    private Transform[] thirdItems = new Transform[1];

    [SerializeField]
    private float movement = 0f;
    [SerializeField]
    private int timesRegenerating = 1;

    protected BackgroundController bgController;

    private const float moveForward = 7.67f;


    // Start is called before the first frame update
    void Start()
    {
        bgController = GetComponentInParent<BackgroundController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 velocity = Vector3.zero;
        Vector3 desiredPosition = transform.position + new Vector3(byBGSpeed? speed * bgController.GetGeneralSpeed(): speed, 0, 0);
        Vector3 smoothPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, 0.3f);
        movement += smoothPosition.x - transform.position.x;
        transform.position = smoothPosition;

        if (regenerate) Regenerate();
        UltraUpdate();
    }

    void Regenerate()
    {
        if (Mathf.Abs(movement)/moveForward >= timesRegenerating)
        {
            timesRegenerating++;
            foreach (Transform i in firstItems)
            {
                i.position = new Vector3(i.position.x + moveForward * 3, i.position.y, i.position.z);
            }
            Transform[] tempItems = firstItems;
            firstItems = secondItems;
            secondItems = thirdItems;
            thirdItems = tempItems;
        }
    }

    protected abstract void UltraUpdate();

}
