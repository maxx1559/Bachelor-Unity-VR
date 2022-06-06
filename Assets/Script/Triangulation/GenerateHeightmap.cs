using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(GameObject))]

public class GenerateHeightmap : MonoBehaviour
{
    Controller controller = Controller.getInstance();
    DataBase db = DataBase.getInstance();

    Hashtable map;

    List<Color> colors = new List<Color>();
    public Gradient gradient;
    bool hasRun = false;


    private void Update()
    {

        if (db.getTriangulationEnabled() && !hasRun)
        {
            Mesh mesh = controller.mesh;
            if (colors.Count == 0)
            {
                int finalShallowDepth = db.getNewShallowDepth();
                int finalDeepDepth = db.getNewDeepDepth();

                // Define the colors of mesh
                foreach (Vector3 p in db.getPoints())
                {
                    float height = Mathf.InverseLerp(finalDeepDepth, finalShallowDepth, p[1]);
                    colors.Add(gradient.Evaluate(height));

                }
            }

            mesh.colors = colors.ToArray();

            GetComponent<MeshFilter>().mesh = controller.mesh;
            hasRun = true;
        }
        else if (db.getUpdateHeightMap() && db.getShowHeightMap())
        {
            this.gameObject.GetComponent<Renderer>().enabled = true;

        } else if(db.getTriangulationEnabled() && !db.getShowHeightMap())
        {
            this.gameObject.GetComponent<Renderer>().enabled = false;
        }
    }
}