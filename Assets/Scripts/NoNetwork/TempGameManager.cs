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
            // �ν��Ͻ��� ���� ��쿡 �����Ϸ� �ϸ� �ν��Ͻ��� �Ҵ����ش�.
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
        // �ν��Ͻ��� �����ϴ� ��� ���λ���� �ν��Ͻ��� �����Ѵ�.
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
        // �Ʒ��� �Լ��� ����Ͽ� ���� ��ȯ�Ǵ��� ����Ǿ��� �ν��Ͻ��� �ı����� �ʴ´�.
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
