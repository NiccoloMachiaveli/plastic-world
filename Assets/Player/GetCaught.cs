using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GetCaught : MonoBehaviour {

    bool isInvincible;        // true if Player has just been hit so score isn't drained
    float invincibleTimer;    // how long invincibility lasts for

    float INV_TIME = 1f;      // Постоянная длительность таймера (1 секунда реального времени)

    bool wasCaught;          // flag that says whether to draw to the screen and restart scene or not

    // Initialize variables
    void Start()
    {
        // invincibility vars
        isInvincible = false;
        invincibleTimer = INV_TIME;
    }

    // Checks values at each frame
    void Update()
    {
        // check values related to invincibility
        if (isInvincible) invincibleTimer -= Time.deltaTime;
        if (invincibleTimer <= 0f) isInvincible = false;
    }

    // Determine what happens when player collides with NPCs
    void OnTriggerEnter(Collider other)
    {
        // Уменьшить счёт, если Protector столкнется с нами - сделать временно непобедимым
        if (other.gameObject.CompareTag("Protector") & !isInvincible)
        {
            PlayerScore.score--;
            WandererAI.levelOneScore = PlayerScore.score;
            isInvincible = true;
            invincibleTimer = INV_TIME;
        }

        // Reload scene and have screen flash red if caught by Predator or Stalker
        if (other.gameObject.CompareTag("Predator") || other.gameObject.CompareTag("Stalker"))
        {
            // indicate game over
            wasCaught = true;
        }
    }

    // Показывать текст и экранные подсказки о том, что вас поймали
    void OnGUI()
    {
        if (wasCaught)
        {
            GUIStyle style = new GUIStyle();
            style.richText = true;
            GUILayout.Label("<size=30><color=white>You were caught! Press any key to continue.</color></size>", style);

            if (Input.anyKeyDown || Input.anyKey) // reload scene upon key press
            {
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }
        }
    }
}
