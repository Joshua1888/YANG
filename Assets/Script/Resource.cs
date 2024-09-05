using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Resource 
{
    private static Dictionary<CardType, Sprite> cardInterface;

    //use the function to get the card interface
    public static Sprite getCardInterface(CardType t)
    {
        if(cardInterface != null) loadCardInterface();

        if(cardInterface.ContainsKey(t))
            return cardInterface[t];

        return null;
    }

    private static void loadCardInterface()
    {
        /*
        cardInterface = new Dictionary<CardType, Sprite>();
        cardInterface.Add(CardType.a, Resources.Load<Sprite>("a"));
        cardInterface.Add(CardType.b, Resources.Load<Sprite>("b"));
        cardInterface.Add(CardType.c, Resources.Load<Sprite>("c"));
    */
    }

}
