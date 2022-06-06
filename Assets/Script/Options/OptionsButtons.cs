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
    public Toggle nnFilter;
    public Toggle odFilter;
    public Toggle tgFilter;
    public TMP_Dropdown triangulation;
    public Toggle edgeTriangleRemoval;
    public TMP_InputField neighbourField;
    public TMP_InputField distanceField;
    public TMP_InputField thresholdField;

    public Image panel;
    public Sprite sprite;

    public void changeBackground()
    {
        panel.sprite = sprite;
        runButton();
    }
    public void runButton()
    {
        resetPointCloudControllerVariables();
        setOptionsVariables();

        print("Saved settings!");

    }

    public void backButton()
    {
        db.setFromPoints(false);
    }

    public void resetOptions()
    {
        DepthSlider.SetValues(db.getSliderLimitShallowDepth(), db.getSliderValueDeepDepth());
        LengthSlider.SetValues(db.getSliderLimitMinLength(), db.getSliderLimitMaxLength());
        WidthSlider.SetValues(db.getSliderLimitMinWidth(), db.getSliderLimitMaxWidth());
        nnFilter.isOn = false;
        odFilter.isOn = false;
        tgFilter.isOn = false;
        triangulation.value = 1;
        edgeTriangleRemoval.isOn = false;
    }

    public void setOptionsVariables()
    {
        //Setting values in the database to the values chosen in the sliders
        db.setSliderValueShallowDepth((int)DepthSlider.Values.minValue);
        db.setSliderValueDeepDepth((int)DepthSlider.Values.maxValue);
        db.setSliderValueMinLength((int)LengthSlider.Values.minValue);
        db.setSliderValueMaxLength((int)LengthSlider.Values.maxValue);
        db.setSliderValueMinWidth((int)WidthSlider.Values.minValue);
        db.setSliderValueMaxWidth((int)WidthSlider.Values.maxValue);

        //Setting values in the database to the values picked in the checkboxes
        db.setOutlierHeightEnabled(odFilter.isOn);
        db.setNearestNeighbourEnabled(nnFilter.isOn);
        db.setTriangulationEnabled(tgFilter.isOn);
        db.setTriangulationType(triangulation.value);
        db.setEdgeTrianglesRemoved(edgeTriangleRemoval.isOn);

        //Setting values in the database to the values in written in the textfields
        // if the values written is not valid, the values is set to the default ones.
        if (int.TryParse(neighbourField.text, out int intResult))
        {
            db.setNumberOfNeighbours(intResult);
        }
        else
        {
            db.setNumberOfNeighbours(20);
        }

        if (float.TryParse(distanceField.text, out float neighbourDistance))
        {
            db.setNeighbourDistance(neighbourDistance);
        }
        else
        {
            db.setNeighbourDistance(1.5);
        }

        if (double.TryParse(thresholdField.text, out double outlierHeightThreshold))
        {
            db.setOutlierHeightThreshold(outlierHeightThreshold);
        }
        else
        {
            db.setOutlierHeightThreshold(1);
        }

        if (db.getTriangulationEnabled())
            db.setShowMesh(true);

    }

    // Resetting the variables in the controller box when running the point cloud
    // to avoid all values when running new pointcloud
    public void resetPointCloudControllerVariables()
    {
        db.setShowMesh(false);
        db.setHeightMapEnabled(false);
        db.setShowHeightMap(false);
        db.setShowPointCloud(true);
        db.setParticleSize(0.05f);
        db.setUpdateHeightMap(false);
        db.setUpdateOceanFloor(false);
        db.setUpdatePointCloud(false);
        db.setUpdatePointSize(false);
        db.setPointCloudGradient(false);
        db.setUpdatePointColor(false);
    }

}
