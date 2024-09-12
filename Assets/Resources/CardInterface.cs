
//files to storage card interface
using System.Collections.Generic;
using System;
using UnityEngine; 
using UnityEditor; 

[CreateAssetMenu(fileName = "CardInterface", menuName = "CardInterface")]
public class CardInterface : ScriptableObject
{
    public List<CardInterfaceStorage> cardInterface;
}
[Serializable] 
public class CardInterfaceStorage 
{
    public CardType type;
    public Sprite sprite;
}