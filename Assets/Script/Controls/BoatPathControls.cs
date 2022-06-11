using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoatPathControls : MonoBehaviour
{

    DataBase db = DataBase.getInstance();
    Toggle toggle;

    // Start is called before the first frame update
    void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    private void ValueChangeCheck()
    {
        db.setShowBoatPathPoints(toggle.isOn);
        db.setUpdateBoatPath(true);
    }

}
