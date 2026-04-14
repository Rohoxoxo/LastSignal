using UnityEngine;

public class StarfieldGenerator : MonoBehaviour
{
    public int starCount = 300;
    public float fieldSize = 40f;
    public float minStarSize = 0.05f;
    public float maxStarSize = 0.2f;

    void Awake()
    {
        Material starMat = new Material(Shader.Find("Standard"));
        starMat.color = Color.white;
        starMat.SetFloat("_Glossiness", 0f);
        starMat.SetFloat("_Metallic", 0f);
        starMat.EnableKeyword("_EMISSION");
        starMat.SetColor("_EmissionColor", Color.white);

        for (int i = 0; i < starCount; i++)
        {
            GameObject star = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            star.name = "Star";
            star.transform.parent = transform;
            Destroy(star.GetComponent<SphereCollider>());

            star.transform.position = new Vector3(
                Random.Range(-fieldSize, fieldSize),
                -0.2f,
                Random.Range(-fieldSize, fieldSize)
            );

            float size = Random.Range(minStarSize, maxStarSize);
            star.transform.localScale = Vector3.one * size;
            star.GetComponent<Renderer>().material = starMat;
        }
    }
}
