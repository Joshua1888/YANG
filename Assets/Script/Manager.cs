using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public GameObject card;
    public GameObject canvas;
    public GameObject deck;
    public int numOfCard;
    private int num;
    private List<GameObject> listOfCard;

    // Start is called before the first frame update
    void Start()
    {
       Init();
    }


    //Effect: Generate the card to canvas
    private void Init()
    {


        listOfCard = new List<GameObject>();

        float numForX = -1.5f;
        float numForY = 3;

        //GameObject deckClone = Instantiate(deck, canvas.transform);

        //deckClone.transform.localPosition = new Vector3(0.0f, -4.5f, 0.0f);
        for(int layer = 3;layer>0;layer--)
        {
            List<GameObject> newGeneratedCards = new List<GameObject>();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    GameObject cardClone = Instantiate(card);
                    cardClone.transform.localPosition = new Vector3(numForX, numForY, layer);
                    
                    newGeneratedCards.Add(cardClone);

                    numForX += 1.5f;
                }

                numForX -= 1.5f*3;
                numForY -= 2;

            }

            foreach(GameObject c in newGeneratedCards)
            {
                if(c == null)
                {
                    Debug.LogError("Can't find the card");
                }
                checkForCoverage(c);
            }
            foreach(GameObject c in newGeneratedCards)
            {

                listOfCard.Add(c);
            }

            numForY += 6;

            numForX -= 0.1f;
            numForY -= -0.2f;

        }
    }

    // Effect Disabled the card not in first layer
    private void checkForCoverage(GameObject check)
    {

        // Get check's object position
        RectTransform rCheck = check.GetComponent<RectTransform>();
        float minVx = rCheck.rect.xMin + rCheck.anchoredPosition.x;
        float maxVx = rCheck.rect.xMax + rCheck.anchoredPosition.x;
        float minVy = rCheck.rect.yMin + rCheck.anchoredPosition.y;
        float maxVy = rCheck.rect.yMax + rCheck.anchoredPosition.y;


        foreach (GameObject cas in listOfCard)
        {

            RectTransform cCheck = cas.GetComponent<RectTransform>();

            float minCx = cCheck.rect.xMin + cCheck.anchoredPosition.x;
            float maxCx = cCheck.rect.xMax + cCheck.anchoredPosition.x;
            float minCy = cCheck.rect.yMin + cCheck.anchoredPosition.y;
            float maxCy = cCheck.rect.yMax + cCheck.anchoredPosition.y;

            bool xOverlap = (minVx < maxCx) && (maxVx > minCx);
            bool yOverlap = (minVy < maxCy) && (maxVy > minCy);

            if (xOverlap && yOverlap)
            {
                Card checkCard = check.GetComponent<Card>();
                Card casCard = cas.GetComponent<Card>();

                if (checkCard == null || casCard == null)
                {
                    Debug.LogError("Can't get the card");
                    return;
                }

                casCard.addAbove();
                checkCard.addBelow(casCard);
            }
        }
    }


}

