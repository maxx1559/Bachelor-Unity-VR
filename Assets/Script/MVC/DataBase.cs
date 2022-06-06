using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using DelaunatorSharp;

public class DataBase
{
    public static DataBase db = new DataBase();

    //Setting initial variables 

    //Values from sonar data
    private int numberOfPings = 0;

    //Values for point cloud rendering
    private List<Vector3> points = new List<Vector3>();
    private List<Vector3> boatPathPoints = new List<Vector3>();
    private List<IPoint> pointsDelaunay = new List<IPoint>();
    private List<int> triangles = new List<int>();

    //Values for pointcloud size
    private int shallowDepth;
    private int deepDepth;
    private int newShallowDepth;
    private int newDeepDepth;
    private int minLength;
    private int maxLength;
    private int minWidth;
    private int maxWidth;

    //Values for sliders in options
    private int sliderValueShallowDepth = 0;
    private int sliderValueDeepDepth = 100;
    private int sliderLimitShallowDepth = 0;
    private int sliderLimitDeepDepth = 100;
    private int sliderValueMinLength = 0;
    private int sliderValueMaxLength = 100;
    private int sliderLimitMinLength = 0;
    private int sliderLimitMaxLength = 100;
    private int sliderValueMinWidth = 0;
    private int sliderValueMaxWidth = 100;
    private int sliderLimitMinWidth = 0;
    private int sliderLimitMaxWidth = 100;

    //Values for check boxes in options
    private bool triangulationEnabled = false;
    private bool edgeTrianglesRemoved = false;
    private bool nearestNeighboursEnabled = false;
    private bool outlierHeightEnabled = false;

    //Values for dropdown menu in options
    private int triangulationType = 0;

    //Values for textfields in options
    private int numberOfNeighbours = 20;
    private double neighbourDistance = 1.5;
    private double outlierHeigthThreshold = 1.0;

    //Values for controls in pointcloud scene
    private bool showMesh = false;
    private bool heightMapEnabled = false;
    private bool showHeightMap = false;
    private bool showPointCloud = true;
    private float particleSize = 0.05f;

    // Variables set if game object should update
    private bool updateHeightMap = false;
    private bool updateOceanFloor = false;
    private bool updatePointCloud = false;
    private bool updatePointSize = false;
    private bool pointCloudGradient = false;
    private bool updatePointColor = false;
    private bool fromPoints = false;

