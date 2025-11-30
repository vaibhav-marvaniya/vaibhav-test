using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    public RectTransform GameplayCanvas;
    public int RowCount, ColCount;
    public GameObject CardPrefab;
    public Transform CardParent;
    public GridLayoutGroup CardGrid;
    public Vector2 CardOrigVector;
    public Vector2 UnusedAreaVector;
    [SerializeField]
    public Vector2 RequiredCardVector, CardOrigSpace;
    [SerializeField]
    List<CardScript> AllCardData = new List<CardScript>();
    [SerializeField]
    List<int> CardData = new List<int>();

    void Start()
    {
        CardOrigVector = CardPrefab.GetComponent<RectTransform>().sizeDelta;
      
        Debug.Log(GameplayCanvas.rect.size);
        Debug.Log(CardParent.transform.GetComponent<RectTransform>().rect.size);

        //CardGrid.cellSize = new Vector2(240, 320);
        //CardGrid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        //CardGrid.constraintCount = 5;
        //CardGrid.spacing = new Vector2(100, 100);
        FindCardUnusedArea();
    }

    void FindCardUnusedArea()
    {
        RowCount = GameManager.instance.RowCount;
        ColCount = GameManager.instance.ColCount;
        Vector2 AreaforCards = new Vector2(CardParent.transform.GetComponent<RectTransform>().rect.width - CardGrid.padding.right - CardGrid.padding.left - (CardGrid.spacing.x * (ColCount - 1)),
            CardParent.transform.GetComponent<RectTransform>().rect.height - CardGrid.padding.top - CardGrid.padding.bottom - (CardGrid.spacing.y * (RowCount - 1)));
        CardOrigSpace.x = CardOrigVector.x * ColCount;
        CardOrigSpace.y = CardOrigVector.y * RowCount;
        Debug.Log(AreaforCards + " " + CardOrigSpace);

        if (CardOrigSpace.x - AreaforCards.x > CardOrigSpace.y - AreaforCards.y)
        {
            RequiredCardVector.x = AreaforCards.x / ColCount;
            RequiredCardVector.x = Mathf.FloorToInt(RequiredCardVector.x);
            RequiredCardVector.y = (RequiredCardVector.x * CardOrigVector.y) / CardOrigVector.x;
            Debug.Log(RequiredCardVector.x + " "+ RequiredCardVector.y);
        }
        else if (CardOrigSpace.y - AreaforCards.y > CardOrigSpace.x - AreaforCards.x)
        {
            RequiredCardVector.y = AreaforCards.y / RowCount;
            RequiredCardVector.y = Mathf.FloorToInt(RequiredCardVector.y);
            RequiredCardVector.x = (RequiredCardVector.y * CardOrigVector.x) / CardOrigVector.y;
            Debug.Log(RequiredCardVector.x + " " + RequiredCardVector.y);
        }
        CardGrid.cellSize = RequiredCardVector;

        CardGrid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        CardGrid.constraintCount = ColCount;

        GenerateGrid();
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
                go.GetComponent<CardScript>().SetCardData(i, j, CardData[(i * ColCount) + j]);
                AllCardData.Add(go.GetComponent<CardScript>());
            }
        }
    }

    public void RemoveMatchedCard(CardScript card)
    {
        if(AllCardData.Contains(card))
            AllCardData.Remove(card);

        if(AllCardData.Count == 0)
        {
            Invoke(nameof(PlayGameoverClip), GameManager.instance.CardFliptime);
        }
    }

    void PlayGameoverClip()
    {
        AudioManager.instance.PlayGameoverClip();
    }
}
