using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PanManager : NetworkBehaviour, ISpawned
{ 
    private List<Pan> panGroup = new List<Pan>();

    [SerializeField]
    private GameObject pan;
    [SerializeField]
    private GameObject planePrefab;
    public NetworkObject plane;

    [Networked, OnChangedRender(nameof(RpcOnChangeCount))]
    public int redPanCount { get; set; } = 0;
    [Networked]
    public int bluePanCount { get; set; } = 0;

    public float blank = 1.5f;

    public int RedPanCount { get { return redPanCount; } }
    public int BluePanCount { get { return bluePanCount; } }

    public bool IsRedWin { get { return redPanCount > bluePanCount; } }

    public override void Spawned()
    {
        if (!HasStateAuthority) return;
        plane = NetworkRunnerHandler.Instance.networkRunner.Spawn(planePrefab, Vector3.zero);
    }

    public void GeneratePans()
    {
        int row = GameManager.Instance.Row;
        int col = GameManager.Instance.Col;
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                float randomOffset = Random.Range(0f, 1f);

                var temp = NetworkRunnerHandler.Instance.networkRunner.Spawn(pan, new Vector3(blank * j + Random.Range(-0.5f, 0.5f), 0, blank * i + Random.Range(-0.5f, 0.5f)));
                temp.GetComponent<Pan>().isFlipped = (randomOffset <= .5f);
                if (randomOffset <= .5f)
                { 
                    redPanCount++; 
                }
                else
                {
                    bluePanCount++;
                }
                panGroup.Add(temp.GetComponent<Pan>());
            }
        }

        RpcOnChangeCount();
        plane.transform.localScale = new Vector3(blank * 0.1f * col + 0.2f , 1f, blank * 0.1f * row + 0.2f);
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

    public void AddPanCount(bool isRed, bool isInitial = false)
    {
        if (isRed)
        {
            redPanCount++;
            if (!isInitial)
            {
                bluePanCount--;
            }
        }
        else
        {
            bluePanCount++;
            if(!isInitial)
            {
                redPanCount--;
            }
        }
    }

    public void FlipPan(int index)
    {
        panGroup[index].Flip();
    }

    [Rpc]
    public void RpcOnChangeCount()
    {
        GameManager.Instance.textManager.SetPanRate((float)BluePanCount / (RedPanCount + BluePanCount));
    }
}
