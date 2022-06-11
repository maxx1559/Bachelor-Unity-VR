using UnityEngine;
using TMPro;

[RequireComponent(typeof(Canvas))]
public class GeneralControls : MonoBehaviour
{
    DataBase db = DataBase.getInstance();

    Canvas canvas;
    TMP_Text pointsCount;
    TMP_Text trianglesCount;
    TMP_Text trianglesText;
    bool display = true;

    void Start()
    /* Setting number of points, and number of triangles if enabled.
    */
    {
        canvas = GetComponent<Canvas>();
        pointsCount = GameObject.Find("PointsCount").GetComponent<TMP_Text>();
        trianglesText = GameObject.Find("TrianglesText").GetComponent<TMP_Text>();
        trianglesCount = GameObject.Find("TrianglesCount").GetComponent<TMP_Text>();

        pointsCount.text = db.getPoints().Count.ToString();

        if (db.getTriangulationEnabled())
        {
            trianglesCount.text = db.getTriangles().Count.ToString();
        } else
        {
            trianglesText.text = "";
            trianglesCount.text = "";
        }

    }

    void Update()
    /* Checks if "h" key is pressed to either hide or show the controls display
    */
    {
        if (Input.GetKeyDown("h"))
        {
            display = !display;
            canvas.enabled = display;
        }

    }

}
