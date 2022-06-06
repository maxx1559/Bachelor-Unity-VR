using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointColorControls : MonoBehaviour
{
    DataBase db = DataBase.getInstance();

    Toggle toggle;
    public GameObject scale;
    public Toggle toggleHeightMap;

    void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(delegate { valueChangeCheck(); });
        scale.SetActive(toggle.isOn);
    }

    private void valueChangeCheck()
    {
        db.setPointCloudGradient(toggle.isOn);
        db.setUpdatePointColor(true);
        if (toggleHeightMap.isOn)
        {
            scale.SetActive(true);
        }
        else
        {
            scale.SetActive(toggle.isOn);
        }

    }

}
