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
    private int aboveNum;       //sorage the number of cards above the current card
    private List<Card> below;   //store which cards are below the current card
    
    private Vector3 targetPos;
    private float speed = 1.0f;
    private bool isMoving;

    private bool isClicked;

    void Start()
    {
        isMoving = false;
        aboveNum = 0;
        below = new List<Card>();
        type = CardType.undefined;
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

        SpriteRenderer interfaceSprite = transform.GetChild(1).GetComponent<SpriteRenderer>();
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
        if(aboveNum !=0 )
        {
            Debug.Log("Can't remove the card");
            return;
        }

        RemoveDownParts();

        // try to move the card to Square
        Deck.Moving(this);

    }

    public void RemoveDownParts()
    {
        foreach (Card c in below)
        {
            c.removeCard(this);
        }
    }

    public void addAbove(Card c)
    {
        aboveNum++;
    }
    public void removeCard(Card c)
    {
        aboveNum--;
    }

    public void addBelow(Card c)
    {
        below.Add(c);
    }

    public void changeClick()
    {
        isClicked = true;
    }

    public bool getIsClick()
    {
        return isClicked;
    }
}