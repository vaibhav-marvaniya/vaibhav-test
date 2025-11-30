using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int RowCount, ColCount;
    public float CardFliptime, CardFrontTime;
    [SerializeField]
    int PlayerTurns;
    public List<CardScript> ClickedCards = new List<CardScript>();

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
    }

    public void OnCardClick(CardScript cardScript)
    {
        ClickedCards.Add(cardScript);
        if(ClickedCards.Count % 2 == 0)
        {
            PlayerTurns++;
            CheckforPairMatch();
        }    
    }

    void CheckforPairMatch()
    {
        Debug.Log("CheckforPairMatch");
        for (int i = 0; i < ClickedCards.Count / 2; i++)
        {
            Debug.Log(ClickedCards[(i * 2)].CardDataNum + " "+ ClickedCards[(i * 2) + 1].CardDataNum);
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
        Debug.Log("Matched");
        RemoveCardfromClickedCard(card1);
        RemoveCardfromClickedCard(card2);
        card1.CardMatched(card2);
        card2.CardMatched(card1);
    }

    void CardUnmatched(CardScript card1, CardScript card2)
    {
        Debug.Log("Unmatched");
        RemoveCardfromClickedCard(card1);
        RemoveCardfromClickedCard(card2);
        card1.CardUnmatched(card2);
        card2.CardUnmatched(card1);
    }

    public void RemoveCardfromClickedCard(CardScript cardScript)
    {
        if(ClickedCards.Contains(cardScript))
            ClickedCards.Remove(cardScript);
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
