using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointCloudControls : MonoBehaviour
{
    DataBase db = DataBase.getInstance();
    Toggle toggle;

    void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(delegate { valueChangeCheck(); });
    }

    private void valueChangeCheck()
    {
        db.setShowPointCloud(toggle.isOn);
        db.setUpdatePointCloud(true);
    }

    void Update()
    {
        
    }
}
