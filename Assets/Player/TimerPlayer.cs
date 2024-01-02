using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerPlayer : MonoBehaviour
{
    private float timeLeft = 60.0f;
    public static bool isUsed;// sets whether scoring is in use or not for the scene
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().Equals(SceneManager.GetSceneByBuildIndex(3)))
            isUsed = true;
        else
            isUsed = false;
        InvokeRepeating("DecrementTimer", 1.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void DecrementTimer()
    {
        if (SceneManager.GetSceneByBuildIndex(3).isLoaded)
        {
            timeLeft--;

            if (timeLeft == 0)
            {
                SceneManager.LoadScene(3);
            }
        }
        
    }

    void OnGUI()
    {
        if (isUsed) 
            GUI.Label(new Rect(10, 10, 100, 20), "Time: " + timeLeft);
    }
}
