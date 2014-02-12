﻿using UnityEngine;
using System.Collections;

namespace EvolveAPet {
	public class Builder : MonoBehaviour {

		// Use this for initialization
		void Start () {
			StartCoroutine("Build");
		}
		
		// Update is called once per frame
		void Update () {
		
		}

		IEnumerator Build() {
			//Remove the previous animal
			GameObject.Destroy(GameObject.FindGameObjectWithTag("Animal"));
			Resources.UnloadUnusedAssets();
			
			//Wait one frame for destroys to commit
			yield return new WaitForSeconds(0f);
			
			//Create new animal
			GameObject animal = (GameObject)Instantiate(Resources.Load ("Prefabs/animal"));
			animal.GetComponent<PhysicalAnimal>().animal = new Animal();
			animal.GetComponent<PhysicalAnimal>().Build(animal);
		}

		void OnGUI() {
			if (GUI.Button (new Rect(Screen.width-200,Screen.height-30,200, 30), "Build")) {
				StartCoroutine("Build");
			}
		}
	}
}
