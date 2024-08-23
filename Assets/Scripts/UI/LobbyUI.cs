using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;
using System;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] Button SingleMatchButton;
    [SerializeField] Button MultiMatchButton;
    [SerializeField] Button previousButton;
    [SerializeField] Button nextButton;
    [SerializeField] TextMeshProUGUI characterName;
    [SerializeField] TextMeshProUGUI description;

    [SerializeField] RectTransform MatchmakingWaitingPanel;
    [SerializeField] TextMeshProUGUI waitingText;

    [SerializeField] CharacterDesc characterDesc;

    [SerializeField] Button QuitButton;
    [SerializeField] Button SoundManageButton;

    private int current = 0;
    private int previous = 0;

    public Transform rootPlayer;
    public Transform[] players;

    private void Start()
    {
        SingleMatchButton.onClick.AddListener(() =>
        {
            MatchButton(NetworkRunnerHandler.Instance.FindOneVsOneMatch);
        });

        MultiMatchButton.onClick.AddListener(() =>
        {
            MatchButton(NetworkRunnerHandler.Instance.FindThreeVsThreeMatch);
        });

        QuitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });

        SoundManageButton.onClick.AddListener(() =>
        {

        });

        previousButton.onClick.AddListener(PreviousPlayer);
        nextButton.onClick.AddListener(NextPlayer);

        players = new Transform[rootPlayer.childCount];

        for (int i = 0; i < rootPlayer.childCount; i++)
        {
            players[i] = rootPlayer.GetChild(i);
        }

        SetText();
        players[current].gameObject.SetActive(true);
    }

    public void MatchButton(Action action)
    {
        StartCoroutine(WatingTextRoutine());
        MatchmakingWaitingPanel.gameObject.SetActive(true);
        NetworkRunnerHandler.Instance.SelectedPlayer = current;
        action?.Invoke();
    }

    public void NextPlayer()
    {
        players[current].gameObject.SetActive(false);

        previous = current;
        current = (current + 1) % rootPlayer.childCount;

        SetText();

        players[current].gameObject.SetActive(true);
    }

    public void PreviousPlayer()
    {
        players[current].gameObject.SetActive(false);

        current = previous;
        previous = previous == 0 ? rootPlayer.childCount - 1 : previous - 1;

        SetText();

        players[current].gameObject.SetActive(true);
    }

    public void SetText()
    {
        characterName.text = characterDesc.characterName[current];
        description.text = characterDesc.characterDesc[current];
    }

    private float duration = 0.2f;
    private int len = 1;
    private IEnumerator WatingTextRoutine()
    {
        StringBuilder builder = new();
        string tot = "技记 立加 吝....";
        int baseLen = "技记 立加 吝".Length;
        while(true)
        {
            waitingText.text = tot.Substring(0, baseLen + len);
            yield return new WaitForSeconds(duration);
            len = (len + 1) % 4;
        }
    }
}
