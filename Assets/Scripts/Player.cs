﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	private CharacterController	controller;
	private Vector3 			moveVector;
	private Vector3 			positionVector;

	//Player Attributes
	public  float 				red 	= 100f;
	public  float 				green 	= 100f;
	public  float 				blue 	= 100f;
	public  ParticleSystem 		smoke;
	private Color 				smokeColour;
    public  Color               oceanColor;

	//FrameCounter
	private int 				framz;
	public int					smokeDelay;

	//Constants
	private float 	animationDuration 	= 4.0f;
	private float 	buoyancy 			= 0.5f;
	private float 	speed 				= 5f;
	private float 	verticalVelocity	= 0.0f;
	public float 	minWidth			= 0f;
	public float 	maxWidth			= 10f;
    private float 	health				= 100f;
	public float 	healthMult			= 5;



    private void Start () 
	{
		controller = GetComponent<CharacterController> ();
		positionVector = transform.position;

		//set Player start attributes Colour is between 0 and 1
		smokeColour = new Color(1f, 1f, 1f, 1f);
		framz = 0;
        oceanColor = new Color(0, 0, 1, 1);

	}
	
	private void Update () 
	{



		//testing colour change --> it works
		//smokeColour = Random.ColorHSV();

		/*
		 * Restricts player from moving left and right until the camera animation is complete
		*/

		if (Time.time < animationDuration) 
		{
			controller.Move (Vector3.forward * speed * Time.deltaTime);
			return;
		}

		moveVector = Vector3.zero;

		/*
		 * By using axis, we can easily port to differnt input devices. 
		 * It also requires less code. Inside unity, go to edit -> project settings -> Input
		 * Here you can see all the features of using the Horizontal axis.
		 * 
		 * X is for Horizontal Movement
		 * Left and Right
		*/

		//Removing horizontal movement as per Benham
		//moveVector.x = Input.GetAxis("Horizontal") * speed;



		/* 
		 * Y is for vertical movement, such as buoyancy
		 * Here we make a simple buoyancy function. When the object is not touching the ground, 
		 * every second vertical velocity is increased. Otherwise, it is constant
		*/
		moveVector.y = Input.GetAxis("Vertical") * speed;

		if (controller.isGrounded) 
		{
			verticalVelocity = 0f;
			//verticalVelocity = -0.01f;
		} else 
		{
			verticalVelocity -= buoyancy * Time.deltaTime;
		}
		moveVector.y += verticalVelocity;



		// Z is for forward movement
		moveVector.z = speed;

		controller.Move(moveVector * Time.deltaTime);

		//Clamping
		positionVector 		= transform.position;
		//Not Needed now
		//positionVector.x 	= Mathf.Clamp (positionVector.x, minWidth, maxWidth);
		positionVector.y 	= Mathf.Clamp (positionVector.y, minWidth, 2*maxWidth);
		transform.position	= positionVector;


		//Update smoke colour. The if statement allows for fewer particles to be generated, editable from unity interface.
		smokeColour = new Color(red/100f, green/100f, blue/100f, 1f);

		//If health will effect the alpha, comment out above line and use the one below
		//smokeColour = new Color(red/100f, green/100f, blue/100f, health/100f);

		if (framz > smokeDelay) {
			var emitParams = new ParticleSystem.EmitParams ();
			//emitParams.position = transform.position;
			//Debug.Log (emitParams.position);
			emitParams.startColor = smokeColour;
			smoke.Emit (emitParams, 1);
			framz = 0;
		}
		framz++;
		
	}

    public void EatFishColor(Color color)
    {
        oceanColor += color;

		//hitting fish changes the colour attributes of the player which changes the smoke colour
		red -= healthMult * color.r;
		blue -= healthMult * color.b;
		green -= healthMult * color.g;
    }

    public void AffectHealth(float healthImpact)
    {
        health += healthImpact;
		//I thought we were using the colour to represent health, not another variable
		//I set this up to affect the colour of the smoke.
		//Alternatvly, we could have this second health be the alpha of the smoke to show the health.
    }

    //Remove Health if fish is alive at a rate of 1 unit per second
    IEnumerator removeHealth()
    {
        while (true)
        {
            if (health > 0f)
            { // if health > 0
                health -= 1f; // reduce health and wait 1 second
                yield return new WaitForSeconds(1);
            }
            else
            { // if health < 0
                yield return null;
            }
        }
    }
}
