using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class OptionsUpdate : MonoBehaviour
{
    DataBase db = DataBase.getInstance();

    public TMP_InputField input1;
    public TMP_InputField input2;
    public TMP_InputField input3;

    void Start()
    {
        print("OptionsUpdate ");
        input1.text = db.getNumberOfNeighbours().ToString();
        input2.text = db.getNeighbourDistance().ToString();
        input3.text = db.getOutlierHeightThreshold().ToString();
    }

}
