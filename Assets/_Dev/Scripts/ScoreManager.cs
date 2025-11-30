using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text ScoreText, ComboText, Turntext;
    [SerializeField]
    int CurrentScore, MatchPoints, UnmatchPoints, ComboCount, PlayerTurnCount;

    public void UpdateScore()
    {
        ScoreText.text = CurrentScore.ToString();
    }

    public void MatchScore()
    {
        CurrentScore += MatchPoints * ComboCount;
        UpdateScore();
    }

    public void UnmatchScrore()
    {
        CurrentScore -= UnmatchPoints;
        UpdateScore();
    }

    public void UpdateCombo(bool reset = false)
    {
        if (reset)
            ComboCount = 0;
        else
            ComboCount++;
        ComboText.text = ComboCount.ToString() + "x";
    }

    public void UpdateTurns()
    {
        PlayerTurnCount++;
        Turntext.text = PlayerTurnCount.ToString();
    }
}
