using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{

    private static List<Vector3> holder;
    private const int numOfCardH = 7;
    public static int index;
    public static List<Card> cardsDeck;
    public static float yCord = -4.5f;


    // Start is called before the first frame update
    void Start()
    {
        holder = new List<Vector3>();
        index = 0;
        cardsDeck = new List<Card>();
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
            num += diatance;
            Vector3 v = new(num, yCord, -5);
            holder.Add(v);
        }
    }

    public static void Moving(Card c)
    {
        if (!c.getIsClick())
        {
            if (index >= holder.Count)
            {
                Debug.LogError("No more positions available in the holder!" + holder.Count);
            }
            else
            {
                Vector3 targetPosition = holder[index]; // Target position from holder
                index++;
                c.changeClick();
                // Start the smooth movement coroutine
                c.StartCoroutine(MoveCardToPosition(c, targetPosition, 1.0f)); 

            }
        }
        else
        {
            Debug.Log("Cards already in the deck");
        }        
    }


    private static IEnumerator MoveCardToPosition(Card c, Vector3 targetPosition, float speed)
    {
        while (c.transform.position != targetPosition)
        {
            // Move towards the target position at a constant speed
            c.transform.position = Vector3.MoveTowards(c.transform.position, targetPosition, speed * Time.deltaTime);
            yield return null; // Wait until the next frame
        }
    }


}
