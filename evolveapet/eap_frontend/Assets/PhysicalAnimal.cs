﻿using UnityEngine;
using System.Collections;

namespace EvolveAPet {
	public class PhysicalAnimal : MonoBehaviour {

		public Animal animal { set; get; }

		private Vector2 lastPos;
		private float delta;
		private bool touched;

		private GameObject animalSkeleton;
		
		void Start() {
			animalSkeleton = transform.FindChild("animal skeleton").gameObject;
			StartCoroutine("twitch");
			StartCoroutine("blink");
		}
		
		void Update() {
			tickle ();
		}
		
		void setTouched(bool b) {
			touched = b;
		}
		
		void tickle() {
			if ( Input.GetMouseButtonDown(0) ) {
				lastPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			} else if ( Input.GetMouseButton(0) ) {
				delta += Mathf.Abs(Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition),lastPos));
				lastPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			} else if ( Input.GetMouseButtonUp(0) ) {
				delta = 0f;
				touched = false;
			}
			
			if ((delta > 20f) && touched) {
				animalSkeleton.GetComponent<Animator>().SetTrigger("Tickle");
				delta = 0f;
				touched = false;
			}
		}
		
		IEnumerator twitch() {
			for(;;) {
				animalSkeleton.GetComponent<Animator>().SetTrigger("Twitch");
				yield return new WaitForSeconds(UnityEngine.Random.Range (10f, 20f));
			}	
		}
		
		IEnumerator blink() {
			for(;;) {
				animalSkeleton.GetComponent<Animator>().SetTrigger("Blink");
				yield return new WaitForSeconds(UnityEngine.Random.Range (2f, 5f));
			}	
		}
		
		public void Build(GameObject parent) {

			//Find transforms
			Transform tParent = parent.transform.FindChild("animal skeleton").transform;

			Transform tBody = tParent.FindChild("body").transform;
			Transform tHead = tParent.FindChild("head").transform;
			Transform tTail = tParent.FindChild("tail").transform;
			Transform tLegs = tParent.FindChild("legs").transform;
			Transform tFrontLeg = tLegs.FindChild("front leg").transform;
			Transform tBackLeg = tLegs.FindChild("back leg").transform;
			Transform tArms = tParent.FindChild("arms").transform;
			Transform tFrontArm = tArms.FindChild("front arm").transform;
			Transform tBackArm = tArms.FindChild("back arm").transform;
			Transform[] tEyes = {tHead.FindChild("eye 0").transform, tHead.FindChild("eye 1").transform, tHead.FindChild("eye 2").transform};
			Transform tFrontEar = tHead.FindChild("front ear").transform;
			Transform tBackEar = tHead.FindChild("back ear").transform;
			
			//Build body, rotate if bipedal and scale/colour as needed
			Torso t = (Torso)animal.BodyPartArray[3];
			GameObject body = (GameObject)Instantiate(Resources.Load ("Prefabs/Animals/"+t.shape+"/body"));
			body.GetComponent<SpriteRenderer>().color = DecodeCol(t.colour);
			SpriteRenderer spots = body.transform.FindChild("spots").GetComponent<SpriteRenderer>();
			SpriteRenderer stripes = body.transform.FindChild("stripes").GetComponent<SpriteRenderer>();
			spots.color = DecodeCol (t.colour);
			stripes.color = DecodeCol (t.colour);
			switch (t.pattern) {
			case 0: 
				spots.enabled = false;
				stripes.enabled = false;
				break;
			case 1:
				stripes.enabled = false;
				break;
			case 2:
				spots.enabled = false;
				break;
			}
			if (!((Arms)animal.BodyPartArray[4]).isQuadrupedal) {
				body.transform.Rotate(new Vector3(0,0,-30));
			}
			body.transform.parent = tBody;
			Scale(tBody, t.size);
			
			//build head, color eyes, choose mouth and scale/colour as needed
			Head h = (Head)animal.BodyPartArray[2];
			GameObject head = (GameObject)Instantiate(Resources.Load ("Prefabs/Animals/"+h.shape+"/head"));
			head.GetComponent<SpriteRenderer>().color = DecodeCol (h.colour);
			spots = head.transform.FindChild("spots").GetComponent<SpriteRenderer>();
			stripes = head.transform.FindChild("stripes").GetComponent<SpriteRenderer>();
			spots.color = DecodeCol (h.colour);
			stripes.color = DecodeCol (h.colour);
			switch (h.pattern) {
			case 0: 
				spots.enabled = false;
				stripes.enabled = false;
				break;
			case 1:
				stripes.enabled = false;
				break;
			case 2:
				spots.enabled = false;
				break;
			}
			head.transform.position = tHead.position = body.transform.FindChild ("head joint").transform.position;
			head.transform.parent = tHead;
			
			//build eyes
			Eyes e = (Eyes)animal.BodyPartArray[1];
			for (int i = 0; i<e.number; i++) {
				GameObject eye = (GameObject)Instantiate (Resources.Load ("Prefabs/Animals/"+e.shape+"/eye"));
				eye.transform.position = tEyes[i].position = head.transform.FindChild("eye joint "+i).transform.position;
				eye.transform.parent = tEyes[i];
				Scale (tEyes[i], e.size);
				eye.GetComponent<SpriteRenderer>().color = DecodeCol (e.colour);
			}
			
			//build ears
			Ears ea = (Ears)animal.BodyPartArray[0];
			GameObject ear = (GameObject)Instantiate(Resources.Load ("Prefabs/Animals/"+ea.shape+"/front ear"));
				ear.GetComponent<SpriteRenderer>().color = DecodeCol (ea.colour);
				ear.transform.position = tFrontEar.position = head.transform.FindChild ("ear joint").transform.position;
				ear.transform.parent = tFrontEar;
				Scale(tFrontEar, ea.size);

			ear = (GameObject)Instantiate(Resources.Load ("Prefabs/Animals/"+ea.shape+"/back ear"));
				ear.GetComponent<SpriteRenderer>().color = DecodeCol (ea.colour);
				ear.transform.position = tBackEar.position = head.transform.FindChild ("ear joint").transform.position;
				ear.transform.parent = tBackEar;
				Scale(tBackEar, ea.size);
			
			Scale(tHead, h.size);
			
			//build mouth
			if (h.teethShape == "HERBIVORE") {
				head.transform.FindChild ("c mouth").GetComponent<SpriteRenderer>().enabled = false;
			}

			//build tail, scale/colour as needed
			Tail ta = (Tail)animal.BodyPartArray[6];
			GameObject tail = (GameObject)Instantiate(Resources.Load ("Prefabs/Animals/"+ta.shape+"/tail"));
			tail.GetComponent<SpriteRenderer>().color = DecodeCol (ta.colour);
			spots = tail.transform.FindChild("spots").GetComponent<SpriteRenderer>();
			stripes = tail.transform.FindChild("stripes").GetComponent<SpriteRenderer>();
			spots.color = DecodeCol (ta.colour);
			stripes.color = DecodeCol (ta.colour);
			switch (ta.pattern) {
			case 0: 
				spots.enabled = false;
				stripes.enabled = false;
				break;
			case 1:
				stripes.enabled = false;
				break;
			case 2:
				spots.enabled = false;
				break;
			}
			tail.transform.position = tTail.position = body.transform.FindChild ("tail joint").transform.position;
			tail.transform.parent = tTail;
			Scale(tTail, ta.size);
			
			//build legs, scale/colour as needed
			Legs l = (Legs)animal.BodyPartArray[5];
			GameObject leg = (GameObject)Instantiate(Resources.Load ("Prefabs/Animals/"+l.shape+"/front leg"));
			leg.GetComponent<SpriteRenderer>().color = DecodeCol(l.colour);
			spots = leg.transform.FindChild("spots").GetComponent<SpriteRenderer>();
			stripes = leg.transform.FindChild("stripes").GetComponent<SpriteRenderer>();
			spots.color = DecodeCol (l.colour);
			stripes.color = DecodeCol (l.colour);
			switch (l.pattern) {
			case 0: 
				spots.enabled = false;
				stripes.enabled = false;
				break;
			case 1:
				stripes.enabled = false;
				break;
			case 2:
				spots.enabled = false;
				break;
			}
			leg.transform.position = tFrontLeg.position = body.transform.FindChild ("leg joint").transform.position;
			leg.transform.parent = tFrontLeg;
			Scale(tFrontLeg, l.size);
			
			leg = (GameObject)Instantiate(Resources.Load ("Prefabs/Animals/"+l.shape+"/back leg"));
			leg.GetComponent<SpriteRenderer>().color = DecodeCol(l.colour);
			spots = leg.transform.FindChild("spots").GetComponent<SpriteRenderer>();
			stripes = leg.transform.FindChild("stripes").GetComponent<SpriteRenderer>();
			spots.color = DecodeCol (l.colour);
			stripes.color = DecodeCol (l.colour);
			switch (l.pattern) {
			case 0: 
				spots.enabled = false;
				stripes.enabled = false;
				break;
			case 1:
				stripes.enabled = false;
				break;
			case 2:
				spots.enabled = false;
				break;
			}
			leg.transform.position = tBackLeg.position = body.transform.FindChild ("leg joint").transform.position;
			leg.transform.parent = tBackLeg;
			Scale(tBackLeg, l.size);
			
			//build arms, replace with legs if bipedal and scale/colour as needed
			Arms a = (Arms)animal.BodyPartArray[4];
			string limb = a.isQuadrupedal ? "leg" : "arm";
			
			if (a.number != 0) {
				GameObject arm = (GameObject)Instantiate(Resources.Load ("Prefabs/Animals/"+a.shape+"/front "+limb));
				arm.GetComponent<SpriteRenderer>().color = DecodeCol(a.colour);
				spots = arm.transform.FindChild("spots").GetComponent<SpriteRenderer>();
				stripes = arm.transform.FindChild("stripes").GetComponent<SpriteRenderer>();
				spots.color = DecodeCol (a.colour);
				stripes.color = DecodeCol (a.colour);
				switch (a.pattern) {
				case 0: 
					spots.enabled = false;
					stripes.enabled = false;
					break;
				case 1:
					stripes.enabled = false;
					break;
				case 2:
					spots.enabled = false;
					break;
				}
				arm.transform.position = tFrontArm.position = body.transform.FindChild ("arm joint").transform.position;
				arm.transform.parent = tFrontArm;
				if (a.isQuadrupedal) {
					Scale(tFrontArm, l.size);
				} else {
					Scale(tFrontArm, a.size);
				}
				
				arm = (GameObject)Instantiate(Resources.Load ("Prefabs/Animals/"+a.shape+"/back "+limb));
				arm.GetComponent<SpriteRenderer>().color = DecodeCol (a.colour);
				spots = arm.transform.FindChild("spots").GetComponent<SpriteRenderer>();
				stripes = arm.transform.FindChild("stripes").GetComponent<SpriteRenderer>();
				spots.color = DecodeCol (a.colour);
				stripes.color = DecodeCol (a.colour);
				switch (a.pattern) {
				case 0: 
					spots.enabled = false;
					stripes.enabled = false;
					break;
				case 1:
					stripes.enabled = false;
					break;
				case 2:
					spots.enabled = false;
					break;
				}
				arm.transform.position = tBackArm.position = body.transform.FindChild ("arm joint").transform.position;
				arm.transform.parent = tBackArm;
				if (a.isQuadrupedal) {
					Scale(tBackArm, l.size);
				} else {
					Scale(tBackArm, a.size);
				}
			}
			
		}
		
		float[] sizes = {0.9f, 1f, 1.1f};
		
		//Parse the scale ints and scale accordingly
		void Scale (Transform t, int size) {
			t.localScale = new Vector3(sizes[size],sizes[size], 1);
		}
		
		Color DecodeCol (int[] c) {
			return new Color(c[0]/255f, c[1]/255f, c[2]/255f);
		}

	}
}
