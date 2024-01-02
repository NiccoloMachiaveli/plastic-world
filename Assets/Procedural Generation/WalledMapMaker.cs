using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalledMapMaker : MonoBehaviour {

    // public vars
    public GameObject player;        
    public GameObject reticle;      
    public GameObject[] npcs;        
    public GameObject[] floor;       
    public GameObject wallXHigh;     
    public GameObject wallXLow;      
    public GameObject wallZHigh;     
    public GameObject wallZLow;      
    public int mapWidth;             
    public int mapHeight;            
    public float tileSize;
    public int maxBuildingSize;      // Самые большие случайно сгенерированные здания должны быть размером мин. 3

    GameObject[,] mapFloor;          
    GameObject[,] mapWalls;          
    Vector3 mapCenter;

    void Start()
    {
        if (tileSize == 0f) tileSize = 3.2f;
        mapCenter = new Vector3(mapWidth * tileSize / 2, 0.25f, mapHeight * tileSize / 2);

        layoutMap();
        fillMap();
        fillEdges();

        placeNpcs();

        Vector3 playerStart = new Vector3(tileSize, player.transform.position.y, tileSize);
        Instantiate(player, playerStart, Quaternion.Euler(new Vector3(0f, 45f)));
        Instantiate(reticle);
    }

    // Узнает где пол и стены должны находиться в мире
    void layoutMap()
    {
        mapFloor = new GameObject[mapWidth, mapHeight];
        mapWalls = new GameObject[mapWidth, mapHeight];

        for (int i = 0; i < mapWidth; i++)
            for (int j = 0; j < mapHeight; j++)
            {
                mapFloor[i, j] = floor[Random.Range(0, floor.GetLength(0))];

                if (i % (maxBuildingSize + 1) == 0 && j % (maxBuildingSize + 1) == 0 && 
                    i <= mapWidth - maxBuildingSize && j <= mapHeight - maxBuildingSize)
                    layoutBuilding(i, j);

                if (mapWalls[i, j] != null)
                    if (Random.value < 0.05)
                        mapWalls[i, j] = null;
            }
    }

    // Ставит стены и деревья в мире
    void fillMap()
    {
        for (int i = 0; i < mapWidth; i++)
            for (int j = 0; j < mapHeight; j++)
            {
                GameObject floorObj = mapFloor[i, j];
                GameObject wallObj = mapWalls[i, j];

                Instantiate(floorObj, new Vector3((i + 0.5f) * tileSize, 0.0f, (j + 0.5f) * tileSize),
                    Quaternion.identity);

                if (wallObj != null)
                    Instantiate(wallObj, new Vector3((i + 0.5f) * tileSize, wallObj.transform.position.y, 
                        (j + 0.5f) * tileSize), Quaternion.identity);
            }
    }

    // Ставить стены по краям комнаты
    void fillEdges()
    {
        for (int i = 0; i < mapWidth; i++)
        {
            Instantiate(wallXHigh, new Vector3((i + 0.5f) * tileSize, 
                wallXHigh.transform.position.y, 0.5f * tileSize), Quaternion.identity);
            Instantiate(wallXHigh, new Vector3((i + 0.5f) * tileSize,
                wallXHigh.transform.position.y, (mapHeight - 0.5f) * tileSize), Quaternion.identity);
        }

        for (int j = 0; j < mapHeight; j++)
        {
            Instantiate(wallZHigh, new Vector3(0.5f * tileSize,
                wallZHigh.transform.position.y, (j + 0.5f) * tileSize), Quaternion.identity);
            Instantiate(wallZHigh, new Vector3((mapWidth - 0.5f) * tileSize,
                wallZHigh.transform.position.y, (j + 0.5f) * tileSize), Quaternion.identity);
        }
    }

    // Ставить NPC в случайных местах в комнате
    void placeNpcs()
    {
        foreach (GameObject npc in npcs)
        {
            float xbound = (mapWidth - 1) * tileSize;
            float ybound = (mapHeight - 1) * tileSize;
            Instantiate(npc, new Vector3(Random.Range(tileSize, xbound), 0f,
                Random.Range(tileSize, ybound)), Random.rotation);
        }
    }

    // помещает квадраты стен случайных размеров, которые представляют закрытое место
    void layoutBuilding(int x, int y)
    {
        int size = Random.Range(3, maxBuildingSize);

        for (int i = x; i < x + size; i++)
        {   
            mapWalls[i, y] = pick(wallXHigh, wallXLow, 0.5f);
            mapWalls[i, y + size] = pick(wallXHigh, wallXLow, 0.5f);
        }

        for (int j = y; j < y + size; j++)
        {
            mapWalls[x, j] = pick(wallZHigh, wallZLow, 0.5f);
            mapWalls[x + size, j] = pick(wallZHigh, wallZLow, 0.5f);
        }
    }

    // выбиратает между двумя вещами с некоторой вероятностью (P(option1) = prob, P(option2) = 1 - prob)
    GameObject pick(GameObject option1, GameObject option2, float prob)
    {
        if (Random.value < prob)
            return option1;
        else
            return option2;
    }
}
