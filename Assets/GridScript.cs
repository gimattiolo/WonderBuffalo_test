using UnityEngine;
using System;
using System.Collections.Generic;

public class GridScript : MonoBehaviour {


    void Start() {
        Init();
    }

	// Use this for initialization
	protected void Init () {

        totalAmplitude = 0.0f;

        // create render texture

        canvasTextures = new RenderTexture[2];
        canvasTextures[0] = new RenderTexture(resolution, resolution, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
        //canvasTextures[0].DiscardContents(false, false);
        canvasTextures[1] = new RenderTexture(resolution, resolution, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
        //canvasTextures[1].DiscardContents(false, false);

        worldoffsets = new RenderTexture(resolution, resolution, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);


        clearShader = Shader.Find("Unlit/ClearShader");
        clearMaterial = new Material(clearShader);

        ripplingShader = Shader.Find("Unlit/RipplingShader");
        ripplingMaterial = new Material(ripplingShader);


        Mesh mesh = GetComponent<MeshFilter>().mesh;

        //float[] bboxSizeArray = new float[4] { 0.0f, 0.0f, 0.0f, 1.0f };
        if (mesh != null)
        {
            Bounds bounds = mesh.bounds;


            bbox2DSize = new Vector2();


            // in world reference framework
            Vector3 bbox3DSizeO = new Vector3(2 * bounds.extents.x, 2 * bounds.extents.y, 2 * bounds.extents.z);

            // in object reference framework
            //Vector3 bbox3DSizeO = gameObject.transform.InverseTransformDirection(bbox3DSizeW);

            bbox2DSize[0] = bbox3DSizeO.x;
            bbox2DSize[1] = bbox3DSizeO.z;
 
            //bboxSizeArray[0] = bbox2DSize.x;
            //bboxSizeArray[1] = bbox2DSize.y;
        }

        //ripplingMaterial.SetFloatArray("_BBox", bboxSizeArray);

        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
        {
            gridMaterial = rend.material;
            if(gridMaterial != null)
            {
                gridShader = gridMaterial.shader;
            }
        }

        ripples = new Ripple[maxRipples];

        for(int i = 0; i < ripples.Length; ++i)
        {
            ripples[i].T0 = -1.0f;
        }
        //Reset();
    }

    private void Update()
    {
        UpdateRipples();
    }


    // Update is called once per frame
    public void UpdateRipples () {
        // get current time
        float now = Time.time;

        //Reset();

        Vector4 centerVector = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);

        Vector4 dir = new Vector4(yDir.x, yDir.y, yDir.z, 1.0f);
        gridMaterial.SetVector("_OffsetDirection", dir);

        int index = 0;

        int inputIndex =  0;
        int outputIndex = 0;

        //Graphics.Blit(canvasTextures[1], canvasTextures[0], clearMaterial);
        //Graphics.Blit(canvasTextures[0], canvasTextures[1], clearMaterial);





        for (int i = 0; i < ripples.Length; ++i)
        {
            if(ripples[i].T0 < 0.0f)
            {
                continue;
            }

            if (now - ripples[i].T0 > timeThreshold)
            {
				DeleteRipple(i);
                continue;
            }

            inputIndex = index;
            index = (index + 1) % 2;
            outputIndex = index;

            Ripple ripple = ripples[i];

            ripplingMaterial.SetFloat("_Amplitude", ripple.amplitude);
            ripplingMaterial.SetFloat("_TemporalOmega", ripple.temporalOmega);
            ripplingMaterial.SetFloat("_SpatialOmega", ripple.spatialOmega);
            ripplingMaterial.SetFloat("_Offset", ripple.offset);
            ripplingMaterial.SetFloat("_TemporalDecay", ripple.temporalDecay);
            ripplingMaterial.SetFloat("_SpatialDecay", ripple.spatialDecay);

            ripplingMaterial.SetFloat("_T0", ripple.T0);
            ripplingMaterial.SetFloat("_T", now);

            ripplingMaterial.SetFloat("_TotalAmplitude", totalAmplitude);

            centerVector[0] = ripple.center.x;
            centerVector[1] = ripple.center.y;
            ripplingMaterial.SetVector("_Center", centerVector);

            ripplingMaterial.SetFloat("_Accumulate", Convert.ToSingle(i != 0));

            //ripplingMaterial.SetFloat("_Accumulate", Convert.ToSingle(i));

            // this set _MainTex to canvasTextures[index]
            Graphics.Blit(canvasTextures[inputIndex], canvasTextures[outputIndex], ripplingMaterial);

            //Graphics.Blit(canvasTextures[outputIndex], canvasTextures[inputIndex], clearMaterial);
        }

        Graphics.Blit(canvasTextures[outputIndex], worldoffsets);


        gridMaterial.SetTexture("_WorldPositionOffsets", worldoffsets);
        gridMaterial.SetFloat("_TotalAmplitude", totalAmplitude);
        gridMaterial.SetFloat("_MaxClamp", maxClamp);
    }

    public void Reset()
    {
        Graphics.Blit(canvasTextures[1], canvasTextures[0], clearMaterial);
      Graphics.Blit(canvasTextures[0], canvasTextures[1], clearMaterial);
   }


    private int FindSlot(bool forced)
    {
        float now = Time.time;
        for (int i = 0; i < ripples.Length; ++i)
        {
            Ripple r = ripples[i];
            if (r.T0 == -1.0f)
            {
                return i;
            }

            if (now - r.T0 > timeThreshold)
            {
                return i;
            }

        }

        if (forced)
        {
            int maxIndex = -1;
            float maxDt = -1; ;
            for (int i = 0; i < ripples.Length; ++i)
            {
                Ripple r = ripples[i];

                if (r.T0 == -1.0f)
                {
                    continue;
                }

                if (now - r.T0 > maxDt)
                {
                    maxIndex = i;
                }
            }

            if(0 <= maxIndex && maxIndex < ripples.Length)
            {
                return maxIndex;
            }
        }

        return -1;
    } 

    public bool CreateRipple(Vector3 pointW, float amplitude, float temporalOmega, float spatialOmega, float offset, float temporalDecay, float spatialDecay, bool forced)
    {
        int slot = FindSlot(forced);
        if(slot == -1)
        {
            return false;
        }

		DeleteRipple (slot);

        float now = Time.time;

        Ripple r = new Ripple();

        // transform point into object reference framework
        Vector3 pointO = gameObject.transform.InverseTransformPoint(pointW);

        // remove component along offset direction
        pointO -= Vector3.Dot(pointO, yDir) * yDir;


        r.center[0] = - pointO.x / bbox2DSize[0] + uvTransform[0];
        r.center[1] = - pointO.z / bbox2DSize[1] + uvTransform[1];

        Debug.Assert(!float.IsNaN(r.center[0]));
        Debug.Assert(!float.IsNaN(r.center[1]));
        //r.center[0] = 0.5f;
        //r.center[1] = 0.5f;


        r.amplitude = amplitude;
        r.temporalOmega = temporalOmega;
        r.spatialOmega = spatialOmega;
        r.offset = offset;
        r.temporalDecay = temporalDecay;
        r.spatialDecay = spatialDecay;
        r.T0 = now;
        
        totalAmplitude += amplitude;

        ripples[slot] = r;

        ++NTotal;

        return true;
    }

	public void DeleteRipple(int i)
	{
		if (ripples[i].T0 != -1.0f) 
		{
			totalAmplitude -= ripples[i].amplitude;
			--NTotal;
			ripples[i].T0 = -1.0f;
		}
	}

    public int GetActiveRippleNumber()
    {
        int output = 0; ;
        for(int i =0; i < ripples.Length; ++i)
        {
            if(ripples[i].T0 != -1)
            {
                ++output;
            }
        }

        return output;
    }

    private struct Ripple
    {
        public Vector2 center;
        public float amplitude;
        public float spatialOmega;
        public float temporalOmega;
        public float offset;
        public float temporalDecay;
        public float spatialDecay;

        public float T0;
    }
    public float maxClamp;

    public int maxRipples;
    public float timeThreshold;
    public int resolution;
    
    // in object reference framework
    static private Vector3 yDir = new Vector3(0.0f, 1.0f, 0.0f);

    private RenderTexture worldoffsets;
    private RenderTexture[] canvasTextures;
    private Shader ripplingShader;
    private Material ripplingMaterial;
    private Shader gridShader;
    private Material gridMaterial;
    private Shader clearShader;
    private Material clearMaterial;

    private Ripple[] ripples;
    private int NTotal;
    private float totalAmplitude;

    

    private Vector2 bbox2DSize;

    private static Vector2 uvTransform = new Vector2(0.5f, 0.5f);
}
