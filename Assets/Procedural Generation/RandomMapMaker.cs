using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Этот код бдует прикреплен к процедурной генерации, чтобы случайно генерировать предметы
public class RandomMapMaker : MonoBehaviour {

    // публичные переменные
    public GameObject player;        // Экземпляр объекта
    public GameObject reticle;       // Представление мыши на экране
    public GameObject[] npcs;        // Список неигровых персонажей 
    public GameObject[] floor;       // Список префабов пола
    public GameObject[] groundItems; // Список наземных предметов
    public GameObject[] inAirItems;  // Список летающих предметов
    public GameObject wallXHigh;     // Горизонтальная стена по оси X
	public GameObject wallXLow;      // Вертикальная стена по оси X
    public GameObject wallZHigh;     // Горизонтальная стена по оси Z
    public GameObject wallZLow;      // Вертикальная стена по оси Z
    public float groundItemProb;     // Вероятность размещения наземных предметов
    public float inAirItemProb;      // Вероятность размещения летающих предметов
    public float minScaling;         // Минимально возможный размер тайлов
	public float maxScaling;         // Максимально возможный размер тайлов
    public int mapWidth;             // Ширина карты в количестве тайлов пола
    public int mapHeight;            // Высота карты в количестве тайлов пола
    public float tileSize;           // Размер тайлов
    public float yOffset;            // Расстояние между летающими и наземными предметами
    public bool shouldStack;         // Правда, если летающий объект может выйти на наземный объект

    // приватные переменные
    GameObject[,] mapFloor;          // Тип и место пола на карте
    GameObject[,] mapItems;          // Какой и куда, поместить наземный предмет
    GameObject[,] mapInAirItems;     // Какой и куда, поместить летающий предмет
    Vector3 mapCenter;               // Позиция центра карты

    // Вызывается в начале программы
    void Start()
    {
        // Инициализация переменных
        if (tileSize == 0f) tileSize = 3.2f;  // Если нету размера, ставим этот
        mapCenter = new Vector3(mapWidth * tileSize / 2, 1f, mapHeight * tileSize / 2);

        // Вызов функций для отображения карты в виде массива и создать экземпляр среды
        layoutMap();
        fillMap();
		fillEdges();

        // После создания сцены, помещает NPC
        placeNpcs();

        // Помещает игрока в центр и добавляет представление мыши
        Instantiate(player, mapCenter, Quaternion.identity);
        Instantiate(reticle);
    }

    // Узнает где пол и предметы должны находиться в мире
    void layoutMap()
    {
        mapFloor = new GameObject[mapWidth, mapHeight];
        mapItems = new GameObject[mapWidth, mapHeight];
        mapInAirItems = new GameObject[mapWidth, mapHeight];

        for (int i = 0; i < mapWidth; i++)
            for (int j = 0; j < mapHeight; j++)
            {
                mapFloor[i, j] = floor[Random.Range(0, floor.GetLength(0))];

                if (Random.value < groundItemProb)
                    mapItems[i, j] = groundItems[Random.Range(0, groundItems.GetLength(0))];
                else
                    mapItems[i, j] = null;

                if (Random.value < inAirItemProb)
                {
                    mapInAirItems[i, j] = inAirItems[Random.Range(0, inAirItems.GetLength(0))];
                    if (!shouldStack && mapItems[i, j] != null)
                        mapInAirItems[i, j] = null;
                }
                else
                    mapInAirItems[i, j] = null;
            }
    }

    // Ставит полы и предметы в мире
    void fillMap()
    {
        for (int i = 0; i < mapWidth; i++)
            for (int j = 0; j < mapHeight; j++)
            {
                GameObject floorObj = mapFloor[i, j];
                GameObject groundItemObj = mapItems[i, j];
                GameObject inAirItemObj = mapInAirItems[i, j];

                Instantiate(floorObj, new Vector3(i * tileSize, 0.0f, j * tileSize), 
                    Quaternion.identity);

                if (groundItemObj != null)
                {
                    float scale = Random.Range(minScaling, maxScaling);
                    Vector3 scaling = new Vector3(scale, scale, scale);
					GameObject t = Instantiate(groundItemObj, 
                        new Vector3(i * tileSize, groundItemObj.transform.position.y, j * tileSize),
                        Quaternion.identity);
                    t.transform.localScale += scaling;
                }

                if (inAirItemObj != null)
                    Instantiate(inAirItemObj, new Vector3(i * tileSize, 
                        yOffset + inAirItemObj.transform.position.y, j * tileSize), 
                        Quaternion.identity);
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
            Instantiate(npc, new Vector3(Random.Range(0f, xbound), 0f, 
                Random.Range(0f, ybound)), Quaternion.identity);
        }
    }

    // Создает NavMesh после генерации карты
    void bakeNavMesh()
    {
        Bounds b = new Bounds(mapCenter, mapCenter + new Vector3(0f, 10f, 0f));
        List<NavMeshBuildSource> sources = new List<NavMeshBuildSource>();
        List<NavMeshBuildMarkup> markups = new List<NavMeshBuildMarkup>();
        NavMeshBuilder.CollectSources(b, 0, NavMeshCollectGeometry.RenderMeshes, 0, markups, sources);
        NavMeshBuildSettings settings = NavMesh.CreateSettings();

        NavMeshBuilder.BuildNavMeshData(settings, sources, b, mapCenter, Quaternion.identity);
    }
}
