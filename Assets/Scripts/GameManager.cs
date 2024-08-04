using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fusion;

public class GameManager : NetworkBehaviour
{
    private static GameManager _instance;

    //public PanGroup panGroup;
    public PanManager panManager;
    public TextManager textManager;

    public GameObject character;

    [SerializeField]
    private int row;
    [SerializeField]
    private int col;
    [SerializeField]
    private int gameTime;

    public int Row { get { return row; } }
    public int Col { get { return col; } }

    //temp code
    public VariableJoystick joy;
    public Button btn;

    public static GameManager Instance
    {
        get
        {
            // �ν��Ͻ��� ���� ��쿡 �����Ϸ� �ϸ� �ν��Ͻ��� �Ҵ����ش�.
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(GameManager)) as GameManager;

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
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            GameStart();
        }
    }

    private void GameStart()
    {
        textManager.TimerStart(gameTime);
        panManager.GeneratePans(row, col);
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
        var temp = Instantiate(character);
        temp.transform.position = new Vector3((1.5f * (row / 2)), 0.75f, (1.5f * (col / 2)));
        //temp.GetComponent<Character>().joystick = joy;
        temp.GetComponent<Character>().AssignUI(joy, btn);

        Camera.main.GetComponent<CameraController>().Target = temp;
    }
}
