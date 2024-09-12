using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

// defined the type of cards 
public enum CardType
{
    undefined,
    a,
    b,
    c
} 

public class Card : MonoBehaviour
{
 
    private CardType type;      //type of the card
    private int aboveNum_;       //sorage the number of cards above the current card
   
    private List<Card> below;   //store which cards are below the current card
    
    private Vector3 targetPos;
    private float speed = 1.0f;
    private bool isMoving;

    private bool isMovedFromBoard;

    public GameObject darkMask;

    private int aboveNum
    {
        get { return aboveNum_; }
        set
        {
            aboveNum_ = value;
            try
            {
                if (aboveNum_ == 0)
                {
                    darkMask.SetActive(false);
                }
                else if (aboveNum_ > 0)
                {
                    darkMask.SetActive(true);
                }
                else
                {
                    Debug.LogError("aboveNum is less than 0");
                }
            }
            catch
            {
                Debug.LogError("Can't find darkMask");
            }
        }
    }       //sorage the number of cards above the current card

    void Awake()
    {
        darkMask = transform.GetChild(0).gameObject;
        isMoving = false;
        aboveNum = 0;
        below = new List<Card>();
        type = CardType.undefined;
        isMovedFromBoard = false;
        // test part
            setType(CardType.undefined);
        // test ends
    }

    
    void FixedUpdate()
    {
        // if the card should be moved
        if( isMoving )
        {
            // Move our position a step closer to the target.
            var step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, targetPos, step);

            //sotp moving if card has reached the target position
            if(Vector3.Distance(transform.position, targetPos) < 0.001f)
            {
                isMoving = false;
            } 
        }

    }

    //set the target position for the card and start moving
    public void moveTo(Vector3 pos)
    {
        if (isMoving)
        {
            Debug.Log("is still moving");
        }
        
        targetPos = pos;
        isMoving = true;
        
    }

    //set the type of the card and change the interface
    public void setType(CardType t)
    {
        type = t;

        SpriteRenderer interfaceSprite = transform.GetChild(2).GetComponent<SpriteRenderer>();
        if(interfaceSprite == null)
        {
            Debug.LogError("Can't get SpriteRenderer");
            return;
        }
        if(Resource.getCardInterface(t) == null)
        {
            Debug.LogError("Resource.getCardInterface(t) is null");
            return;
        }
        
        interfaceSprite.sprite = Resource.getCardInterface(t);
        return;
    }

    public void TaskOnClick()
    {
        if(isMovedFromBoard)
        {
            Debug.Log("Card is already moved from board");
            return;
        }

        if(aboveNum !=0 )
        {
            Debug.Log("Can't remove the card");
            return;
        }

        Deck.Moving(this);

        // 
        RemoveDownParts();

        // try to move the card to Square
        
        Debug.Log("Card is clicked");
        Debug.Log("Card type is: " + type);
        Debug.Log("Number of cards above: " + aboveNum);   
        Debug.Log("Number of cards below: " + below.Count);

    }

    public void RemoveDownParts()
    {
        foreach (Card c in below)
        {
            c.removeAbove();
        }
    }

    public void addAbove()
    {
        aboveNum++;
    }
    public void removeAbove()
    {
        aboveNum--;
    }

    public void addBelow(Card c)
    {
        if(below == null)
        {
            Debug.LogError("Can't find below");
            return;
        }
        below.Add(c);
    }


    public bool getIsClick()
    {
        return isMovedFromBoard;
    }


    public void changeClick()
    {
        isMovedFromBoard = true;
    }
    
}