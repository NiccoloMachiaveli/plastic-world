using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoldFunctions : MonoBehaviour 
{
    public static int levelTwoScore;
	// Rotate coin
	void Update () {
		transform.Rotate (new Vector3 (15, 30, 45) * Time.deltaTime);
	}

	// If player or companion touch this, disappear and increment score
	void OnTriggerEnter(Collider other) 
	{
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Companion"))
        {
            PlayerScore.score++;
            levelTwoScore = PlayerScore.score;

            gameObject.SetActive(false);

            if (PlayerScore.score >= GameObject.FindGameObjectsWithTag("Pickup").Length + 6)
            {
                // Load the next scene
                SceneManager.LoadScene(3);
            }

        }
	}
}
