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
       // Init();
    }


    //Effect: Generate the card to canvas
    private void Init()
    {


        listOfCard = new List<GameObject>();

        int numForX = -15;
        int numForY = 30;

        GameObject deckClone = Instantiate(deck, canvas.transform);

        deckClone.transform.localPosition = new Vector3(0.0f, -4.5f, 0.0f);

        for (int i = 0; i < 3; i++)
        {
            for (int n = 0; n < 3; n++)
            {
                GameObject cardClone = Instantiate(card, canvas.transform);
                cardClone.transform.localPosition = new Vector3(numForX, numForY, 0);
                checkForCoverage(cardClone);
                listOfCard.Add(cardClone);

                GameObject cardCloneb = Instantiate(card, canvas.transform);
                cardCloneb.transform.localPosition = new Vector3(numForX, numForY - 2, 0);
                checkForCoverage(cardCloneb);
                listOfCard.Add(cardCloneb);

                numForX += 15;
            }

            numForX = -15;
            numForY -= 20;

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

            bool xOverlap = minVx < maxCx && maxVx > minCx;
            bool yOverlap = minVy < maxCy && maxVy > minCy;

            if (xOverlap && yOverlap)
            {
                // cas.GetComponent<Card>().interactable = false;
                check.GetComponent<Card>().addBelow(cas.GetComponent<Card>());
                cas.GetComponent<Card>().addAbove(check.GetComponent<Card>());
            }
        }
    }


}

