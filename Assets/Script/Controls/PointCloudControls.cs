using UnityEngine;
using UnityEngine.UI;

public class PointCloudControls : MonoBehaviour
{
    DataBase db = DataBase.getInstance();
    
    public GameObject scale;
    Toggle displayToggle;
    public Toggle toggleGradient;
    public Toggle toggleHeightmap;


    void Start()
    {
        displayToggle = GetComponent<Toggle>();
        displayToggle.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
    }

    private void ValueChangeCheck()
    {
        db.setShowPointCloud(displayToggle.isOn);
        db.setUpdatePointCloud(true);

        if(!displayToggle.isOn)
        {
            toggleGradient.interactable = false;
        } else
        {
            toggleGradient.interactable = true;
        }

    }

}
