using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardScript : MonoBehaviour
{
    public int CardX, CardY;
    public int CardDataNum;
    public CardState CurrentCardState;

    public Sprite BackSprite, FrontSprite;
    public Text CardDataText;
    public Image CardImg;
    public Button CardBtn;

    private void Awake()
    {
        CardBtn.onClick.AddListener(OnCardClick);
    }

    public void SetCardData(int x, int y, int num)
    {
        CardX = x;
        CardY = y;
        CardDataNum = num;
        CardDataText.text = "";
        CurrentCardState = CardState.back;
        CardImg.sprite = BackSprite;
    }

    void OnCardClick()
    {
        Debug.Log("OnCardClick " + gameObject.name);
        Invoke(nameof(RevertCardtoBack), GameManager.instance.CardFrontTime);
        CurrentCardState = CardState.front;
        CardImg.sprite = FrontSprite;
        CardDataText.text = CardDataNum.ToString();
        GameManager.instance.OnCardClick(this);
    }

    void RevertCardtoBack()
    {
        GameManager.instance.RemoveCardfromClickedCard(this);
        CurrentCardState = CardState.back;
        CardImg.sprite = BackSprite;
        CardDataText.text = "";
    }

    public void CardMatched()
    {

    }

    public void CardUnmatched()
    {
        RevertCardtoBack();
    }
}

public enum CardState : int
{
    back = 0, front = 1
}
