using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    DataBase db = DataBase.getInstance();

    public Toggle toggle;
    //Only for toggleTriangle method, if changing in toggleEdgeTriangleRemoval, use "toggle" as EdgeTriangleRemovalToggle
    public Toggle edgeTrianglesToggle;
    //
    public TMP_InputField input1;
    public TMP_InputField input2;
    public GameObject text1;
    public GameObject text2;

    public TMP_Dropdown dropdown;
    
    public void toggleNN()
    {
        db.setNearestNeighbourEnabled(toggle.isOn);
        input1.interactable = toggle.isOn;
        input2.interactable = toggle.isOn;
    }

    public void toggleOD()
    {
        db.setOutlierHeightEnabled(toggle.isOn);
        input1.interactable = (toggle.isOn);
    }

    public void toggleTriangle()
    {
        db.setTriangulationEnabled(toggle.isOn);
        dropdown.interactable = toggle.isOn;
        edgeTrianglesToggle.interactable = toggle.isOn;
    }

    public void toggleEdgeTriangleRemoval()
    {
        db.setEdgeTrianglesRemoved(toggle.isOn);

    }

}
