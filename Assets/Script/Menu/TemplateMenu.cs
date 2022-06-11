using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TemplateMenu : MonoBehaviour
{
    DataBase db = DataBase.getInstance();
    Controller controller = Controller.getInstance();

    public TMP_Dropdown dropdown;
    public Image panel;
    public Sprite sprite;
    public TMP_InputField input1;
    public TMP_InputField input2;
    public TMP_InputField input3;

    public void loadTemplate()
    {
        // Checking which template has been picked, then setting default values into database
        if (dropdown.value == 0)
        {
            controller.setPath(Application.streamingAssetsPath + @"/NBS-Snippets-Sensor-WC_JSON.json");
            db.setDefaultNumberOfNeighbours(30);
            db.setDefaultNeighbourDistance(1);
            db.setDefaultOutlierHeightThreshold(1);

        }
        else if (dropdown.value == 1)
        {
            controller.setPath(Application.streamingAssetsPath + @"/NBS-Snippets-Sensor-WC+-+1_JSON.json");
            db.setDefaultNumberOfNeighbours(120);
            db.setDefaultNeighbourDistance(1.3);
            db.setDefaultOutlierHeightThreshold(5);
        } 
        else if (dropdown.value == 2)
        {
            controller.setPath(Application.streamingAssetsPath + @"/20150411_145216_JSON.json");
            db.setDefaultNumberOfNeighbours(20);
            db.setDefaultNeighbourDistance(1.5);
            db.setDefaultOutlierHeightThreshold(3);
        } 
        else if (dropdown.value == 3)
        {
            controller.setPath(Application.streamingAssetsPath + @"/20200407_104315_PDS_JSON.json");
            db.setDefaultNumberOfNeighbours(20);
            db.setDefaultNeighbourDistance(1.5);
            db.setDefaultOutlierHeightThreshold(3);
        } 
        else if (dropdown.value == 4)
        {
            controller.setPath(Application.streamingAssetsPath + @"/Compressed WC - 2_JSON.json");
            db.setDefaultNumberOfNeighbours(25);
            db.setDefaultNeighbourDistance(0.8);
            db.setDefaultOutlierHeightThreshold(2);
        } 
        else if (dropdown.value == 5)
        {
            controller.setPath(Application.streamingAssetsPath + @"/NBS-Snippets-Sensor - 1_JSON.json");
            db.setDefaultNumberOfNeighbours(40);
            db.setDefaultNeighbourDistance(0.9);
            db.setDefaultOutlierHeightThreshold(3);
        } 
        else if (dropdown.value == 6)
        {
            controller.setPath(Application.streamingAssetsPath + @"/NBS-Snippets-Sensor - 2_JSON.json");
            db.setDefaultNumberOfNeighbours(30);
            db.setDefaultNeighbourDistance(0.8);
            db.setDefaultOutlierHeightThreshold(5.5);
        }
        else if (dropdown.value == 7)
        {
            controller.setPath(Application.streamingAssetsPath + @"/SimulationFile_JSON.json");
            db.setDefaultNumberOfNeighbours(50);
            db.setDefaultNeighbourDistance(10);
            db.setDefaultOutlierHeightThreshold(99);
        }

        // Calling the LoadController method in Controller with the chosen JSON file
        controller.LoadController();
    }

    public void ChangeBackground()
    {
        panel.sprite = sprite;
    }

}
