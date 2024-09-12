using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Resource 
{
    private static Dictionary<CardType, Sprite> cardInterfaces;

    //use the function to get the card interface
    public static Sprite getCardInterface(CardType t)
    {
        if (cardInterfaces == null)
            if (!loadCardInterface())
            {
                Debug.LogError("Can't load card interface");
                return null;
            }

        if(cardInterfaces.ContainsKey(t))
            return cardInterfaces[t];

        Debug.LogWarning("Can't find the card interface");

        return null;
    }

    // to load card interface into RAM
    private static bool loadCardInterface()
    {
        List<CardInterfaceStorage> cardInterfacesST = Resources.Load<CardInterface>("CardInterface").cardInterface;
        if (cardInterfacesST == null)
        {
            Debug.LogError("Can't load card interface");
            return false;
        }

        if(cardInterfacesST.Count == 0)
        {
            Debug.LogError("Card interface is empty");
            return false;
        }

        cardInterfaces = new Dictionary<CardType, Sprite>();

        foreach(CardInterfaceStorage c in cardInterfacesST)
        {
            cardInterfaces.Add(c.type, c.sprite);
        }

        return true;
    }

}

