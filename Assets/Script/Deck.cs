using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Deck : MonoBehaviour
{

    private static List<Vector3> holder;
    public static List<Card> cardsDeck;
    public static List<CardType> typeInDeck;
    private const int numOfCardH = 7;
    public static float yCord = -4.5f;
    //public static List<CardType> typeDeck;


    // Start is called before the first frame update
    void Start()
    {
        holder = new List<Vector3>();
        cardsDeck = new List<Card>();
        typeInDeck = new List<CardType>();

        DeckPos();
        Debug.Log("Number of positions in holder: " + holder.Count);
    }

    // Initiate the positions for holding card in deck
    private static void DeckPos()
    {
        float num = -1.75f;
        float diatance = (-1.07f) - -1.75f;
        for (int i = 0; i < numOfCardH; i++)
        {
            Vector3 v = new(num, yCord, -5);
            holder.Add(v);
            num += diatance;
        }
    }


    public static void Moving(Card c)
    {
        int index = 0;
        if (!c.getIsClick() && !(cardsDeck.Count >= 7))
        {
            c.changeClick();
            if (!typeInDeck.Contains(c.type))
            {
                cardsDeck.Insert(typeInDeck.Count, c);
                typeInDeck.Insert(typeInDeck.Count, c.type);
                UpdatePosition();
            } else
            {
                foreach (Card cds in cardsDeck)
                {
                    CardType type = cds.type;
                    if (c.type == cds.type)
                    {
                        index = cardsDeck.IndexOf(cds);
                        cardsDeck.Insert(index, c);
                        typeInDeck.Insert(index, c.type);
                        break;
                    }
                }
                UpdatePosition();
                ElimaniteCheck(c.type);
            }
        }
        else
        {
            Debug.Log("Cards already in the deck");
        }
    }


    private static void UpdatePosition()
    {
        int index = 0;
        foreach (Card cds in cardsDeck)
        {
            MovePosition(cds, index);
            index++;
        }
    }

    private static void ElimaniteCheck(CardType cTyp)
    {
        int count = 0;
        foreach (Card c in cardsDeck)
        {
            if (c.type == cTyp)
            {
                count++;
            }
        }
        Debug.LogWarning(count);
        if (count == 3)
        {
            int removePos = typeInDeck.IndexOf(cTyp);
            for (int i = 0; i < 3; i++)
            {
                cardsDeck[removePos].Invisible();
                cardsDeck.RemoveAt(removePos);
                typeInDeck.RemoveAt(removePos);
            }
            UpdatePosition();
        }

    }



    private static void MovePosition(Card c, int index)
    {
        Vector3 targetPosition = holder[index]; // Target position from holder
        // Start the smooth movement coroutine
        c.moveTo(targetPosition, 5f);
    }

    //private static IEnumerator MoveCardToPosition(Card c, Vector3 targetPosition, float speed)
    //{
    //    while (c.transform.position != targetPosition)
    //    {
    //        // Move towards the target position at a constant speed
    //        c.transform.position = Vector3.MoveTowards(c.transform.position, targetPosition, speed * Time.deltaTime);
    //        yield return null; // Wait until the next frame
    //    }
    //}


}
