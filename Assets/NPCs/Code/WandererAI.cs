using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;


// Wanderer выбирает новый пункт назначения в пределах видимости всякий раз, 
// когда он достигает своего текущего пункта назначения.
public class WandererAI : CoreAI
{
    public bool inDanger;   // Переменная на которую смотрить Protector чтобы определить состояние

    float COOL_TIME = 5f;  // Константа времени восстановления в секундах
    float dangerCooldown;   // Сбрасывается в состоянии опасности по достижении COOL_TIME
    float walkDist;         // Расстояние, на которое Wanderer должен пройти, прежде чем сменить направление 
    public static int levelOneScore;


    protected void Start()
	{
        // initialize parent class
        CoreAIStart();

        // set vars
        inDanger = false;
        dangerCooldown = COOL_TIME;
        walkDist = 20f;

        // begin walking randomly
        randomDest(walkDist);
	}

	// Update is called once per frame
	void Update()
	{
		// if reached target or out of range, randomly change target position
		if ((my_transform.position - dest).magnitude < 2 || 
            (my_transform.position - dest).magnitude > walkDist + 1)
            randomDest(walkDist);

        // wander randomly - check if the movement is valid in case the Wanderer has
        // tried to walk off the map.  If that has happened, set a new destination
        bool couldMove = moveTo();
        if (!couldMove) { randomDest(walkDist); }

        // if inDanger is true, add to the cool down timer - reset inDanger if cool
        if (inDanger) dangerCooldown -= Time.deltaTime;
        if (dangerCooldown <= 0.0)
        {
            inDanger = false;
            dangerCooldown = COOL_TIME;
        }
	}

    // if player touches this, increment score, indicate danger, 
    // and then jump to a random place in room
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // add to score
            PlayerScore.score = PlayerScore.score + 100;
            levelOneScore = PlayerScore.score;

            // С пяти процентной вероятностью будет смена сцены
            if (Random.value < 0.20f)
            {
                SceneManager.LoadScene(2);
            }

            // indicate danger
            inDanger = true;
            dangerCooldown = COOL_TIME;

            // jump randomly
            RandomMapMaker script = GameObject.Find("ProceduralGenerator").GetComponent<RandomMapMaker>();
            bool successful = false;
            while (!successful)// keep jumping until it's valid
            {
                float randomX = Random.Range(0f, (script.mapWidth - 1) * script.tileSize);
                float randomZ = Random.Range(0f, (script.mapWidth - 1) * script.tileSize);
                successful = my_nav.Warp(new Vector3(randomX, 0f, randomZ)); 
            }
        }
    }
}
