using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelect : MonoBehaviour
{
    public int current = 0;
    public int previous = 0;

    public Transform rootPlayer;

    public Transform[] players;


    private void Start()
    {
        players = new Transform[rootPlayer.childCount];

        for (int i = 0; i < rootPlayer.childCount; i++)
        {
            players[i] = rootPlayer.GetChild(i);
        }

        players[current].gameObject.SetActive(true);
    }

    public void NextPlayer()
    {
        players[current].gameObject.SetActive(false);

        previous = current;
        current = (current + 1) % rootPlayer.childCount;

        players[current].gameObject.SetActive(true);
    }

    public void PreviousPlayer()
    {
        players[current].gameObject.SetActive(false);

        current = previous;
        previous = previous == 0 ? rootPlayer.childCount - 1 : previous - 1;

        players[current].gameObject.SetActive(true);
    }
}
