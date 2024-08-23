using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public Action OnGameStart;

    [Networked]
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

        OnGameStart += () => textManager.isStarted = !textManager.isStarted;

        NetworkRunnerHandler.Instance.networkRunner.SessionInfo.IsOpen = true;
    }

    public void GameStart()
    {   
        //Action should be done on every single client
        SetCharacterColor();
        SetCharacterPos();
        Camera.main.GetComponent<CameraController>().SetCameraBoundary();
        
        //Action should be done only on MasterClient
        if (NetworkRunnerHandler.Instance.networkRunner.IsSharedModeMasterClient)
        {
            NetworkRunnerHandler.Instance.networkRunner.SessionInfo.IsOpen = false;
            panManager.GeneratePans();
            RpcGameStart();
        }   
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RpcGameStart()
    {
        SoundManager.PlayBGM("ingame");
        textManager.TimerStart();
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RpcSetWaitingForStart(bool value)
    {
        WaitingForStart = value;
        if(value)
        {
            StartCoroutine(GameStartEnum());
        }
    }

    private IEnumerator GameStartEnum()
    {
        yield return StartCoroutine(textManager.ShowReadyText(delayBeforeStart));
        if (!WaitingForStart) yield break;
        GameStart();
    }

    public void GameEnd() 
    {
        if (!HasStateAuthority) return;

        panManager.CountPanColor();
        string str = panManager.RedPanCount.ToString() + " : " + panManager.BluePanCount.ToString();
        Debug.Log(str);
        RpcGameEnd(str, panManager.IsRedWin);
    }

    [Rpc]
    public void RpcGameEnd(string str, bool res)
    {
        SoundManager.PlayEffect("result");
        InputManager.Instance.Inactivate();
        textManager.GameOver(str, res);
    }

    private void SetCharacterPos()
    {
        var localCharacter = NetworkRunnerHandler.Instance.networkRunner.GetPlayerObject(NetworkRunnerHandler.Instance.networkRunner.LocalPlayer);

        if (localCharacter == null) return;
        if (localCharacter.GetComponent<Character>().team == 0)
        {
            localCharacter.transform.position = new Vector3(panManager.blank * UnityEngine.Random.Range(0, col - 1), 4.5f, panManager.blank * UnityEngine.Random.Range(0, row - 1));
        }
        else
        {
            localCharacter.transform.position = new Vector3(panManager.blank * UnityEngine.Random.Range(0, col - 1), 4.5f, panManager.blank * UnityEngine.Random.Range(0, row - 1));
        }
    }

    private void SetCharacterColor()
    {
        foreach(var item in Runner.ActivePlayers)
        {
            Debug.Log(item);
            var curCharacter = Runner.GetPlayerObject(item);
            if (curCharacter == null) return;
            curCharacter.GetComponent<Character>()?.SetTeamColor();
        }
    }
}
