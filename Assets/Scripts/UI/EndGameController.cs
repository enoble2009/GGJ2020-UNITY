using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndGameController : MonoBehaviour
{

    private const int STATE_NONE = 0;
    private const int STATE_WIN = 1;
    private const int STATE_LOSE = 2;
    private const int STATE_CLICK = 3;

    [SerializeField]
    private Image background;
    [SerializeField]
    private Image win;
    [SerializeField]
    private Image lose;

    private int state;
    private float timeToAppearMessage = 2f;

    // Start is called before the first frame update
    void Start()
    {
        state = STATE_NONE;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == STATE_WIN)
        {
            timeToAppearMessage -= Time.deltaTime;
            background.color = new Color(0.6351094f, 0.8679245f, 0.5444998f, (2f-timeToAppearMessage)/2.5f);
            win.color = new Color(1f, 1f, 1f, (2f - timeToAppearMessage) / 2f);
            if (timeToAppearMessage <= 0f)
            {
                background.color = new Color(0.6351094f, 0.8679245f, 0.5444998f, .8f);
                win.color = Color.white;
                state = STATE_CLICK;
            }
        }
        if (state == STATE_LOSE)
        {
            timeToAppearMessage -= Time.deltaTime;
            background.color = new Color(0.8666667f, 0.5995196f, 0.5995196f, (2f - timeToAppearMessage) / 2.5f);
            lose.color = new Color(1f, 1f, 1f, (2f - timeToAppearMessage) / 2f);
            if (timeToAppearMessage <= 0f)
            {
                background.color = new Color(0.8666667f, 0.5995196f, 0.5995196f, .8f);
                lose.color = Color.white;
                state = STATE_CLICK;
            }
        }
        if (state == STATE_CLICK)
        {
            if (Input.anyKey || Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                SceneManager.LoadScene("Main Menu");
            }
        }
    }

    public void WinGame()
    {
        state = STATE_WIN;
    }

    public void LoseGame()
    {
        state = STATE_LOSE;
    }
}
