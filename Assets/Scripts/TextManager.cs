using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextManager : MonoBehaviour
{
    
    public TextMeshProUGUI timeText;
    
    public TextMeshProUGUI resultText;

    private float time;

    private bool gameOver = true;

    private void Start()
    {
        GameManager.Instance.OnGameStart += TimerStart;
    }

    public void TimerStart()
    {
        time = GameManager.Instance.GameTime;
        timeText.gameObject.SetActive(true);
        gameOver = false;
    }

    private void Update()
    {
        if (time > 0)
        {
            time -= Time.deltaTime;
        }
        else
        {
            if (!gameOver)
            {
                GameManager.Instance.GameEnd();
                gameOver = true;
            }
        }

        timeText.text = Mathf.Ceil(time).ToString();
    }

    public void GameOver(string str, bool redWin)
    {
        resultText.gameObject.SetActive(true);

        if (redWin)
        {
            resultText.color = Color.red;
            resultText.text = str + "\nRED WIN!";
        }
        else
        {
            resultText.color = Color.blue;
            resultText.text = str + "\nBLUE WIN!";
        }
    }
}