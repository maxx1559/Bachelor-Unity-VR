using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PointSizeControls : MonoBehaviour, IPointerUpHandler
{
    DataBase db = DataBase.getInstance();
    Slider slider;
    Text pointSizeText;
    float prevVal = 0;

    void Start()
    {
        slider = GetComponent<Slider>();
        pointSizeText = GameObject.Find("pointSize").GetComponent<Text>();

        slider.onValueChanged.AddListener(delegate { valueChangeCheck(); });
        prevVal = slider.value;
    }

    void valueChangeCheck()
    {
        pointSizeText.text = "Point Size: " + Math.Round((Decimal)slider.value, 2);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (slider.value != prevVal)
        {
            prevVal = slider.value;
            db.setUpdatePointSize(true);
            db.setParticleSize(prevVal);

        }
    }
}
