using UnityEngine;

public class influenceShaderScript : MonoBehaviour 
{
    public Material InfluenceMaterial;
    public float radius = 1;
    public Color color = Color.white;	

	void Update () 
	{
        InfluenceMaterial.SetVector("_Center", transform.position);
        InfluenceMaterial.SetFloat("_Radius", radius);
        InfluenceMaterial.SetColor("_RadiusColor", color);
    }
}