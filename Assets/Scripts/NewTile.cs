﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewTile : MonoBehaviour {

	public GameObject tilePrefab;
    public GameObject fishPrefab;
	public GameObject fishHook;
    public GameObject seaweed;
	private GameObject player;

    // Use this for initialization
    void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
	}
	

	public GameObject SpawnTile(bool fish, int timeAlive)
	{
		GameObject go;
		go = Instantiate (tilePrefab) as GameObject;


		//spawn fish
		if (fish) {
			int fishz = Random.Range (5, 15);
			for (int i = 0; i < fishz; i++) {
				if (Random.Range (0, 10000) < timeAlive)
					SpawnFish (go.transform, true);
				else
					SpawnFish (go.transform, false);
			}

			//Spawn fish hooks
			for (int i = 0; i < 3; i++) {
				SpawnFishHooks(go.transform);
			}

            //Spawn seaweed
            for (int i = 0; i < 30; i++)
            {
                SpawnSeaweed(go.transform);
            }
        }



        //Implement Background Object Spawning
        #warning Call background object spawning methods with random frequency

        return go;
	}

	private void SpawnFish(Transform tile, bool hard)
    {
        GameObject go;
        go = Instantiate(fishPrefab) as GameObject;

		//Parent of fish is the tile
		go.transform.parent = tile;
   

		//Set fish colour based on paramaters or difficulty.

		if (hard) {
			int minC = player.GetComponent<Player> ().getMinColour ();
			if (minC == 0)
				go.GetComponent<SpawnedFish> ().setColour (Color.red);
			else if (minC == 1)
				go.GetComponent<SpawnedFish> ().setColour (Color.green);
			else
				go.GetComponent<SpawnedFish> ().setColour (Color.blue);
		}

		go.transform.position = new Vector3(10, Random.Range(-35f, 7f), Random.Range(-80f, 80f));
    }


    private void SpawnFishHooks(Transform tile) {
		GameObject go = Instantiate (fishHook) as GameObject;

		//Make tile parent of hook
		go.transform.parent = tile;

		//Set position of hook
		go.transform.position = new Vector3(10, Random.Range(-30f, 7f), Random.Range(-100f, 100f));
    }

    private void SpawnSeaweed(Transform tile)
    {
        GameObject go = Instantiate(seaweed) as GameObject;

        Renderer renderer = go.GetComponent<Renderer>();
        Material mat = renderer.material;

        mat.SetFloat("_Offset", Random.Range(0f, 20f));

        go.transform.parent = tile;

        go.transform.position = new Vector3(30, Random.Range(-60f, -50f), Random.Range(-100f, 100f));
    }

}