    private DataBase() {}
    public static DataBase getInstance()
    {
        return db;
    }
    //Set variables methods
    public void setNumberOfPings(int newNumberOfPings)
    {
        numberOfPings = newNumberOfPings;
    }
    public void setPoints(List<Vector3> newPoints)
    {
        points = newPoints;
    }
    public void setBoatPathPoints(List<Vector3> newBoatPathPoints)
    {
        boatPathPoints = newBoatPathPoints;
    }
    public void setPointsDelauney(List<IPoint> newPointsDelauney)
    {
        pointsDelaunay = newPointsDelauney;
    }
    public void setTriangles(List<int> newTriangles)
    {
        triangles = newTriangles;
    }
    public void setShallowDepth(int newMinDepth)
    {
        shallowDepth = newMinDepth;
    }
    public void setDeepDepth(int newMaxDepth)
    {
        deepDepth = newMaxDepth;
    }
    public void setNewShallowDepth(int newNewShallowDepth)
    {
        newShallowDepth = newNewShallowDepth;
    }
    public void setNewDeepDepth(int newNewDeepDepth)
    {
        newDeepDepth = newNewDeepDepth;
    }
    public void setSliderValueShallowDepth(int newSliderValueShallowDepth)
    {
        sliderValueShallowDepth = newSliderValueShallowDepth;
    }
    public void setSliderValueDeepDepth(int newSliderValueDeepDepth)
    {
        sliderValueDeepDepth = newSliderValueDeepDepth;
    }
    public void setSliderLimitShallowDepth(int newSliderShallowDepth)
    {
        sliderLimitShallowDepth = newSliderShallowDepth;
    }
    public void setSliderLimitDeepDepth(int newSliderLimitDeepDepth)
    {
        sliderLimitDeepDepth = newSliderLimitDeepDepth;
    }
    public void setMinLength(int newMinLength)
    {
        minLength = newMinLength;
    }
    public void setMaxLength(int newMaxLength)
    {
        maxLength = newMaxLength;
    }
    public void setSliderValueMinLength(int newSliderValueMinLength)
    {
        sliderValueMinLength = newSliderValueMinLength;
    }
    public void setSliderValueMaxLength(int newSliderValueMaxLength)
    {
        sliderValueMaxLength = newSliderValueMaxLength;
    }
    public void setSliderLimitMinLength(int newSliderLimitMinLength)
    {
        sliderLimitMinLength = newSliderLimitMinLength;
    }
    public void setSliderLimitMaxLength(int newSliderLimitMaxLength)
    {
        sliderLimitMaxLength = newSliderLimitMaxLength;
    }
    public void setMinWidth(int newMinWidth)
    {
        minWidth = newMinWidth;
    }
    public void setMaxWidth(int newMaxWidth)
    {
        maxWidth = newMaxWidth;
    }
    public void setSliderValueMinWidth(int newSliderValueMinWidth)
    {
        sliderValueMinWidth = newSliderValueMinWidth;
    }
    public void setSliderValueMaxWidth(int newSliderValueMaxWidth)
    {
        sliderValueMaxWidth = newSliderValueMaxWidth;
    }
    public void setSliderLimitMinWidth(int newSliderLimitMinWidth)
    {
        sliderLimitMinWidth = newSliderLimitMinWidth;
    }
    public void setSliderLimitMaxWidth(int newSliderLimitMaxLength)
    {
        sliderLimitMaxWidth = newSliderLimitMaxLength;
    }
    public void setNearestNeighbourEnabled(bool newNearestNeighbour)
    {
        nearestNeighboursEnabled = newNearestNeighbour;
    }
    public void setOutlierHeightEnabled(bool newOutlierHeigth)
    {
        outlierHeightEnabled = newOutlierHeigth;
    }
    public void setNumberOfNeighbours(int newNoOfNeighbours)
    {
        numberOfNeighbours = newNoOfNeighbours;
    }
    public void setNeighbourDistance(double newNeighbourDist)
    {
        neighbourDistance = newNeighbourDist;
    }
    public void setOutlierHeightThreshold(double newOutlierThreshold)
    {
        outlierHeigthThreshold = newOutlierThreshold;
    }
    public void setShowMesh(bool newShowMesh)
    {
        showMesh = newShowMesh;
    }
    public void setTriangulationEnabled(bool newTriangulation)
    {
        triangulationEnabled = newTriangulation;
    }
    public void setTriangulationType(int newTriangulationType)
    {
        triangulationType = newTriangulationType;
    }
    public void setEdgeTrianglesRemoved(bool newEdgeTrianglesRemoved)
    {
        edgeTrianglesRemoved = newEdgeTrianglesRemoved;
    }
    public void setHeightMapEnabled(bool newHeightMapEnabled)
    {
        heightMapEnabled = newHeightMapEnabled;
    }
    public void setShowHeightMap(bool newShowHeightMap)
    {
        showHeightMap = newShowHeightMap;
    }
    public void setShowPointCloud(bool newShowPointCloud)
    {
        showPointCloud = newShowPointCloud;
    }
    public void setUpdateHeightMap(bool newUpdateHeightMap)
    {
        updateHeightMap = newUpdateHeightMap;
    }
    public void setUpdateOceanFloor(bool newUpdateOceanFloor)
    {
        updateOceanFloor = newUpdateOceanFloor;
    }
    public void setUpdatePointCloud(bool newUpdatePointCloud)
    {
        updatePointCloud = newUpdatePointCloud;
    }
    public void setUpdatePointSize(bool newUpdatePointSize)
    {
        updatePointSize = newUpdatePointSize;
    }
    public void setParticleSize(float newParticleSize)
    {
        particleSize = newParticleSize;
    }
    public void setPointCloudGradient(bool newPointCloudGradient)
    {
        pointCloudGradient = newPointCloudGradient;
    }
    public void setUpdatePointColor(bool newUpdatePointColor)
    {
        updatePointColor = newUpdatePointColor;
    }
    public void setFromPoints(bool newFromPoints)
    {
        fromPoints = newFromPoints;
    }


