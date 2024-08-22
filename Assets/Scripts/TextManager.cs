using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Fusion;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    public bool isStarted { get; set; } = false;

    public TextMeshProUGUI timeText;
    public TextMeshProUGUI alertText;
    public GameObject result;
    public Transform titleImages;
    public TextMeshProUGUI panResultText;

    public Slider panSlider;

    private float time;

    private bool gameOver = true;

    public void TimerStart()
    {
        time = GameManager.Instance.GameTime;
        timeText.gameObject.SetActive(true);
        panSlider.gameObject.SetActive(true);
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
        result.gameObject.SetActive(true);

        if (redWin)
        {
            titleImages.GetChild(1).gameObject.SetActive(true);
            panResultText.color = Color.red;
            panResultText.text = str + "\nª°∞≠∆¿ Ω¬∏Æ!";
        }
        else
        {
            titleImages.GetChild(0).gameObject.SetActive(true);
            panResultText.color = Color.blue;
            panResultText.text = str + "\n∆ƒ∂˚∆¿ Ω¬∏Æ!";
        }
    }

    public IEnumerator ShowReadyText(int time)
    {
        alertText.gameObject.SetActive(true);
        float timer = time;
        while(timer > 0 && GameManager.Instance.WaitingForStart)
        {
            alertText.text = $"{Mathf.Ceil(timer)}√  »ƒ ∞‘¿”¿Ã Ω√¿€µÀ¥œ¥Ÿ...";
            timer -= Time.deltaTime;
            yield return null;
        }
        alertText.gameObject.SetActive(false);
    }

    public void SetPanRate(float value)
    {
        panSlider.value = value;
    }

    public void ReturnToHome()
    {
        NetworkRunnerHandler.Instance.networkRunner.Shutdown();
    }
}