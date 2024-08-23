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
    public TextMeshProUGUI resultText;

    public RectTransform timerRect;

    public Slider panSlider;

    private float time;

    private bool gameOver = true;
    private float panRate = .5f;

    public void TimerStart()
    {
        
        time = GameManager.Instance.GameTime;
        timerRect.gameObject.SetActive(true);
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

    public void GameOver(int red, int blue, bool redWin)
    {
        result.gameObject.SetActive(true);

        if (redWin)
        {
            titleImages.GetChild(1).gameObject.SetActive(true);
            panResultText.color = Color.red;
            panResultText.text = $"<color=red>{red} <color=black>vs <color=blue>{blue}\n\n<color=red>ª°∞≠∆¿ Ω¬∏Æ!";
        }
        else
        {
            titleImages.GetChild(0).gameObject.SetActive(true);
            panResultText.color = Color.blue;
            panResultText.text = $"<color=red>{red} <color=black>vs <color=blue>{blue}\n\n<color=blue>∆ƒ∂˚∆¿ Ω¬∏Æ!";
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
        panRate = value;
    }

    public void ReturnToHome()
    {
        SoundManager.StopAll();
        NetworkRunnerHandler.Instance.networkRunner.Shutdown();
    }
}