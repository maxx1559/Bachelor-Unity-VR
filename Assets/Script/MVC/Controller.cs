using DelaunatorSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using Accord.Collections;
using UnityEngine.Networking;
using System.Collections;

public class Controller
{
    public static Controller controller = new Controller();

    DataBase db = DataBase.getInstance();
    Sonar sonarData;

    public Mesh mesh = null;
    public GameObject toggleGroup;

    string fileName;
    private Controller()
    {
        //Debug.Log("Controller");
    }


    public void LoadController()
    {
        //Debug.Log("Load controller");
        if (String.IsNullOrEmpty(fileName))
        {
            fileName = @"C:\Users\kanne\Desktop\7k_data_extracted_rotated.json";
            //fileName = @"C:\Users\Max\Desktop\7k_data_extracted_rotated.json";
            //fileName = @"C:\Users\jacob\OneDrive\Dokumenter\GitHub\bachelor_project_teledyne\7k_data_extracted_rotated.json";
        }
        WWW reader = new WWW(fileName);
        while (!reader.isDone) { }

        string jsonString = reader.text;
        //string jsonString = File.ReadAllText(fileName);
        sonarData = JsonConvert.DeserializeObject<Sonar>(jsonString);

        db.setNumberOfPings(sonarData.no_pings);

        //setting min and max values from pointcloud in database
        db.setShallowDepth(sonarData.minimum_depth);
        db.setDeepDepth(sonarData.maximum_depth);
        db.setMinLength(sonarData.min_length_axis);
        db.setMaxLength(sonarData.max_length_axis);
        db.setMinWidth(sonarData.min_width_axis);
        db.setMaxWidth(sonarData.max_width_axis);

        //Changing values for sliders to be more user friendly (From 0 to x, instead of -x to x)
        db.setSliderValueShallowDepth(Math.Abs(db.getShallowDepth()));
        db.setSliderLimitShallowDepth(Math.Abs(db.getShallowDepth()));
        db.setSliderValueDeepDepth(Math.Abs(db.getDeepDepth()));
        db.setSliderLimitDeepDepth(Math.Abs(db.getDeepDepth()));
        db.setSliderValueMinLength(0);
        db.setSliderLimitMinLength(0);

        //Different methods if the min length is positive instead of negative negative
        if (db.getMinLength() < 0)
        {
            db.setSliderValueMaxLength(db.getMaxLength() + Math.Abs(db.getMinLength()));
            db.setSliderLimitMaxLength(db.getMaxLength() + Math.Abs(db.getMinLength()));
        }
        else
        {
            db.setSliderValueMaxLength(db.getMaxLength() - db.getMinLength());
            db.setSliderLimitMaxLength(db.getMaxLength() - db.getMinLength());
        }

        //The width slider has values from -z to z, since the boat is roughly at z = 0 when sending out a ping
        //So values will go from negative to the left of boat and positive to the right
        db.setSliderValueMinWidth(db.getMinWidth());
        db.setSliderValueMaxWidth(db.getMaxWidth());
        db.setSliderLimitMinWidth(db.getMinWidth());
        db.setSliderLimitMaxWidth(db.getMaxWidth());

    }

