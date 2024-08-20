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

        OnGameStart += panManager.GeneratePans;
        OnGameStart += () => textManager.isStarted = !textManager.isStarted;
    }


    public void GameStart() => RpcGameStart();
    [Rpc]
    public void RpcGameStart()
    {
        Debug.Log("GAME START");
        //Action should be done on every single client
        SetCharacterColor();
        Camera.main.GetComponent<CameraController>().SetCameraBoundary();

        //Action should be done only on MasterClient
        if (NetworkRunnerHandler.Instance.networkRunner.IsSharedModeMasterClient)
        {
            SetCharacterPos();
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
        var localCharacter = NetworkRunnerHandler.Instance.networkRunner.GetPlayerObject(NetworkRunnerHandler.Instance.networkRunner.LocalPlayer);

        if (localCharacter == null) return;
        if (localCharacter.GetComponent<Character>().team == 0)
        {
            localCharacter.transform.position = new Vector3((panManager.blank * (col / 2)), 4.5f, 0f);
        }
        else
        {
            localCharacter.transform.position = new Vector3((panManager.blank * (col / 2)), 4.5f, panManager.blank * (row - 1));
        }
    }

    private void SetCharacterColor()
    {
        foreach(var item in Runner.ActivePlayers)
        {
            Debug.Log(item);
            var curCharacter = Runner.GetPlayerObject(item);
            if (curCharacter == null) return;
            if (curCharacter.GetComponent<Character>().team == 0)
            {
                curCharacter.GetComponent<MeshRenderer>().material.color = Color.blue;
            }
            else
            {
                curCharacter.GetComponent<MeshRenderer>().material.color = Color.red;
            }
        }
    }
}
