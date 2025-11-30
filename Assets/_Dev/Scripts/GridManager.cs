using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [SerializeField]
    int RowCount, ColCount;
    public GameObject CardPrefab;
    public Transform CardParent;
    public GridLayoutGroup CardGrid;
    [SerializeField]
    Vector2 CardOrigVector, UnusedAreaVector;
    [SerializeField]
    public Vector2 RequiredCardVector, CardOrigSpace;

    [SerializeField]
    List<CardScript> AllCardData = new List<CardScript>();

    [SerializeField]
    List<int> CardData = new List<int>();

    void Start()
    {
        CardOrigVector = CardPrefab.GetComponent<RectTransform>().sizeDelta;
    }

    public void StarGridGeneration()
    {
        FindCardUnusedArea(null);
    }

    public void StarGridGenerationFromSave(List<SaveLoadManager.CardSaveData> savedCards)
    {
        FindCardUnusedArea(savedCards);
    }

    void FindCardUnusedArea(List<SaveLoadManager.CardSaveData> savedCards)
    {
        RowCount = GameManager.instance.RowCount;
        ColCount = GameManager.instance.ColCount;

        Vector2 areaForCards = new Vector2(
            CardParent.transform.GetComponent<RectTransform>().rect.width
                - CardGrid.padding.right
                - CardGrid.padding.left
                - (CardGrid.spacing.x * (ColCount - 1)),
            CardParent.transform.GetComponent<RectTransform>().rect.height
                - CardGrid.padding.top
                - CardGrid.padding.bottom
                - (CardGrid.spacing.y * (RowCount - 1))
        );

        CardOrigSpace.x = CardOrigVector.x * ColCount;
        CardOrigSpace.y = CardOrigVector.y * RowCount;

        if (CardOrigSpace.x - areaForCards.x > CardOrigSpace.y - areaForCards.y)
        {
            RequiredCardVector.x = areaForCards.x / ColCount;
            RequiredCardVector.x = Mathf.FloorToInt(RequiredCardVector.x);
            RequiredCardVector.y =
                (RequiredCardVector.x * CardOrigVector.y) / CardOrigVector.x;
        }
        else if (CardOrigSpace.y - areaForCards.y > CardOrigSpace.x - areaForCards.x)
        {
            RequiredCardVector.y = areaForCards.y / RowCount;
            RequiredCardVector.y = Mathf.FloorToInt(RequiredCardVector.y);
            RequiredCardVector.x =
                (RequiredCardVector.y * CardOrigVector.x) / CardOrigVector.y;
        }

        CardGrid.cellSize = RequiredCardVector;
        CardGrid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        CardGrid.constraintCount = ColCount;

        if (savedCards == null)
            GenerateGrid();
        else
            GenerateGridFromSave(savedCards);
    }

    void GenerateGrid()
    {
        CardData.Clear();
        for (int i = 0; i < RowCount * ColCount; i++)
        {
            CardData.Add((i / 2) + 1);
        }

        GameManager.Shuffle(CardData);

        AllCardData.Clear();
        GameObject go;

        for (int i = 0; i < RowCount; i++)
        {
            for (int j = 0; j < ColCount; j++)
            {
                go = Instantiate(CardPrefab, CardParent);
                CardScript card = go.GetComponent<CardScript>();
                card.SetCardData(i, j, CardData[(i * ColCount) + j]);
                AllCardData.Add(card);
            }
        }
    }

    void GenerateGridFromSave(List<SaveLoadManager.CardSaveData> savedCards)
    {
        AllCardData.Clear();
        CardData.Clear();
        GameObject go;

        int totalCards = RowCount * ColCount;
        if (savedCards == null || savedCards.Count != totalCards)
        {
            GenerateGrid();
            return;
        }

        for (int i = 0; i < RowCount; i++)
        {
            for (int j = 0; j < ColCount; j++)
            {
                int index = (i * ColCount) + j;
                go = Instantiate(CardPrefab, CardParent);
                CardScript card = go.GetComponent<CardScript>();

                int num = savedCards[index].CardDataNum;
                card.SetCardData(i, j, num);

                if (savedCards[index].IsMatched)
                {
                    card.ApplyLoadedMatchedState();
                }

                CardData.Add(num);
                AllCardData.Add(card);
            }
        }
    }

    public List<CardScript> ReturnCardScriptData()
    {
        return AllCardData;
    }
}
