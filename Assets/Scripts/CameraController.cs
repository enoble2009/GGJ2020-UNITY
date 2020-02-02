using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{

    [SerializeField]
    private float zoomSpeed = 1f;
    [SerializeField]
    private float alphaSpeed = 1f;
    private float defaultZoom = 1f;
    private Camera camera;

    private float maxZoom = 1.65f;
    private float minZoom = 0.7f;

    private Color alphaColor = new Color(1f, 1f, 1f, 0.2f);
    [SerializeField]
    private Image[] uiImages;
    private float duration = 5; // duration in seconds
    private float t = 0; // lerp control variable

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.KeypadMinus) || Input.GetKey(KeyCode.O))
        {
            ZoomOut();
        }
        else if (Input.GetKey(KeyCode.Plus) || Input.GetKey(KeyCode.KeypadPlus) || Input.GetKey(KeyCode.P))
        {
            ZoomIn();
        }
        else if (Input.GetKey(KeyCode.Z))
        {
            camera.orthographicSize = defaultZoom;
            UpdateAplhaUI();
        }
    }

    private void ZoomIn()
    {
        if (camera.orthographicSize > minZoom)
        {
            camera.orthographicSize -= zoomSpeed * Time.deltaTime;
            UpdateAplhaUI();
        }
    }

    private void ZoomOut()
    {
        if (camera.orthographicSize < maxZoom)
        {
            camera.orthographicSize += zoomSpeed * Time.deltaTime;
            UpdateAplhaUI();
        }
    }

    void UpdateAplhaUI()
    {
        if (camera.orthographicSize > 0.8f && camera.orthographicSize < 1f)
            foreach (Image img in uiImages)
                img.color = new Color(1f, 1f, 1f, ((camera.orthographicSize - 0.8f) / 0.2f));
        if (camera.orthographicSize > 1f) foreach(Image img in uiImages) img.color = Color.white;
        if (camera.orthographicSize < .8f) foreach (Image img in uiImages) img.color = alphaColor;
    }

}
