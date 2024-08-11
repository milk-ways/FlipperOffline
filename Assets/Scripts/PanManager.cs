using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanManager : MonoBehaviour
{
    private List<Pan> panGroup = new List<Pan>();

    [SerializeField]
    private GameObject pan;
    [SerializeField]
    private GameObject plane;

    private int redPanCount;
    private int bluePanCount;

    public float blank = 1.5f;

    public int RedPanCount { get { return redPanCount; } }
    public int BluePanCount { get { return bluePanCount; } }

    public bool IsRedWin { get { return redPanCount > bluePanCount; } }

    private void Start()
    {
        //GameManager.Instance.OnGameStart += GeneratePans;
        TempGameManager.Instance.OnGameStart += NoNetworkGeneratePans;
    }


    public void GeneratePans()
    {
        int row = GameManager.Instance.Row;
        int col = GameManager.Instance.Col;
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                var temp = NetworkRunnerHandler.Instance.networkRunner.Spawn(pan, new Vector3(1.5f * j, 0, 1.5f * i));
                temp.transform.parent = gameObject.transform;
                panGroup.Add(temp.GetComponent<Pan>());
            }
        }
    }

    public void NoNetworkGeneratePans()
    {
        int row = TempGameManager.Instance.Row;
        int col = TempGameManager.Instance.Col;
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                var temp = Instantiate(pan);
                temp.transform.parent = gameObject.transform;
                temp.gameObject.transform.position = new Vector3(blank * j, 0, blank * i);
                panGroup.Add(temp.GetComponent<Pan>());
            }
        }

        plane.transform.localScale = new Vector3(blank * 0.1f * col, 1f, blank * 0.1f * row);
        plane.transform.position = new Vector3(blank * (col / 2), 0f, blank * (row / 2));
    }

    public void CountPanColor()
    {
        redPanCount = 0;
        bluePanCount = 0;

        foreach (var pan in panGroup)
        {
            if (pan.isFlipped)
            {
                redPanCount++;
            }
            else
            {
                bluePanCount++;
            }
        }
    }

    public void FlipPan(int index)
    {
        panGroup[index].Flip();
    }
}
