using UnityEngine;
using System;
using System.Collections.Generic;



public class LevelScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        grid = GameObject.Find("DeformableGrid");
        if (grid != null)
        {
            gridScript = grid.GetComponent<GridScript>();
        }

        startTime = Time.time;

        deltas = new List<float>();

        deltas.Add(2);
        //deltas.Add((int)Random.Range(1, 10));

        //bounds = new Bounds();
        //Mesh mesh = GetComponent<MeshFilter>().mesh;
        //if(mesh != null)
        //{
        //    bounds.Encapsulate(mesh.bounds);
        //}
    }

    // Update is called once per frame
    void Update () {
        int now = System.DateTime.Now.Second;

        int i = 0;
        while(i < deltas.Count)
        {
            if (now - startTime >= deltas[i])
            {
                GameObject[] objs = GameObject.FindGameObjectsWithTag("Rippler");

                for(int j= 0; j < objs.Length; ++j)
                {
                    Vector3 point = objs[j].transform.position;
                    //point.x = Random.Range(-bounds.extents.x, bounds.extents.x);
                    //point.y = Random.Range(-bounds.extents.y, bounds.extents.y);
                    //point.z = Random.Range(-bounds.extents.z, bounds.extents.z);
                    bool forced = false;
                    gridScript.CreateRipple(point, 5.0f, 2f, (float)Math.PI, 0.0f, 1.0f / (-10.0f), 1.0f / (-0.25f), forced);

                }


                //Vector3 point1 = new Vector3(18.98f, 3.0f, -3.33f);
                ////point.x = Random.Range(-bounds.extents.x, bounds.extents.x);
                ////point.y = Random.Range(-bounds.extents.y, bounds.extents.y);
                ////point.z = Random.Range(-bounds.extents.z, bounds.extents.z);
                //gridScript.CreateRipple(point1, 10.0f, 2f, (float)Math.PI, 0.0f, 1.0f / (-10.0f), 1.0f / (-0.25f));
                //Vector3 point2 = new Vector3(16.37f, 5.994141f, 1.33f);
                //gridScript.CreateRipple(point2, 10.0f, 2f, (float)Math.PI, 0.0f, 1.0f / (-10.0f), 1.0f / (-0.25f));
                deltas.RemoveAt(i);
            }
            ++i;
        }
    }

    private GameObject grid;
    private GridScript gridScript;
    private float startTime;
    //private Bounds bounds;
    private List<float> deltas;
}
