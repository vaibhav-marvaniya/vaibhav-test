using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int RowCount, ColCount;
    public float CardFliptime, CardFrontTime;
    [SerializeField]
    List<CardScript> ClickedCards = new List<CardScript>();

    public GridManager GridManager;
    public ScoreManager ScoreManager;
    public SaveLoadManager SaveLoadManager;
    public Button RestartBtn;
    public Text WarningTxt;

    public static GameManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }
    private static GameManager _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        ClickedCards.Clear();

        if (SaveLoadManager == null)
            SaveLoadManager = FindObjectOfType<SaveLoadManager>();

        RestartBtn.onClick.AddListener(RestartGame);
    }

    private void Start()
    {
        if (SaveLoadManager == null)
            SaveLoadManager = FindObjectOfType<SaveLoadManager>();

        if (SaveLoadManager != null && SaveLoadManager.CheckifhaveSavedData())
        {
            var levelData = SaveLoadManager.LoadData();
            if (levelData != null)
            {
                int savedTotalCards = levelData.RowCount * levelData.ColCount;
                if (savedTotalCards % 2 != 0)
                {
                    WarningTxt.text = "Invalid grid size.\nTotal number of cards must be even for pairs";
                    SaveLoadManager.ClearSave();
                }
                else
                {
                    RowCount = levelData.RowCount;
                    ColCount = levelData.ColCount;

                    ScoreManager.ApplyLoadedValues(
                        levelData.CurrentScore,
                        levelData.ComboCount,
                        levelData.PlayerTurnCount,
                        levelData.MatchedCardCount
                    );

                    GridManager.StarGridGenerationFromSave(levelData.AllCardData);

                    CheckForWin();
                    return;
                }
            }
        }
        int totalCards = RowCount * ColCount;
        if (totalCards % 2 != 0)
        {
            WarningTxt.text = "Invalid grid size.\nTotal number of cards must be even for pairs";
            return;
        }
        WarningTxt.text = "";
        GridManager.StarGridGeneration();
        CheckForWin();
    }

    void RestartGame()
    {
        if (SaveLoadManager == null)
            SaveLoadManager = FindObjectOfType<SaveLoadManager>();

        if (SaveLoadManager != null)
            SaveLoadManager.ClearSave();

        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void OnCardClick(CardScript cardScript)
    {
        ClickedCards.Add(cardScript);
        if (ClickedCards.Count % 2 == 0)
        {
            CheckforPairMatch();
        }
    }

    void CheckforPairMatch()
    {
        for (int i = 0; i < ClickedCards.Count / 2; i++)
        {
            if (ClickedCards[(i * 2)].CardDataNum == ClickedCards[(i * 2) + 1].CardDataNum)
            {
                CardMatched(ClickedCards[(i * 2)], ClickedCards[(i * 2) + 1]);
            }
            else
            {
                CardUnmatched(ClickedCards[(i * 2)], ClickedCards[(i * 2) + 1]);
            }
        }
    }

    void CardMatched(CardScript card1, CardScript card2)
    {
        RemoveCardfromClickedCard(card1);
        RemoveCardfromClickedCard(card2);

        card1.CardMatched(card2);
        card2.CardMatched(card1);

        ScoreManager.UpdateTurns();
        ScoreManager.UpdateCombo();
        ScoreManager.MatchScore();
        ScoreManager.UpdateMatchedCardCount();

        AudioManager.instance.PlayMatchFlip();

        CheckForWin();
    }

    void CardUnmatched(CardScript card1, CardScript card2)
    {
        RemoveCardfromClickedCard(card1);
        RemoveCardfromClickedCard(card2);

        card1.CardUnmatched(card2);
        card2.CardUnmatched(card1);

        ScoreManager.UpdateTurns();
        ScoreManager.UpdateCombo(true);
        ScoreManager.UnmatchScrore();

        AudioManager.instance.PlayMismatchFlip();
    }

    public void RemoveCardfromClickedCard(CardScript cardScript, bool isRevert = false)
    {
        if (isRevert)
        {
            if (ClickedCards.Count == 1)
            {
                if (ClickedCards[0].CardDataNum == cardScript.CardDataNum)
                {
                    ScoreManager.UpdateTurns();
                }
            }
        }

        if (ClickedCards.Contains(cardScript))
            ClickedCards.Remove(cardScript);
    }

    void CheckForWin()
    {
        if (ScoreManager.ReturnMatchedCardount() >= RowCount * ColCount)
        {
            WarningTxt.text = "You Won";
            if (SaveLoadManager != null)
                SaveLoadManager.ClearSave();
            AudioManager.instance.PlayGameoverClip();
        }
    }

    public static void Shuffle<T>(IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
