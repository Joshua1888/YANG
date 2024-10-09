using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Rendering;

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
        const float deltaX = 0.7f;
        const float deltaY = 0.7f;

        Dictionary<CardType,int> remaining = new Dictionary<CardType, int>();

        listOfCard = new List<GameObject>();
        SaveJsonFile map = readFile("Assets/Resources/test.json");

        remaining.Add(CardType.a, (int)(map.totalCard / 9) * 3);
        remaining.Add(CardType.b, (int)(map.totalCard / 9)*3);
        remaining.Add(CardType.c, map.totalCard - (int)(map.totalCard / 9) * 3*2);

        //GameObject deckClone = Instantiate(deck, canvas.transform);

        //deckClone.transform.localPosition = new Vector3(0.0f, -4.5f, 0.0f);
        for(int layer = 0;layer<map.layers;layer++)
        {
            float numForY = 4;
            float offsetX = map.saveLayer[layer].offsetX ;
            float offsetY = map.saveLayer[layer].offsetY ;
            List<GameObject> newGeneratedCards = new List<GameObject>();
            for (int i = 0; i < map.saveLayer[layer].sizeX; i++)
            {
                float numForX = -2f;
                for (int j = 0; j < map.saveLayer[layer].sizeY; j++)
                {
                    numForX += deltaX;
                    if(!map.saveLayer[layer].layer[i + j * map.saveLayer[layer].sizeX])
                    {
                        continue;
                    }
                    GameObject cardClone = Instantiate(card);
                    cardClone.transform.localPosition = new Vector3(numForX +offsetX, numForY+offsetY, map.layers - layer);
                    
                    newGeneratedCards.Add(cardClone);

                    int resType = Random.Range(0, 3);
                    switch(resType)
                    {
                        case 0:
                            if (remaining[CardType.a]!=0)
                            {
                                cardClone.GetComponent<Card>().setType( CardType.a);
                                remaining[CardType.a]--;
                                break;
                            }
                            else
                            {
                                goto default;
                            }
                        case 1:
                            if (remaining[CardType.b] != 0)
                            {
                                cardClone.GetComponent<Card>().setType(CardType.b);
                                remaining[CardType.b]--;
                                break;
                            }
                            else
                            {
                                goto default;
                            }
                        case 2:

                            if (remaining[CardType.c] != 0)
                            {
                                cardClone.GetComponent<Card>().setType(CardType.c);
                                remaining[CardType.c]--;
                                break;
                            }
                            else
                            {
                                goto default;
                            }
                        default:
                            if (remaining[CardType.a] != 0)
                            {
                                cardClone.GetComponent<Card>().setType(CardType.a);
                                remaining[CardType.a]--;
                                break;
                            }
                            else if (remaining[CardType.b] != 0)
                            {
                                cardClone.GetComponent<Card>().setType(CardType.b);
                                remaining[CardType.b]--;
                                break;
                            }
                            else if (remaining[CardType.c] != 0)
                            {
                                cardClone.GetComponent<Card>().setType(CardType.c);
                                remaining[CardType.c]--;
                                break;
                            }
                            else
                            {
                                Debug.LogError("Can't find the card type");
                                break;
                            }
                            break;
                    }

                }
                numForY -= deltaY;

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


        }
    }

    public SaveJsonFile readFile(string path)
    {
        SaveJsonFile save = new SaveJsonFile();
        string json = System.IO.File.ReadAllText(path);
        if(json == null)
        {
            Debug.LogError("Can't read the file");
            return null;
        }
        save = JsonUtility.FromJson<SaveJsonFile>(json);
        return save;
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

