using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI resultText;
    private float time;

    private bool gameOver = false;

    private void Awake()
    {
        time = 5f;
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
                GameOver();
                gameOver = true;
            }
        }
        
        timeText.text = Mathf.Ceil(time).ToString();
    }

    private void GameOver()
    {
        resultText.gameObject.SetActive(true);
        resultText.text = GameManager.Instance.panGroup.CalcPan();
    }
}
