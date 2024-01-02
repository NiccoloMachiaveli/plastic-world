﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Родительский класс для всех наших конкретных модулей ИИ, который предоставляет методы для высокоуровневых действий,
// таких как перемещение или ориентация вокруг игрока. Этот класс используется для хранения и установки переменных, 
// относящихся к состоянию NPC, а также предоставляет функции-оболочки для перемещения и т. д., 
// в первую очередь для облегчения использования программистом. Реализации движения и видения находятся во вспомогательных классах, 
// соответствующим образом помеченных как Movement и Vision.

public class CoreAI : MonoBehaviour
{
    // ***** Constants *****

    // ***** AI State Variables *****
    protected Transform my_transform;      // this NPC's position, rotation, scale
	protected NavMeshAgent my_nav;         // this NPC's navigation component (for Navmesh use)
	protected Rigidbody my_Rigidbody;      // this NPC's rigidbody component (for physics calculations)
    protected Vector3 dest;                // this NPC's current destination
	public bool isActive;                  // toggle variable for NPC being on or off (good for putting them to sleep or deactivating robots, etc)

    // ***** Helper Classes *****
    private Movement mv;                   // class to handle movement implementations
    private Vision vs;                     // class to handle vision implementations

    // ***** Player Details *****
    protected GameObject player;           // the human player's object reference
	protected GameObject playerFront;	   // a location 6 units in front of the player (for following or tracking and prediction)
    protected Transform player_transform;  // the location, rotation, etc. of the player object
    
    // This initializes CoreAI - call it in the Start() method!
    protected void CoreAIStart()
    {
        // set state variables, default active state is on
        my_transform = GetComponent<Transform>();
        my_nav = GetComponent<NavMeshAgent>();
        my_Rigidbody = GetComponent<Rigidbody>();
        isActive = true; // turn this to false in your own NPC class if you want a character to start deactivated

        // initialize helper classes
        mv = new Movement(this.gameObject);
        vs = new Vision(this.gameObject);

        // initialize player details
        player = GameObject.FindWithTag("Player");
		playerFront = GameObject.Find("Front of Player");
        player_transform = player.GetComponent<Transform>();
    }


	// ************ Activity State functions ************

	// Tell the NPC to "wake up" by passing in true or "go to sleep" by passing in false, returns the argument
	protected bool setActive(bool val)
	{
		return isActive = val;
	}

    // Inverts the active state, returns new state as bool
    protected bool toggleActive()
    {
        return isActive = !isActive;
    }


    // ************* Utility Functions ***************
    
    // designate new location of interest
    protected void setDest(Vector3 d)
    {
        dest = d;
    }

    // designate new random location of interest, within walkDist and on the Navmesh
    protected void randomDest(float walkDist)
    {
        NavMeshHit hit;
        Vector3 randomDirection = Random.insideUnitSphere * walkDist;
        randomDirection += my_transform.position;
        NavMesh.SamplePosition(randomDirection, out hit, walkDist, NavMesh.AllAreas);
        setDest(hit.position);
    }

	// return GameObject under the mouse, good for first-person position and directing
	protected GameObject getMouseObject()
	{
		return mv.getMouseObject();
	}

    // returns the distance between NPC position and player position
    protected float distToPlayer()
    {
        return Vector3.Distance(my_transform.position, player_transform.position);
    }


    // ****************** Movement *******************

    // head toward the specified GameObject's position - returns true if valid destination
    protected bool moveTo(GameObject obj)
    {
        return mv.moveTo(obj);
    }

    // head toward a specific position - returns true if valid destination
    protected bool moveTo(Vector3 d)
    {
        return mv.moveTo(d);
    }

    // head toward target destination if no argument is given - returns true if valid destination
    protected bool moveTo()
    {
        return mv.moveTo(dest);
    }

    // stop moving
    protected void stopHere()
    {
        mv.stopHere();
    }


    // ************* Rotation and Looking ****************

    // return the mouse location as a position
    protected Vector3 findMouse()
	{
        return mv.findMouse();
	}

    // turn towards target destination if no arguments are given
    protected void lookAt()
    {
        mv.lookAt(dest);
    }

    // turn towards specified GameObject
    protected void lookAt(GameObject obj)
    {
        mv.lookAt(obj);
    }

    // turn towards specified position
    protected void lookAt(Vector3 pos)
    {
        mv.lookAt(pos);
	}

	// turn towards where the mouse is pointing
	protected void lookAtMouse()
	{
        mv.lookAtMouse();
	}

	// *************** Vision ***************

    //check if object is in view, true if it is, false if not
    protected bool inFOV(GameObject obj)
    {
        return vs.inFOV(obj);   
    }

    // see if a certain position is within the field of view
    protected bool inFOV(Vector3 pos)
    {
        return vs.inFOV(pos);
    }

    // if no argument is given, just checks if the target destination is in view
    protected bool inFOV()
    {
        return vs.inFOV(dest);
    }
}