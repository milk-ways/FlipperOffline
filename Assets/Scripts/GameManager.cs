using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;
using System;

public class GameManager : NetworkBehaviour, ISpawned
{
    private static GameManager _instance;

    public PanManager panManager;
    public TextManager textManager;

    [SerializeField]
    private int row;
    [SerializeField]
    private int col;
    [SerializeField]
    private int gameTime;

    public int Row { get { return row; } }
    public int Col { get { return col; } }
    public int GameTime => gameTime;
    //temp code
    public VariableJoystick joy;
    public Button btn;

    public Action OnGameStart;

    public bool WaitingForStart { get; set; } = false;

    [SerializeField] private int delayBeforeStart = 5;

    public static GameManager Instance
    {
        get
        {
            // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }

    public override void Spawned()
    {
        panManager = FindObjectOfType<PanManager>();
        textManager = FindObjectOfType<TextManager>();
        joy = FindObjectOfType<VariableJoystick>();

        OnGameStart += panManager.GeneratePans;
        OnGameStart += () => textManager.isStarted = !textManager.isStarted;
    }

    public void GameStart()
    {
        SetCharacterPos();
        if (NetworkRunnerHandler.Instance.networkRunner.IsSharedModeMasterClient)
        {
            OnGameStart?.Invoke();
        }   
    }

    public void GameEnd() 
    {
        panManager.CountPanColor();

        string str = panManager.RedPanCount.ToString() + " : " + panManager.BluePanCount.ToString();
        textManager.GameOver(str, panManager.IsRedWin);
    }

    private void SetCharacterPos()
    {
        var localCharacter = NetworkRunnerHandler.Instance.LocalCharacter;

        if (localCharacter == null) return;
        if (localCharacter.GetComponent<Character>().team == 0)
        {
            localCharacter.transform.position = new Vector3((panManager.blank * (col / 2)), 4.5f, 0f);
            localCharacter.GetComponent<MeshRenderer>().material.color = Color.blue;
        }
        else
        {
            localCharacter.transform.position = new Vector3((panManager.blank * (col / 2)), 4.5f, panManager.blank * (row - 1));
            localCharacter.GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }
}
