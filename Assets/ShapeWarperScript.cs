using UnityEngine;
using System;
using System.Collections;

public class ShapeWarperScript : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
		done = false;
		startTime = -1;
		shapeWarpShader = Shader.Find("Unlit/ShapeWarperShader");

		if (shapeWarpShader == null) 
		{
			return;
		}

		shapeWarpMaterial = GetComponent<Renderer> ().material;

		if(other != null)
		{
			otherShapeWarpMaterial = other.GetComponent<Renderer>().material;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
		if (!done) 
		{
			if (Time.time > 5) 
			{
				done  = true;
				StartWarp();
			}
		}


		float t0 = Mathf.Lerp (Time.time, startTime, Convert.ToSingle (startTime >= 0.0f));

		float alpha = Mathf.Lerp(0.0f, 1.0f, (Time.time - t0) / (0.5f * duration));
		float r = Mathf.Lerp(radius, 0.0f, alpha);

		shapeWarpMaterial.SetFloat ("_Radius", r);
		shapeWarpMaterial.SetFloat ("_Alpha", alpha);

		float otherAlpha = Mathf.Lerp(1.0f, 0.0f, (Time.time - (t0 + 0.5f * duration)) / (0.5f * duration));
		float otherR = Mathf.Lerp(radius, 0.0f, otherAlpha);

		otherShapeWarpMaterial.SetFloat ("_Radius", otherR);
		otherShapeWarpMaterial.SetFloat ("_Alpha", otherAlpha);
	}

	public void StartWarp()
	{
		startTime = Time.time;
	}


	public GameObject other;
	public float radius;
	public float duration;



	private Shader shapeWarpShader;
	private Material shapeWarpMaterial;
	private Material otherShapeWarpMaterial;


	private bool done;
	private float startTime;

}
