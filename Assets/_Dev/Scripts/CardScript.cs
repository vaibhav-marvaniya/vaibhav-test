using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardScript : MonoBehaviour
{
    [SerializeField]
    int CardX, CardY;
    public int CardDataNum;
    [SerializeField]
    CardState CurrentCardState;
    [SerializeField]
    public bool isCardFrontFlipped = false;
    [SerializeField]
    bool CanClickCard = true;

    public Sprite BackSprite, FrontSprite;
    public Text CardDataText;
    public Image CardImg;
    public Button CardBtn;

    private float ShakeTimer;
    private Vector3 RandomPos;
    [SerializeField]
    float ShakeTime = 1f;
    [SerializeField]
    float _distance = 2f;

    Coroutine shakecoroutine;
    Vector2 StartPos;

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
        CanClickCard = true;
    }

    void OnCardClick()
    {
        Debug.Log("OnCardClick "+ CanClickCard);
        if (!CanClickCard)
            return;
        CanClickCard = false;
        CardBtn.interactable = CanClickCard;
        Invoke(nameof(RevertCardtoBack), GameManager.instance.CardFrontTime);
        StartCoroutine(ScaleOverSeconds(new Vector3(0, 1, 1), GameManager.instance.CardFliptime / 2, null, () =>
        { StartCoroutine(ScaleOverSeconds(new Vector3(1, 1, 1), GameManager.instance.CardFliptime / 2, SetFrontCard, () => { isCardFrontFlipped = true; })); }));
        GameManager.instance.OnCardClick(this);
    }

    void SetFrontCard()
    {
        Debug.Log("SetFrontCard");
        CurrentCardState = CardState.front;
        CardImg.sprite = FrontSprite;
        CardDataText.text = CardDataNum.ToString();
    }

    void SetBackCard()
    {
        Debug.Log("SetBackCard");
        CurrentCardState = CardState.back;
        CardImg.sprite = BackSprite;
        CardDataText.text = "";
    }

    void RevertCardtoBack()
    {
        Debug.Log("RevertCard");
        GameManager.instance.RemoveCardfromClickedCard(this);
        if(shakecoroutine!=null)
            StopCoroutine(shakecoroutine);
        CardImg.transform.localPosition = StartPos;
        StartCoroutine(ScaleOverSeconds(new Vector3(0, 1, 1), GameManager.instance.CardFliptime / 2, null, () =>
        { StartCoroutine(ScaleOverSeconds(new Vector3(1, 1, 1), GameManager.instance.CardFliptime / 2, SetBackCard, () => { isCardFrontFlipped = false; CanClickCard = true; CardBtn.interactable = CanClickCard; })); }));
    }

    public void CardMatched(CardScript secondcard)
    {
        CancelInvoke(nameof(RevertCardtoBack));
        StartCoroutine(MatchAnimation(secondcard, 0.3f, Vector2.one * 1.2f, null));
    }

    public void CardUnmatched(CardScript secondcard)
    {
        CancelInvoke(nameof(RevertCardtoBack));
        shakecoroutine = StartCoroutine(Shake(secondcard));
    }

    public IEnumerator ScaleOverSeconds(Vector3 end, float seconds, System.Action preaction, System.Action postaction)
    {
        Debug.Log("ScaleOverSeconds");
        preaction?.Invoke();
        float elapsedTime = 0;
        Vector3 start = transform.localScale;
        while (elapsedTime < seconds)
        {
            transform.localScale = Vector3.Lerp(start, end, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.localScale = end;
        postaction?.Invoke();
    }

    public IEnumerator MatchAnimation(CardScript secondcard, float seconds, Vector3 end, System.Action postaction)
    {
        yield return new WaitUntil(() => isCardFrontFlipped == true && secondcard.isCardFrontFlipped == true);
        float elapsedTime = 0;
        Vector3 start = transform.localScale;
        while (elapsedTime < seconds)
        {
            transform.localScale = Vector3.Lerp(start, end, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.localScale = end;

        elapsedTime = 0;
        while (elapsedTime < seconds)
        {
            transform.localScale = Vector3.Lerp(end, start, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.localScale = start;

        elapsedTime = 0;
        while (elapsedTime < seconds)
        {
            transform.localScale = Vector3.Lerp(start, end, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.localScale = end;

        elapsedTime = 0;
        while (elapsedTime < seconds)
        {
            transform.localScale = Vector3.Lerp(end, start, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.localScale = start;

        Color originalColor = CardImg.color;
        Color targetColor = CardImg.color;
        targetColor.a = 0.0f;
        elapsedTime = 0;
        while (elapsedTime < seconds)
        {
            CardImg.color =  Color.Lerp(originalColor, targetColor, (elapsedTime / seconds));
            CardDataText.color = Color.Lerp(originalColor, targetColor, (elapsedTime / seconds));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        postaction?.Invoke();
    }

    private IEnumerator Shake(CardScript secondcard)
    {
        yield return new WaitUntil(() => isCardFrontFlipped == true && secondcard.isCardFrontFlipped == true);
        StartPos = CardImg.transform.localPosition;
        ShakeTimer = 0f;
        while (ShakeTimer < ShakeTime)
        {
            ShakeTimer += Time.deltaTime;
            RandomPos = StartPos + (Random.insideUnitCircle * _distance);
            CardImg.transform.localPosition = RandomPos;
            yield return null;
        }
        CardImg.transform.localPosition = StartPos;
        Invoke(nameof(RevertCardtoBack), 1f);
    }
}

public enum CardState : int
{
    back = 0, front = 1
}
