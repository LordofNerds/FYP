using UnityEngine;
using System.Collections;

public class SpawnScene : MonoBehaviour { 

	public GameObject car;
	public GameObject plane;
	public Vector3 groundSize = new Vector3(50.0F, 1.0F, 50.0F);
	Texture2D _tex;
                                        
	// Use this for initialization
	void Start () {
		_tex = Resources.Load("tarmac") as Texture2D;
		//SPAWN THE GROUND
		plane = GameObject.CreatePrimitive (PrimitiveType.Plane);
		plane.renderer.material.mainTexture = _tex;
		plane.transform.position = Vector3.zero;
		plane.transform.localScale += groundSize;

		


		//SPAWN A CAR
		car = (GameObject) Instantiate(Resources.Load("prefabs/Car"), new Vector3(0.0F, 1.0F, 0.0F), Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
