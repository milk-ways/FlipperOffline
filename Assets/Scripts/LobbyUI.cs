using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] Button SingleMatchButton;
    [SerializeField] Button MultiMatchButton;

    private void Start()
    {
        SingleMatchButton.onClick.AddListener(() =>
        {
            NetworkRunnerHandler.Instance.FindOneVsOneMatch();
        });

        MultiMatchButton.onClick.AddListener(() =>
        {
            NetworkRunnerHandler.Instance.FindThreeVsThreeMatch();
        });
    }
}
