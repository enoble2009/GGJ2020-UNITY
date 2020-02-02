using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

    private const int STATE_SPLASH = 0;
    private const int STATE_MENU = 1;
    private const int STATE_CREDITS = 2;
    private const int STATE_TUTORIAL1 = 3;
    private const int STATE_TUTORIAL2 = 4;

    private int state;

    [SerializeField]
    private SpriteRenderer mainMenu;
    [SerializeField]
    private SpriteRenderer credits;
    [SerializeField]
    private SpriteRenderer tutorial1;
    [SerializeField]
    private SpriteRenderer tutorial2;

    private float timeToDisappear = .2f;
    private float timeToAppear = .3f;
    private float timeToNewStatus = 2f;
    private SpriteRenderer showing;
    private SpriteRenderer hiding;
    private int newStatus = -1;

    // Start is called before the first frame update
    void Start()
    {
        state = STATE_SPLASH;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(state);
        if (newStatus > -1) TurnToNewStatus();
        if (showing != null) Show(showing);
        if (hiding != null) Hide(hiding);
        if (showing == null && hiding == null && newStatus == -1)
        {
            if (state == STATE_SPLASH)
            {
                // NO SE PUEDE HACER NADA
            }
            else if (state == STATE_MENU)
            {
                ProcessMouse();
            }
            else if (state == STATE_CREDITS)
            {
                // CUALQUIER CLICK O TECLA VUELVE A MENU
                if (Input.anyKey || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                {
                    newStatus = STATE_MENU;
                    hiding = credits;
                    showing = mainMenu;
                }
            }
            else if (state == STATE_TUTORIAL1)
            {
                // CUALQUIER CLICK O TECLA VUELVE A MENU
                if (Input.anyKey || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                {
                    AudioManager.Inst.Play("boton");
                    newStatus = STATE_TUTORIAL2;
                    hiding = tutorial1;
                    showing = tutorial2;
                }
            }
            else if (state == STATE_TUTORIAL2)
            {
                // CUALQUIER CLICK O TECLA VUELVE A MENU
                if (Input.anyKey || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                {
                    AudioManager.Inst.Play("boton");
                    
                    SceneManager.LoadScene("Game");
                }

                
            }
        }
    }

    private void TurnToNewStatus()
    {
        timeToNewStatus -= Time.deltaTime;
        if (timeToNewStatus <= 0)
        {
            state = newStatus;
            newStatus = -1;
            timeToNewStatus = 2f;
        }
    }

    internal void StartMainMenu()
    {
        state = STATE_MENU;
    }

    private void ProcessMouse()
    {
        if (showing == null && hiding == null && newStatus == -1)
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

                if (hit.collider != null && hit.collider.name == "PLAY")
                {
                    AudioManager.Inst.Play("boton");

                    hiding = mainMenu;
                    showing = tutorial1;
                    newStatus = STATE_TUTORIAL1;
                }
                else if (hit.collider != null && hit.collider.name == "QUIT")
                {
                    AudioManager.Inst.Play("boton");
                    Application.Quit();
                }

                

                else if (hit.collider != null && hit.collider.name == "CREDITS")
                {
                    AudioManager.Inst.Play("boton");
                    hiding = mainMenu;
                    showing = credits;
                    newStatus = STATE_CREDITS;
                }
        }
    }

    private void Show(SpriteRenderer sr)
    {
        timeToAppear -= Time.deltaTime;
        sr.color = new Color(1f, 1f, 1f, 1-timeToAppear / .2f);

        if (timeToAppear <= 0f)
        {
            sr.color = new Color(1f, 1f, 1f, 1f);
            showing = null;
            timeToAppear = .3f;
        }
    }

    private void Hide(SpriteRenderer sr)
    {
        timeToDisappear -= Time.deltaTime;
        sr.color = new Color(1f, 1f, 1f, timeToDisappear / .2f);

        if (timeToDisappear <= 0f)
        {
            sr.color = new Color(1f, 1f, 1f, 0f);
            hiding = null;
            timeToDisappear = .2f;
        }
    }
}
