using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;
using System;

public class TempGameManager : MonoBehaviour
{
    private static TempGameManager _instance;

    public PanManager panManager;
    public TextManager textManager;

    public GameObject character;

    public VariableJoystick joy;
    public Button btn;

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

    public static TempGameManager Instance
    {
        get
        {
            // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(TempGameManager)) as TempGameManager;

                if (_instance == null)
                    Debug.Log("no Singleton obj");
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        // 인스턴스가 존재하는 경우 새로생기는 인스턴스를 삭제한다.
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        // 아래의 함수를 사용하여 씬이 전환되더라도 선언되었던 인스턴스가 파괴되지 않는다.
        //DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Invoke("GameStart", 5);
    }

    public void GameStart()
    {
        OnGameStart?.Invoke();
        
        GenerateCharacter();
        Camera.main.GetComponent<CameraController>().SetCameraBoundary();
    }

    public void GameEnd() 
    {
        panManager.CountPanColor();

        string str = panManager.RedPanCount.ToString() + " : " + panManager.BluePanCount.ToString();
        textManager.GameOver(str, panManager.IsRedWin);
    }

    private void GenerateCharacter()
    {
        var localCharacter = Instantiate(character);

        if (localCharacter.GetComponent<Character>().team == 0)
        {
            localCharacter.transform.position = new Vector3((panManager.blank * (col / 2)), 4.5f, 0f);
            localCharacter.GetComponent<MeshRenderer>().material.color = Color.blue;
        }
        else
        {
            localCharacter.transform.position = new Vector3((panManager.blank * (col / 2)), 4.5f, panManager.blank * (row-1));
            localCharacter.GetComponent<MeshRenderer>().material.color = Color.red;
        }

        localCharacter.GetComponent<Character>().AssignUI(joy, btn);

        Camera.main.GetComponent<CameraController>().Target = localCharacter.gameObject;
    }
}
