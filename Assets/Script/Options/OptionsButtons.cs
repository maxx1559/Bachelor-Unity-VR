using UnityEngine;
using UnityEngine.UI;
using Min_Max_Slider;
using TMPro;

public class OptionsButtons : MonoBehaviour
{
    DataBase db = DataBase.getInstance();

    public MinMaxSlider DepthSlider;
    public MinMaxSlider LengthSlider;
    public MinMaxSlider WidthSlider;
    public Toggle toggleTriangulation;
    public Toggle toggleNearestNeighbour;
    public Toggle toggleHeightOutlier;
    public Toggle toggleEdgeTriangleRemoval;
    public TMP_Dropdown triangulation;
    public TMP_InputField noOfNeighbourInputField;
    public TMP_InputField neighbourDistanceInputField;
    public TMP_InputField outlierHeightThresholdInputField;
    public Image panel;
    public Sprite sprite;

    public void ChangeBackground()
    {
        panel.sprite = sprite;
        RunButton();
    }
    public void RunButton()
    {
        ResetPointCloudControllerVariables();
        setOptionsVariables();
    }

    public void BackButton()
    {
        db.setFromPoints(false);
    }

    public void setOptionsVariables()
    /* When the run button is pressed, all the chosen values in options will be set in
       the database
    */
    {
        // Setting values in the database to the values chosen in the sliders
        db.setSliderValueShallowDepth((int)DepthSlider.Values.minValue);
        db.setSliderValueDeepDepth((int)DepthSlider.Values.maxValue);
        db.setSliderValueMinLength((int)LengthSlider.Values.minValue);
        db.setSliderValueMaxLength((int)LengthSlider.Values.maxValue);
        db.setSliderValueMinWidth((int)WidthSlider.Values.minValue);
        db.setSliderValueMaxWidth((int)WidthSlider.Values.maxValue);

        // Setting values in the database to the values picked in the checkboxes
        db.setOutlierHeightEnabled(toggleHeightOutlier.isOn);
        db.setNearestNeighbourEnabled(toggleNearestNeighbour.isOn);
        db.setTriangulationEnabled(toggleTriangulation.isOn);
        db.setTriangulationType(triangulation.value);
        db.setEdgeTrianglesRemovalEnabled(toggleEdgeTriangleRemoval.isOn);

        // Setting values in the database to the values in written in the textfields
        // if the values written is not valid, the values is set to the default ones.
        if (int.TryParse(noOfNeighbourInputField.text, out int intResult))
        {
            db.setNumberOfNeighbours(intResult);
        }
        else
        {
            db.setNumberOfNeighbours(db.getDefaultNumberOfNeighbours());
        }

        if (double.TryParse(neighbourDistanceInputField.text, out double neighbourDistance))
        {
            db.setNeighbourDistance(neighbourDistance);
        }
        else
        {
            db.setNeighbourDistance(db.getDefaultNeighbourDistance());
        }

        if (double.TryParse(outlierHeightThresholdInputField.text, out double outlierHeightThreshold))
        {
            db.setOutlierHeightThreshold(outlierHeightThreshold);
        }
        else
        {
            db.setOutlierHeightThreshold(db.getDefaultOutlierHeightThreshold());
        }

        if (db.getTriangulationEnabled())
        {
            db.setShowMesh(true);
        }

    }

    public void ResetButton()
    /* Action for the reset button, sets every checkbox to unchecked, and resets all
       values to their default value.
    */
    {
        // Setting values for sliders
        DepthSlider.SetValues(db.getSliderLimitShallowDepth(), db.getSliderLimitDeepDepth());
        LengthSlider.SetValues(db.getSliderLimitMinLength(), db.getSliderLimitMaxLength());
        WidthSlider.SetValues(db.getSliderLimitMinWidth(), db.getSliderLimitMaxWidth());

        // Setting toggles to unchecked
        toggleNearestNeighbour.isOn = false;
        toggleHeightOutlier.isOn = false;
        toggleTriangulation.isOn = false;
        triangulation.value = 1;
        toggleEdgeTriangleRemoval.isOn = false;

        // Setting values for textfields
        noOfNeighbourInputField.text = db.getDefaultNumberOfNeighbours().ToString();
        neighbourDistanceInputField.text = db.getDefaultNeighbourDistance().ToString();
        outlierHeightThresholdInputField.text = db.getDefaultOutlierHeightThreshold().ToString();
    }

    public void ResetPointCloudControllerVariables()
    /* Resetting the variables in the controller box when running the point cloud
       to avoid all values when running new pointcloud
    */
    {
        db.setShowMesh(false);
        db.setHeightMapEnabled(false);
        db.setShowHeightMap(false);
        db.setShowPointCloud(true);
        db.setShowBoatPathPoints(false);
        db.setParticleSize(0.05f);
        db.setUpdateHeightMap(false);
        db.setUpdateOceanFloor(false);
        db.setUpdatePointCloud(false);
        db.setUpdatePointSize(false);
        db.setPointCloudGradient(false);
        db.setUpdatePointColor(false);
        
    }

}
