using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab.ClientModels;

public class LeaderBoard : MonoBehaviour
{
    public Text Player1Name;
    public Text Player1Score;
    public Text Player2Name;
    public Text Player2Score;
    public Text Player3Name;
    public Text Player3Score;
    public Text Player4Name;
    public Text Player4Score;
    public Text Player5Name;
    public Text Player5Score;
    public Text Player6Name;
    public Text Player6Score;
    public Text Player7Name;
    public Text Player7Score;
    public Text Player8Name;
    public Text Player8Score;
    public Text Player9Name;
    public Text Player9Score;
    public Text Player10Name;
    public Text Player10Score;

    private List<Tuple<Text, Text>> leaderboardTextDisplays;
    // Use this for initialization
    void Start()
    {
        ScoreEvents.OnLeaderBoardRetrieved += PopulateLeaderBoard;
        leaderboardTextDisplays = new List<Tuple<Text, Text>>
        {
            new Tuple<Text, Text>(Player1Name, Player1Score),
            new Tuple<Text, Text>(Player2Name, Player2Score),
            new Tuple<Text, Text>(Player3Name, Player3Score),
            new Tuple<Text, Text>(Player4Name, Player4Score),
            new Tuple<Text, Text>(Player5Name, Player5Score),
            new Tuple<Text, Text>(Player6Name, Player6Score),
            new Tuple<Text, Text>(Player7Name, Player7Score),
            new Tuple<Text, Text>(Player8Name, Player8Score),
            new Tuple<Text, Text>(Player9Name, Player9Score),
            new Tuple<Text, Text>(Player10Name, Player10Score),
        };
        ScoreEvents.SendLoadLeaderBoard();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void PopulateLeaderBoard(List<PlayerLeaderboardEntry> leaderboardEntries)
    {
        string currentUserID = PlayerPrefs.GetString("");
        for (int i = 0; i < leaderboardTextDisplays.Count; ++i)
        {
            if (leaderboardEntries.Count <= i)
            {
                leaderboardTextDisplays[i].Item1.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                leaderboardTextDisplays[i].Item1.transform.parent.gameObject.SetActive(true);
                leaderboardTextDisplays[i].Item1.text = leaderboardEntries[i].PlayFabId;
                leaderboardTextDisplays[i].Item2.text = leaderboardEntries[i].StatValue.ToString();
            }
        }
    }
    private void OnDestroy()
    {
        ScoreEvents.OnLeaderBoardRetrieved -= PopulateLeaderBoard;
    }
}
