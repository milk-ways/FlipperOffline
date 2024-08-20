using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Fusion;

public class TextManager : MonoBehaviour
{
    public bool isStarted { get; set; } = false;

    public TextMeshProUGUI timeText;
    public TextMeshProUGUI resultText;

    private float time;

    private bool gameOver = true;

    private void Start()
    {
        //GameManager.Instance.OnGameStart += () => isStarted = !isStarted;
        //TempGameManager.Instance.OnGameStart += NoNetworkTimerStart;
    }

    public void TimerStart()
    {
        time = GameManager.Instance.GameTime;
        timeText.gameObject.SetActive(true);
        gameOver = false;
    }

    public void NoNetworkTimerStart()
    {
        time = TempGameManager.Instance.GameTime;
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
                GameManager.Instance?.GameEnd();
                TempGameManager.Instance?.GameEnd();
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

    public IEnumerator ShowReadyText(int time)
    {
        resultText.gameObject.SetActive(true);
        float timer = time;
        while(timer > 0 && GameManager.Instance.WaitingForStart)
        {
            resultText.text = $"Game Starts in {Mathf.Ceil(timer)}...";
            timer -= Time.deltaTime;
            yield return null;
        }
        resultText.gameObject.SetActive(false);
    }
}