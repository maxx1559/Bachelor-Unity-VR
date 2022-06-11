using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : MonoBehaviour
{
    DataBase db = DataBase.getInstance();

    public Toggle toggle;
    // Only for toggleTriangle method, if changing in toggleEdgeTriangleRemoval,
    // use "toggle" as EdgeTriangleRemovalToggle
    public Toggle edgeTrianglesToggle;
    public TMP_InputField input1;
    public TMP_InputField input2;
    public GameObject text1;
    public GameObject text2;
    public TMP_Dropdown dropdown;
    
    public void NearestNeighbourToggle()
    /* Changing interactable state for neighbour amount and neighbour distance text fields
    */
    {
        input1.interactable = toggle.isOn;
        input2.interactable = toggle.isOn;
    }

    public void HeightOutlierToggle()
    /* Changing interactable state for height outlier threshold text field
    */
    {
        input1.interactable = (toggle.isOn);
    }

    public void TriangulationToggle()
    /* Changing interactable state for dropdown menu and edge triangles removal toggle
    */
    {
        dropdown.interactable = toggle.isOn;
        edgeTrianglesToggle.interactable = toggle.isOn;

        // If Triangulate is unchecked, edge triangles check is also removed
        if(!toggle.isOn)
        {
            edgeTrianglesToggle.isOn = false;
        }

    }

}
