using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashController : MonoBehaviour
{

    [SerializeField]
    private SpriteRenderer logoRenderer;
    [SerializeField]
    private SpriteRenderer bgRenderer;
    [SerializeField]
    private float timeToHide = 2f;
    private float timeToDissapear = .2f;

    // Start is called before the first frame update
    void Start()
    {
        logoRenderer = transform.Find("Logo").GetComponent<SpriteRenderer>();
        bgRenderer = transform.Find("Background").GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        timeToHide -= Time.deltaTime;
        if (timeToHide <= 0f)
        {
            timeToDissapear -= Time.deltaTime;
            logoRenderer.color = new Color(1f, 1f, 1f, timeToDissapear / .2f);
            bgRenderer.color = new Color(0f, 0f, 0f, timeToDissapear / .2f);

            if (timeToDissapear <= 0f)
            {
                FindObjectOfType<MainMenuController>().StartMainMenu();
                Destroy(this.gameObject);
            }
        }
    }
}
