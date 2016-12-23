using UnityEngine;
using System.Collections;

public class MarkerScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Fluctuate(true);
        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
        {
            markerMaterial = rend.material;
            if (markerMaterial != null)
            {
                markerShader = markerMaterial.shader;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {

        Vector3 yAxis = new Vector3(0.0f, 1.0f, 0.0f);
        if (fluctuating)
        {
            float dt = Time.time - t0;
            float power = Mathf.Cos(omega * dt);
            float offset = amplitude * power;

            gameObject.transform.localPosition += offset * yAxis;

            if (markerMaterial != null)
            {
                //Color color = markerMaterial.GetColor()
                //markerMaterial.SetColor();

                //power = Mathf.Lerp(power, 1.0f, 0.5f);
                power = Mathf.Abs(power);
                //float power = markerMaterial.GetFloat("_RimPower");
                markerMaterial.SetFloat("_RimPower", 0.5f * power);

            }
        }


    }

    public void Fluctuate(bool on)
    {
        fluctuating = on;
        if (on)
        {
            t0 = Time.time;

            return;
        }

        //off
        
    }



    public float omega;
    public float amplitude;

    private float t0;
    private bool fluctuating;
    private Material markerMaterial;
    private Shader markerShader;
}