    //Method to load all the points from the JSON file, and filter away points from the chosen option filters
    public void PointLoader()
    {
        // Variables for point coordinates
        Vector3 point;
        List<Vector3> points = new List<Vector3>();
        List<IPoint> pointsDelaunay = new List<IPoint>();
        List<Vector3> boatPathPoints = new List<Vector3>();

        // Variables for outlier detections
        List<Vector3> newPoints = new List<Vector3>();
        List<float> heightOutlierDetectionList = new List<float>();
        List<double[]> kDTreeSetupList = new List<double[]>();

        //Getting values for pointloader into variables, to avoid excessive calls to the database class
        //and transforming them back from 0 to x to the original -x to x form the points are on
        int finalShallowDepth = -db.getSliderValueShallowDepth();
        int finalDeepDepth = -db.getSliderValueDeepDepth();
        int finalMinLengthAxis = db.getSliderValueMinLength() + db.getMinLength();
        int finalMaxLengthAxis = db.getSliderValueMaxLength() + db.getMinLength();
        int finalMinWidthAxis = db.getSliderValueMinWidth();
        int finalMaxWidthAxis = db.getSliderValueMaxWidth();

        //Fetching values from the database
        bool outlierHeightEnabled = db.getOutlierHeightEnabled();
        bool nearestNeighbourEnabled = db.getNearestNeighbourEnabled();
        int numberOfPings = db.getNumberOfPings();

        //Storing new min and max depth for correct colours in the color height map mesh
        // since the original min and max depth can be filtered away in the pointloader
        int newShallowDepth = int.MinValue + 1;
        int newDeepDepth = int.MaxValue - 1;

        for (int i = 0; i < numberOfPings; i++)
        {
            /* Calculated wrong in the python program
            boatPoint = new Vector3((float)sonarData.pings[i].ping_boat_coord[0], 
                                    (float)sonarData.pings[i].ping_boat_coord[2], 
                                    (float)sonarData.pings[i].ping_boat_coord[1]);
            */

             for (int j = 0; j < sonarData.pings[i].no_points; j++)
            {
                // Setting coordinates for the single current point
                point = new Vector3((float)sonarData.pings[i].coords_x[j], (float)sonarData.pings[i].coords_z[j], (float)sonarData.pings[i].coords_y[j]);

                //Filtering away values not included in the sliders
                if ((point[1] < finalShallowDepth && point[1] > finalDeepDepth)
                    && (point[0] > finalMinLengthAxis && point[0] < finalMaxLengthAxis)
                    && (point[2] > finalMinWidthAxis && point[2] < finalMaxWidthAxis))
                {
                    // adding point to pointcloud
                    points.Add(point);
                    
                    if (!outlierHeightEnabled && !nearestNeighbourEnabled)
                    {
                        pointsDelaunay.Add(new DelaunatorSharp.Point(point[0], point[2]));

                        //Finding the new shallow and deep depth values for color height map
                        if (newShallowDepth < point[1])
                        {
                            newShallowDepth = (int)Math.Ceiling(point[1]);

                        }
                        else if (newDeepDepth > point[1])
                        {
                            newDeepDepth = (int)Math.Floor(point[1]);
                        }
                    }
                    // Adding point to other lists for outlier removal functions to safe running over all points multiple times
                    if (outlierHeightEnabled)
                    {
                        heightOutlierDetectionList.Add(point[1]);
                    }

                    if (nearestNeighbourEnabled)
                    {
                        double[] toKDTreePoint = new double[] { point[0], point[1], point[2] };
                        kDTreeSetupList.Add(toKDTreePoint);
                    }
                    
                }

            }

        }

        if (!outlierHeightEnabled && !nearestNeighbourEnabled)
        {
            db.setPoints(points);
            db.setPointsDelauney(pointsDelaunay);
            db.setNewShallowDepth(newShallowDepth);
            db.setNewDeepDepth(newDeepDepth);
        }

        if (outlierHeightEnabled)
        {
            float sumMean = 0;
            double sumStd = 0;
            float mean = 0;
            double standardDeviation = 0;
            int n = heightOutlierDetectionList.Count;
            double outlierHeightThreshold = db.getOutlierHeightThreshold();

            //Summin over all height values to calculate the mean height
            for (int i = 0; i < n; i++)
            {
                sumMean += heightOutlierDetectionList[i];
            }

            //If there exists more than 1 element in the height list, then the mean and standard deviation can be calculated
            if (n > 0)
            {
                mean = sumMean / n;

                for (int i = 0; i < n; i++)
                {
                    sumStd += Math.Pow(Math.Abs(heightOutlierDetectionList[i] - mean), 2);
                }

                standardDeviation = sumStd / n;
            }

            //Checking all points in the height list
            for (int i = 0; i < n; i++)
            {

                //Points with a z score higher than the defined threshold will not be added to the new point list
                if (Math.Abs((heightOutlierDetectionList[i] - mean) / standardDeviation) < outlierHeightThreshold)
                {
                    newPoints.Add(points[i]);

                    if (!nearestNeighbourEnabled)
                    {
                        pointsDelaunay.Add(new DelaunatorSharp.Point(points[i].x, points[i].z));

                        //Finding the new shallow and deep depth values for color height map
                        if (newShallowDepth < points[i].y)
                        {
                            newShallowDepth = (int)Math.Ceiling(points[i].y);

                        }
                        else if (newDeepDepth > points[i].y)
                        {
                            newDeepDepth = (int)Math.Floor(points[i].y);
                        }

                    }

                }

            }

            points = newPoints;

            if (!nearestNeighbourEnabled)
            {
                db.setPoints(newPoints);
                db.setPointsDelauney(pointsDelaunay);
                db.setNewShallowDepth(newShallowDepth);
                db.setNewDeepDepth(newDeepDepth);
            }

        }

        if (nearestNeighbourEnabled)
        {
            int numberOfNeighbours = db.getNumberOfNeighbours();
            double neighbourDistance = db.getNeighbourDistance();
            newPoints = new List<Vector3>();
            double[][] kDTreeSetupArray = kDTreeSetupList.ToArray();
            KDTree<int> kDTree = KDTree.FromData<int>(kDTreeSetupArray);

            for (int i = 0; i < points.Count; i++)
            {

                double[] currPoint = new double[] { points[i].x, points[i].y, points[i].z };
                List<NodeDistance<KDTreeNode<int>>> neighbours = kDTree.Nearest(currPoint, radius: neighbourDistance);

                if (neighbours.Count > numberOfNeighbours)
                {
                    newPoints.Add(points[i]);
                    pointsDelaunay.Add(new DelaunatorSharp.Point(points[i].x, points[i].z));

                    //Finding the new shallow and deep depth values for color height map
                    if (newShallowDepth < points[i].y)
                    {
                        newShallowDepth = (int)Math.Ceiling(points[i].y);

                    }
                    else if (newDeepDepth > points[i].y)
                    {
                        newDeepDepth = (int)Math.Floor(points[i].y);
                    }

                }

            }

            db.setPoints(newPoints);
            db.setPointsDelauney(pointsDelaunay);
            db.setNewShallowDepth(newShallowDepth);
            db.setNewDeepDepth(newDeepDepth);
        }

    }


    public static Controller getInstance()
    {
        return controller;
    }

    public void setPath(string newPath)
    {
        if (String.IsNullOrEmpty(newPath))
        {
            Debug.Log("newPath is null or empty!");
            Debug.Log(newPath);
            fileName = @"C:\Users\jacob\OneDrive\Dokumenter\GitHub\bachelor_project_teledyne\7k_data_extracted_rotated.json";

        }
        else
        {
            fileName = newPath;
        }

    }

}
