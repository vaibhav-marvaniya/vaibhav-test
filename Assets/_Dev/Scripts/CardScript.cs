using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardScript : MonoBehaviour
{
    public int CardX, CardY;
    public RectTransform CardRect;
    public Button CardBtn;
    public int CardDataNum;
    public Text CardDataText;

    private void Awake()
    {
        CardBtn.onClick.AddListener(OnCardClick);
    }

    public void SetCardData(int x, int y, int num)
    {
        CardX = x;
        CardY = y;
        CardDataText.text = num.ToString(); 
    }

    void OnCardClick()
    {

    }
}
