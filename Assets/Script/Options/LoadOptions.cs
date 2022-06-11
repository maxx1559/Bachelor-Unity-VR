using UnityEngine;
using UnityEngine.UI;
using Min_Max_Slider;
using TMPro;

public class LoadOptions : MonoBehaviour
{
    DataBase db = DataBase.getInstance();

    public MinMaxSlider depthSlider;
    public MinMaxSlider lengthSlider;
    public MinMaxSlider widthSlider;
    public Toggle triangulationToggle;
    public Toggle edgeTrianglesToggle;
    public Toggle nearestNeighbourToggle;
    public Toggle outlierHeightDetectionToggle;
    public TMP_InputField noOfNeighbourInputField;
    public TMP_InputField neighbourDistanceInputField;
    public TMP_InputField outlierHeightThresholdInputField;

    public void loadOptions()
    /* When changing scene to options, all sliders, checkboxes and textfields values must be set.
       If coming from either template- or choosemenu, then the values are set to their default values.
       If coming from point cloud scene, then the values are set to the ones that were set when going
       into the point cloud scene.
    */
    {
        // Check if we just came from the point cloud visualization (clicked "Back to Options")
        if (db.getFromPoints())
        {
            db.setFromPoints(false);
                
            // Setting checkbox values
            triangulationToggle.isOn = db.getTriangulationEnabled();
            edgeTrianglesToggle.isOn = db.getEdgeTrianglesRemovalEnabled();
            nearestNeighbourToggle.isOn = db.getNearestNeighbourEnabled();
            outlierHeightDetectionToggle.isOn = db.getOutlierHeightEnabled();

            // Setting values for textfields
            noOfNeighbourInputField.text = db.getNumberOfNeighbours().ToString();
            neighbourDistanceInputField.text = db.getNeighbourDistance().ToString();
            outlierHeightThresholdInputField.text = db.getOutlierHeightThreshold().ToString();

        }
        else
        {
            // Setting checkbox values
            triangulationToggle.isOn = false;
            edgeTrianglesToggle.isOn = false;
            nearestNeighbourToggle.isOn = false;
            outlierHeightDetectionToggle.isOn = false;

            // Setting values for textfields
            noOfNeighbourInputField.text = db.getDefaultNumberOfNeighbours().ToString();
            neighbourDistanceInputField.text = db.getDefaultNeighbourDistance().ToString();
            outlierHeightThresholdInputField.text = db.getDefaultOutlierHeightThreshold().ToString();
        }

        // Setting the slider values in options
        depthSlider.SetLimits(db.getSliderLimitShallowDepth(), db.getSliderLimitDeepDepth());
        depthSlider.SetValues(db.getSliderValueShallowDepth(), db.getSliderValueDeepDepth());
        lengthSlider.SetLimits(db.getSliderLimitMinLength(), db.getSliderLimitMaxLength());
        lengthSlider.SetValues(db.getSliderValueMinLength(), db.getSliderValueMaxLength());
        widthSlider.SetLimits(db.getSliderLimitMinWidth(), db.getSliderLimitMaxWidth());
        widthSlider.SetValues(db.getSliderValueMinWidth(), db.getSliderValueMaxWidth());

        // Setting placeholder text for text fields as the default value
        noOfNeighbourInputField.placeholder.GetComponent<TMP_Text>().text = "Default: " + db.getDefaultNumberOfNeighbours().ToString();
        neighbourDistanceInputField.placeholder.GetComponent<TMP_Text>().text = "Default: " + db.getDefaultNeighbourDistance().ToString();
        outlierHeightThresholdInputField.placeholder.GetComponent<TMP_Text>().text = "Default: " + db.getDefaultOutlierHeightThreshold().ToString();
    }

}
