using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

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

    private int current = 0;
    private int previous = 0;

    public Transform rootPlayer;
    public Transform[] players;

    private void Start()
    {
        SingleMatchButton.onClick.AddListener(() =>
        {
            StartCoroutine(WatingTextRoutine());
            MatchmakingWaitingPanel.gameObject.SetActive(true);
            NetworkRunnerHandler.Instance.SelectedPlayer = current;
            NetworkRunnerHandler.Instance.FindOneVsOneMatch();
        });

        MultiMatchButton.onClick.AddListener(() =>
        {
            StartCoroutine(WatingTextRoutine());
            MatchmakingWaitingPanel.gameObject.SetActive(true);
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

        SetText();
        players[current].gameObject.SetActive(true);
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
        characterName.text = players[current].name;

        description.text = players[current].GetComponent<Character>().description;
    }

    private float duration = 0.2f;
    private int len = 1;
    private IEnumerator WatingTextRoutine()
    {
        StringBuilder builder = new();
        builder.Append("技记 立加 吝....");

        int baseLen = "技记 立加 吝".Length;
        while(true)
        {
            waitingText.text = builder.ToString(0, baseLen + len);
            yield return new WaitForSeconds(duration);
            len = (len + 1) % 4;
        }
    }
}