    //Get variables methods
    public int getNumberOfPings()
    {
        return numberOfPings;
    }
    public List<Vector3> getPoints()
    {
        return points;
    }
    public List<Vector3> getBoatPathPoints()
    {
        return boatPathPoints;
    }
    public List<IPoint> getPointsDelauney()
    {
        return pointsDelaunay;
    }
    public List<int> getTriangles()
    {
        return triangles;
    }
    public int getShallowDepth()
    {
        return shallowDepth;
    }
    public int getDeepDepth()
    {
        return deepDepth;
    }
    public int getNewShallowDepth()
    {
        return newShallowDepth;
    }
    public int getNewDeepDepth()
    {
        return newDeepDepth;
    }
    public int getSliderValueShallowDepth()
    {
        return sliderValueShallowDepth;
    }
    public int getSliderValueDeepDepth()
    {
        return sliderValueDeepDepth;
    }
    public int getSliderLimitShallowDepth()
    {
        return sliderLimitShallowDepth;
    }
    public int getSliderLimitDeepDepth()
    {
        return sliderLimitDeepDepth;
    }
    public int getMinLength()
    {
        return minLength;
    }
    public int getMaxLength()
    {
        return maxLength;
    }
    public int getSliderValueMinLength()
    {
        return sliderValueMinLength;
    }
    public int getSliderValueMaxLength()
    {
        return sliderValueMaxLength;
    }
    public int getSliderLimitMinLength()
    {
        return sliderLimitMinLength;
    }
    public int getSliderLimitMaxLength()
    {
        return sliderLimitMaxLength;
    }
    public int getMinWidth()
    {
        return minWidth;
    }
    public int getMaxWidth()
    {
        return maxWidth;
    }
    public int getSliderValueMinWidth()
    {
        return sliderValueMinWidth;
    }
    public int getSliderValueMaxWidth()
    {
        return sliderValueMaxWidth;
    }
    public int getSliderLimitMinWidth()
    {
        return sliderLimitMinWidth;
    }
    public int getSliderLimitMaxWidth()
    {
        return sliderLimitMaxWidth;
    }
    public bool getNearestNeighbourEnabled()
    {
        return nearestNeighboursEnabled;
    }
    public bool getOutlierHeightEnabled()
    {
        return outlierHeightEnabled;
    }
    public int getNumberOfNeighbours()
    {
        return numberOfNeighbours;
    }
    public double getNeighbourDistance()
    {
        return neighbourDistance;
    }
    public double getOutlierHeightThreshold()
    {
        return outlierHeigthThreshold;
    }
    public bool getShowMesh()
    {
        return showMesh;
    }
    public bool getTriangulationEnabled()
    {
        return triangulationEnabled;
    }
    public int getTriangulationType()
    {
        return triangulationType;
    }
    public bool getEdgeTrianglesRemoved()
    {
        return edgeTrianglesRemoved;
    }
    public bool getHeightMapEnabled()
    {
        return heightMapEnabled;
    }
    public bool getShowHeightMap()
    {
        return showHeightMap;
    }
    public bool getShowPointCloud()
    {
        return showPointCloud;
    }

    public bool getUpdateHeightMap()
    {
        return updateHeightMap;
    }
    public bool getUpdateOceanFloor()
    {
        return updateOceanFloor;
    }
    public bool getUpdatePointCloud()
    {
        return updatePointCloud;
    }
    public bool getUpdatePointSize()
    {
        return updatePointSize;
    }

    public float getParticleSize()
    {
        return particleSize;
    }
    public bool getPointCloudGradient()
    {
        return pointCloudGradient;
    }
    public bool getUpdatePointColor()
    {
        return updatePointColor;
    }
    public bool getFromPoints()
    {
        return fromPoints;
    }

}
