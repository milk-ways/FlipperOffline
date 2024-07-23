using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
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
        Camera.main.transform.position += Vector3.right * (row / 2) * 1.5f;
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
        temp.GetComponent<Character>().joystick = joy;
    }
}
