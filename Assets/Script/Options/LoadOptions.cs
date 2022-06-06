using UnityEngine;
using UnityEngine.UI;
using Min_Max_Slider;


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

    public void loadOptions()
    {
        //Check if we just came from the point cloud visualization (clicked "Back to Options")
        if (db.getFromPoints())
        {
            db.setFromPoints(false);
            triangulationToggle.isOn = db.getTriangulationEnabled();
            edgeTrianglesToggle.isOn = db.getEdgeTrianglesRemoved();
            nearestNeighbourToggle.isOn = db.getNearestNeighbourEnabled();
            outlierHeightDetectionToggle.isOn = db.getOutlierHeightEnabled();
        }
        else
        {
            triangulationToggle.isOn = false;
            edgeTrianglesToggle.isOn = false;
            nearestNeighbourToggle.isOn = false;
            outlierHeightDetectionToggle.isOn = false;
        }

        //Setting the slider values in options
        depthSlider.SetLimits(db.getSliderLimitShallowDepth(), db.getSliderLimitDeepDepth());
        depthSlider.SetValues(db.getSliderValueShallowDepth(), db.getSliderValueDeepDepth());
        lengthSlider.SetLimits(db.getSliderLimitMinLength(), db.getSliderLimitMaxLength());
        lengthSlider.SetValues(db.getSliderValueMinLength(), db.getSliderValueMaxLength());
        widthSlider.SetLimits(db.getSliderLimitMinWidth(), db.getSliderLimitMaxWidth());
        widthSlider.SetValues(db.getSliderValueMinWidth(), db.getSliderValueMaxWidth());

    }

}
