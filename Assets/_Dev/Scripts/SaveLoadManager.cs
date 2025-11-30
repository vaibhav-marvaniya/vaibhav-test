using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public const string LastSaveData = "lastsaveadata";

    private void OnApplicationPause(bool pause)
    {
        if (pause)
            SaveData();
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }

    public void SaveData()
    {
        if (GameManager.instance == null || GameManager.instance.GridManager == null)
            return;

        LevelData levelData = new LevelData
        {
            RowCount = GameManager.instance.RowCount,
            ColCount = GameManager.instance.ColCount,
            CurrentScore = GameManager.instance.ScoreManager.ReturnCurrentScore(),
            ComboCount = GameManager.instance.ScoreManager.ReturnComboCount(),
            PlayerTurnCount = GameManager.instance.ScoreManager.ReturnTurnCount(),
            MatchedCardCount = GameManager.instance.ScoreManager.ReturnMatchedCardount()
        };


        if (levelData.RowCount * levelData.ColCount > 0 && levelData.MatchedCardCount >= levelData.RowCount * levelData.ColCount)
        {
            ClearSave();
            return;
        }
        List<CardScript> cards = GameManager.instance.GridManager.ReturnCardScriptData();
        levelData.AllCardData = new List<CardSaveData>(cards.Count);

        foreach (var card in cards)
        {
            CardSaveData data = new CardSaveData
            {
                CardDataNum = card.CardDataNum,
                IsMatched = card.isCardMatched
            };
            levelData.AllCardData.Add(data);
        }

        string json = JsonUtility.ToJson(levelData);
        PlayerPrefs.SetString(LastSaveData, json);
        PlayerPrefs.Save();
    }

    public LevelData LoadData()
    {
        if (!PlayerPrefs.HasKey(LastSaveData))
            return null;

        string json = PlayerPrefs.GetString(LastSaveData);
        LevelData levelData = JsonUtility.FromJson<LevelData>(json);
        return levelData;
    }

    public bool CheckifhaveSavedData()
    {
        return PlayerPrefs.HasKey(LastSaveData);
    }

    public void ClearSave()
    {
        if (PlayerPrefs.HasKey(LastSaveData))
            PlayerPrefs.DeleteKey(LastSaveData);
    }

    [System.Serializable]
    public class LevelData
    {
        public int RowCount, ColCount;
        public int CurrentScore, ComboCount, PlayerTurnCount, MatchedCardCount;
        public List<CardSaveData> AllCardData;
    }

    [System.Serializable]
    public class CardSaveData
    {
        public int CardDataNum;
        public bool IsMatched;
    }
}
