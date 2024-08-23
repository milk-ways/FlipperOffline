using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Fusion;
using UnityEngine.UI;
using JetBrains.Annotations;

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

    private void Start()
    {
        StartCoroutine(RateOscilate());
    }

    public void GameOver(int red, int blue, bool redWin)
    {
        result.gameObject.SetActive(true);

        if (redWin)
        {
            panResultText.color = Color.red;
            panResultText.text = $"<color=red>{red} <color=black>vs <color=blue>{blue}\n\n<color=red>»¡°­ÆÀ ½Â¸®!";
        }
        else
        {
            panResultText.color = Color.blue;
            panResultText.text = $"<color=red>{red} <color=black>vs <color=blue>{blue}\n\n<color=blue>ÆÄ¶ûÆÀ ½Â¸®!";
        }

        NetworkRunner runner = NetworkRunnerHandler.Instance.networkRunner;
        Team team = runner.GetPlayerObject(runner.LocalPlayer).GetComponent<Character>().team;
        if(team == Team.red)
        {
            titleImages.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            titleImages.GetChild(0).gameObject.SetActive(true);
        }

        if(team == Team.red ^ redWin)
        {
            resultText.text = "ÆÐ¹è";
        }
        else
        {
            resultText.text = "½Â¸®";
        }   
    }

    public IEnumerator ShowReadyText(int time)
    {
        alertText.gameObject.SetActive(true);
        float timer = time;
        while(timer > 0 && GameManager.Instance.WaitingForStart)
        {
            alertText.text = $"{Mathf.Ceil(timer)}ÃÊ ÈÄ °ÔÀÓÀÌ ½ÃÀÛµË´Ï´Ù...";
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

    private IEnumerator RateOscilate()
    {
        while(true)
        {

            if(Time.frameCount % 2 != 0) panSlider.value = panRate + Random.Range(-0.007f, 0.007f);
            yield return null;
        }

    }
}