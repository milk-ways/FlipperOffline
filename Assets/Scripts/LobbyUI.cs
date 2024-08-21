using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] Button SingleMatchButton;
    [SerializeField] Button MultiMatchButton;
    [SerializeField] Button previousButton;
    [SerializeField] Button nextButton;

    private int current = 0;
    private int previous = 0;

    public Transform rootPlayer;
    public Transform[] players;

    private void Start()
    {
        SingleMatchButton.onClick.AddListener(() =>
        {
            NetworkRunnerHandler.Instance.SelectedPlayer = current;
            NetworkRunnerHandler.Instance.FindOneVsOneMatch();
        });

        MultiMatchButton.onClick.AddListener(() =>
        {
            NetworkRunnerHandler.Instance.SelectedPlayer = current;
            NetworkRunnerHandler.Instance.FindThreeVsThreeMatch();
        });

        previousButton.onClick.AddListener(PreviousPlayer);
        nextButton.onClick.AddListener(NextPlayer);

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
