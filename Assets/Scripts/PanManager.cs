using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanManager : MonoBehaviour
{
    private List<Pan> panGroup = new List<Pan>();

    [SerializeField]
    private GameObject pan;

    private int redPanCount;
    private int bluePanCount;

    public int RedPanCount { get { return redPanCount; } }
    public int BluePanCount { get { return bluePanCount; } }

    public bool IsRedWin { get { return redPanCount > bluePanCount; } }

    private void Start()
    {
        GameManager.Instance.OnGameStart += GeneratePans;
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
