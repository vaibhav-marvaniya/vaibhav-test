using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardScript : MonoBehaviour
{
    public int CardX, CardY;
    public RectTransform CardRect;

    public void SetCardData(int x, int y)
    {
        CardX = x;
        CardY = y;
    }
}
