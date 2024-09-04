using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private List<Card> above;
    private List<Card> below;


    void Start()
    {
        above = new List<Card>();
        below = new List<Card>();
    }

    public void TaskOnClick()
    {
        Debug.LogError("Deck GameObject is not assigned!");

    }

    public void addAbove(Card c)
    {
        above.Add(c);
    }

    public void addBelow(Card c)
    {
        below.Add(c);
    }

    public void removeCard(Card c)
    {
        above.Remove(c);
    }

}