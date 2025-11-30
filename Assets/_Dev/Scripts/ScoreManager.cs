using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text ScoreText, ComboText, Turntext;
    [SerializeField]
    int CurrentScore, MatchPoints, UnmatchPoints, ComboCount, PlayerTurnCount, MatchedCardCount;

    private void Start()
    {
        UpdateScore();
        UpdateComboTextOnly();
        UpdateTurnTextOnly();
    }

    public void UpdateScore()
    {
        if (ScoreText != null)
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

        UpdateComboTextOnly();
    }

    public void UpdateTurns()
    {
        PlayerTurnCount++;
        UpdateTurnTextOnly();
    }

    public void UpdateMatchedCardCount()
    {
        MatchedCardCount += 2;
    }

    public int ReturnCurrentScore()
    {
        return CurrentScore;
    }

    public int ReturnComboCount()
    {
        return ComboCount;
    }

    public int ReturnTurnCount()
    {
        return PlayerTurnCount;
    }

    public int ReturnMatchedCardount()
    {
        return MatchedCardCount;
    }

    public void ApplyLoadedValues(
        int savedScore,
        int savedCombo,
        int savedTurnCount,
        int savedMatchedCardCount
    )
    {
        CurrentScore = savedScore;
        ComboCount = savedCombo;
        PlayerTurnCount = savedTurnCount;
        MatchedCardCount = savedMatchedCardCount;

        UpdateScore();
        UpdateComboTextOnly();
        UpdateTurnTextOnly();
    }

    void UpdateComboTextOnly()
    {
        if (ComboText != null)
            ComboText.text = ComboCount.ToString() + "x";
    }

    void UpdateTurnTextOnly()
    {
        if (Turntext != null)
            Turntext.text = PlayerTurnCount.ToString();
    }
}
