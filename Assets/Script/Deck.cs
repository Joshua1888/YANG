using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{

    private static List<Vector3> holder;
    private const int numOfCardH = 7;
    public static int index;
    public static List<Card> cardsDeck;


    // Start is called before the first frame update
    private static void Start()
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
        int num = -300;
        for (int i = 0; i < numOfCardH; i++)
        {
            num += 75;
            Vector3 v = new Vector3(num, -450, 0);
            holder.Add(v);
        }
    }

    public static void Moving(Card c)
    {
        if (index >= holder.Count)
        {
            Debug.LogError("No more positions available in the holder!" + holder.Count);
        }
        else
        {
            Vector3 v = holder[index];
            c.transform.Translate(v * 5 * Time.deltaTime);
            index++;
        }
    }

}
