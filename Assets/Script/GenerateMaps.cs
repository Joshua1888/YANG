using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

[Serializable]
public class SaveJsonFile
{
    public int layers;
    public int totalCard;
    public SaveLayer[] saveLayer;
}
[Serializable]
public class SaveLayer
{
    public float offsetX;
    public float offsetY;
    public int sizeX;
    public int sizeY;
    public bool[] layer;
}


public class StorageMap
{
    public string name;
    public int CardCount;
    public List<Layer> pos;

    
    public StorageMap(String n)
    {
        name = n;
        pos = new List<Layer>();
    }

    public class Layer
    {
        public int layer;
        public List<Vector2> pos;
        public int count;
        public int getMapMinX()
        {             
            int minX = 1000000;
            foreach (Vector2 v in pos)
            {
                if (v.x < minX)
                {
                    minX = (int)v.x;
                }
            }
            return minX;
        }
        public int getMapMaxX()
        {
            int maxX = -1000000;
            foreach (Vector2 v in pos)
            {
                if (v.x > maxX)
                {
                    maxX = (int)v.x;
                }
            }
            return maxX;
        }
        public int getMapMinY()
        {
            int minY = 1000000;
            foreach (Vector2 v in pos)
            {
                if (v.y < minY)
                {
                    minY = (int)v.y;
                }
            }
            return minY;
        }
        public int getMapMaxY()
        {
            int maxY = -1000000;
            foreach (Vector2 v in pos)
            {
                if (v.y > maxY)
                {
                    maxY = (int)v.y;
                }
            }
            return maxY;
        }
    }

    public List<Vector2> getLayer(int l)
    {
        List<Vector2> res = new List<Vector2>();
        if(pos!=null)
        {
            foreach (Layer layer in pos)
            {
                if (layer.layer == l)
                {
                    res = layer.pos;
                    break;
                }
            }
        }
        return res;
    }

    public int CountTotalNumber()
    {
        int res = 0;
       foreach (Layer l in pos)
        {
            l.count = l.pos.Count;
            res += l.count;
        }
       CardCount = res;
       return res;
    }
    public void addLayer(Dictionary<Vector2,bool> p, int l)
    {
        Layer newLayer = new Layer();
        foreach (Layer layer in pos)
        {
            if (layer.layer == l)
            {
                newLayer = layer;
                break;
            }
        }
        
        newLayer.pos = new List<Vector2>();
        foreach(KeyValuePair<Vector2,bool> kvp in p)
        {
            if (kvp.Value)
            {
                newLayer.pos.Add(kvp.Key);
            }
        }
        newLayer.layer = l;
        pos.Add(newLayer);
        CountTotalNumber();
    }

    public void saveAsJsonFile()
    {
        //check if map valid
        if(CountTotalNumber()%3 != 0)
        {
            Debug.Log(CountTotalNumber());
            Debug.LogError("Map is not valid");
            return;
        }
        SaveJsonFile save = new SaveJsonFile();
        save.layers = pos.Count;
        save.saveLayer = new SaveLayer[pos.Count];
        save.totalCard = CountTotalNumber();

        pos.Sort((x, y) => x.layer.CompareTo(y.layer));

        for(int i = 0; i < pos.Count; i++)
        {
            SaveLayer s = new SaveLayer();
            s.offsetX = 0;
            s.offsetY = 0;
            s.sizeX = pos[i].getMapMaxX() - pos[i].getMapMinX() + 1;
            s.sizeY = pos[i].getMapMaxY() - pos[i].getMapMinY() + 1;

            s.layer = new bool[(pos[i].getMapMaxX()-pos[i].getMapMinX()+1)* (pos[i].getMapMaxY() - pos[i].getMapMinY() + 1)];
            int xOriginal = pos[i].getMapMinX();
            int yOriginal = pos[i].getMapMinY();
            foreach (Vector2 v in getLayer(i))
            {
                s.layer[(int)v.x-xOriginal+((int)v.y-yOriginal)* (pos[i].getMapMaxX() - pos[i].getMapMinX() + 1)] = true;
            }
            save.saveLayer[i] = s;
            
        }

        string json = JsonUtility.ToJson(save);
        System.IO.File.WriteAllText("Assets/Resources/" + name + ".json", json);

    }
    
}

public class GenerateMaps : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }

    //public CardType[] tilesKey;
    //public Tile[] tilesValue;

    public Tile nullTile;
    public Tile blockedTile;
    public Text layerShown;

    //public Dictionary<Vector2, CardType> tileDict = new Dictionary<Vector2, CardType>();
    public Dictionary<Vector2, bool> tileDict = new Dictionary<Vector2, bool>();

    StorageMap storageMap = new StorageMap("test");

    private int cl = 0;
    private int currtLayer
    {
        get { return cl; }
        set
        {
            cl = value;
            layerShown.text = value.ToString();
        }
    }
    

    private void Awake()
    {
        tilemap = GetComponentInChildren<Tilemap>();
        /*
        if (tilesKey.Length != Enum.GetNames(Type.GetType("CardType")).Length-1)
        {
            Debug.LogWarning("Dosen't have all cardType Listed");
        }
        if (tilesValue.Length != Enum.GetNames(Type.GetType("CardType")).Length-1)
        {
            Debug.LogWarning("Not ALL CardType have Tiles");
        }
        */
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (worldPosition.y > 1.2)
                return;

            Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);
            ChangeCardType(new Vector2Int( cellPosition.x,cellPosition.y));
        }
    }

    public void SaveAll()
    {
        Debug.Log("Saving All");
       storageMap.saveAsJsonFile();
    }

    public void Clear()
    {
        tilemap.ClearAllTiles();
    }

    public void SaveMap()
    {
        Debug.Log("Saving Map");
        storageMap.addLayer(tileDict, currtLayer);
        Clear();
        tileDict = new Dictionary<Vector2, bool>();
    }
    public void LoadMap() {
        Debug.Log("Loading Map");
        Clear();
        tileDict = new Dictionary<Vector2, bool>(); 
        List<Vector2> l = storageMap.getLayer(currtLayer);
        foreach (Vector2 pos in l)
        {
            tileDict.Add(new Vector2Int((int)pos.x, (int)pos.y), true);
            tilemap.SetTile(new Vector3Int((int)pos.x, (int)pos.y, 0), blockedTile);
        }
    }
    public void addLayer()
    {
        currtLayer++;
    }
    public void subLayer()
    {
        currtLayer--;
    }

    private void ChangeCardType(Vector2Int pos)
    {
        if (tileDict.ContainsKey(pos))
        {
            tileDict[pos] = !tileDict[pos];
            if (!tileDict[pos])
            {
                tilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), nullTile);
            }
            else if (tileDict[pos])
            {
                tilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), blockedTile);
            }
        }
        else
        {
            tileDict.Add(pos, true);
            tilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), blockedTile);
        }
    }
    /*
    private Tile GetTileValue(CardType Type)
    {
        for (int i = 0; i < tilesKey.Length; i++)
        {
            if (tilesKey[i] == Type)
            {
                return tilesValue[i];
            }
        }
        return null;
    }

    public void ChangeCardType(Vector2Int pos)
    {
        if (tileDict.ContainsKey(pos))
        {
            tileDict[pos]++;
            if (tileDict[pos] > CardType.c)
            {
                tileDict.Remove(pos);
                tilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), nullTile);
                return;
            }
            
            tilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), GetTileValue(tileDict[pos]));
        }
        else
        {
            tileDict.Add(pos, CardType.a);
            tilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), GetTileValue(tileDict[pos]));
        }
    }
    */
}
