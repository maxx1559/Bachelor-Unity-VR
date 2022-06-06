using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


[RequireComponent(typeof(Canvas))]
public class GeneralControls : MonoBehaviour
{
    Canvas canvas;
    TMP_Text pointsCount;
    TMP_Text trianglesCount;
    TMP_Text trianglesText;
    bool display = true;
    DataBase db = DataBase.getInstance();

    // Start is called before the first frame update
    void Start()
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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("h"))
        {
            display = !display;
            canvas.enabled = display;
        }
            

    }
}
