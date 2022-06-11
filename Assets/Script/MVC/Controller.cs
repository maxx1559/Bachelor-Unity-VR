using DelaunatorSharp;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System;
using UnityEngine;
using Accord.Collections;

public class Controller
{
    public static Controller controller = new Controller();

    DataBase db = DataBase.getInstance();
    Sonar sonarData;

    public Mesh mesh = null;
    public GameObject toggleGroup;

    string fileName;

    public int newShallowDepth;
    public int newDeepDepth;

    public void LoadController()
    /* Method used to load the JSON file, then setting values in the database
       from the JSON file.
    */
    {
        //Works for Android devices (for Oculus Quest 2), makes a web request to find the wanted JSON: 
        WWW reader = new WWW(fileName);
        while (!reader.isDone) { }

        string jsonString = reader.text;
        sonarData = JsonConvert.DeserializeObject<Sonar>(jsonString);

        db.setNumberOfPings(sonarData.no_pings);

        // Setting min and max values from pointcloud in database
        db.setShallowDepth(sonarData.minimum_depth);
        db.setDeepDepth(sonarData.maximum_depth);
        db.setMinLength(sonarData.min_length_axis);
        db.setMaxLength(sonarData.max_length_axis);
        db.setMinWidth(sonarData.min_width_axis);
        db.setMaxWidth(sonarData.max_width_axis);

        // Changing values for sliders to be more user friendly (From 0 to x, instead of -x to x)
        db.setSliderValueShallowDepth(0);
        db.setSliderLimitShallowDepth(0);
        db.setSliderValueDeepDepth(Math.Abs(db.getDeepDepth()));
        db.setSliderLimitDeepDepth(Math.Abs(db.getDeepDepth()));
        db.setSliderValueMinLength(0);
        db.setSliderLimitMinLength(0);

        // Different methods if min and/or max length is negative.
        if (db.getMinLength() < 0 && db.getMaxLength() < 0)
        {
            db.setSliderValueMaxLength(Math.Abs(db.getMinLength()) + db.getMaxLength());
            db.setSliderLimitMaxLength(Math.Abs(db.getMinLength()) + db.getMaxLength());
        }
        else if (db.getMinLength() < 0)
        {
            db.setSliderValueMaxLength(db.getMaxLength() + Math.Abs(db.getMinLength()));
            db.setSliderLimitMaxLength(db.getMaxLength() + Math.Abs(db.getMinLength()));
        }
        else
        {
            db.setSliderValueMaxLength(db.getMaxLength() - db.getMinLength());
            db.setSliderLimitMaxLength(db.getMaxLength() - db.getMinLength());
        }

        // The width slider has values from -z to z, since the boat is roughly at z = 0 when sending out the
        // first ping, so values will go from negative to the left of boat and positive to the right
        db.setSliderValueMinWidth(db.getMinWidth());
        db.setSliderValueMaxWidth(db.getMaxWidth());
        db.setSliderLimitMinWidth(db.getMinWidth());
        db.setSliderLimitMaxWidth(db.getMaxWidth());

    }

    //Method to load all the points from the JSON file, and filter away points from the chosen option filters
    public void PointLoader()
    /* Loads points from the JSON file, removes points outside slider values and then calls
       OutlierHeightDetection and/or NearestNeighbourDetection if enabled from options, else
       stores the values in the database.
    */
    {
        // Variables for point coordinates
        Vector3 point;
        List<Vector3> points = new List<Vector3>();

        // Delauney points is all points' x and z values
        List<IPoint> pointsDelaunay = new List<IPoint>();

        // Lists for nearest neighbour outlier detection
        List<double[]> kDTreePoints = new List<double[]>();

        // Boat path points is location of the boat at every ping
        List<Vector3> boatPathPoints = new List<Vector3>();

        // Resetting values in database
        db.setPoints(points);
        db.setPointsDelauney(pointsDelaunay);
        db.setTriangles(new List<int>());

        // Getting values from database for pointloader variables, to avoid excessive calls to the
        // database class. Then transforming them back from 0 to x to the original min_x to max_x
        // form the points in the JSON file are on.
        int finalShallowDepth = -db.getSliderValueShallowDepth();
        int finalDeepDepth = -db.getSliderValueDeepDepth();
        int finalMinLengthAxis = db.getSliderValueMinLength() + db.getMinLength();
        int finalMaxLengthAxis = db.getSliderValueMaxLength() + db.getMinLength();
        int finalMinWidthAxis = db.getSliderValueMinWidth();
        int finalMaxWidthAxis = db.getSliderValueMaxWidth();

        // Fetching values from the database
        bool outlierHeightEnabled = db.getOutlierHeightEnabled();
        bool nearestNeighbourEnabled = db.getNearestNeighbourEnabled();
        int numberOfPings = db.getNumberOfPings();

        // Center point of point cloud, used for setting the initial camera angle
        Vector3 centerPoint = new Vector3();

        // Storing new min and max depth for correct colours in the color height map mesh
        // since the original min and max depth can be filtered away in the pointloader
        newShallowDepth = int.MinValue + 1;
        newDeepDepth = int.MaxValue - 1;

        int noOfPoints = sonarData.no_counts;

        for (int i = 0; i < numberOfPings; i++)
        {

            boatPathPoints.Add(new Vector3((float)sonarData.pings[i].ping_boat_coord[0],
                                            (float)sonarData.pings[i].ping_boat_coord[2],
                                            (float)sonarData.pings[i].ping_boat_coord[1]));

            for (int j = 0; j < sonarData.pings[i].no_points; j++)
            {
                // Setting coordinates for the single current point
                point = new Vector3((float)sonarData.pings[i].coords_x[j], (float)sonarData.pings[i].coords_z[j], (float)sonarData.pings[i].coords_y[j]);
                centerPoint += point;

                // Filtering away values not included in the sliders
                if ((point[1] < finalShallowDepth && point[1] > finalDeepDepth)
                    && (point[0] > finalMinLengthAxis && point[0] < finalMaxLengthAxis)
                    && (point[2] > finalMinWidthAxis && point[2] < finalMaxWidthAxis))
                {
                    // Adding point to pointcloud
                    points.Add(point);
                    
                    if (!outlierHeightEnabled && !nearestNeighbourEnabled)
                    {
                        pointsDelaunay.Add(new DelaunatorSharp.Point(point[0], point[2]));

                        // Finding the new shallow and deep depth values for color height map
                        if (newShallowDepth < point[1])
                        {
                            newShallowDepth = (int)Math.Ceiling(point[1]);

                        }
                        else if (newDeepDepth > point[1])
                        {
                            newDeepDepth = (int)Math.Floor(point[1]);
                        }
                    }
                    // Adding point to other lists for nearest neighbour detection algorithm
                    if (!outlierHeightEnabled && nearestNeighbourEnabled)
                    {
                        double[] toKDTreePoint = new double[] { point[0], point[1], point[2] };
                        kDTreePoints.Add(toKDTreePoint);
                    }
                    
                }

            }

        }

        // Checking if more than 0 points in sonar file, to avoid division with 0
        if(noOfPoints > 0)
        {
            centerPoint /= noOfPoints;
        }
        
        // Setting centerpoint and boat path points in database
        db.setCenterPoint(centerPoint);
        db.setBoatPathPoints(boatPathPoints);

        // Checking if either outlier height or nearest neighbour is enabled in options
        if (outlierHeightEnabled) 
        {
            OutlierHeightDetection(points);

        } else if (nearestNeighbourEnabled && points.Count > 0)
        {
            NearestNeighbourDetection(points, kDTreePoints.ToArray());
        }
        else
        {
            // Setting values in the database if neither outlier detection is enabled
            db.setPoints(points);
            db.setPointsDelauney(pointsDelaunay);
            db.setNewShallowDepth(newShallowDepth);
            db.setNewDeepDepth(newDeepDepth);
        }

    }

    private void OutlierHeightDetection(List<Vector3> points)
    /* Calculates Z score for all points' y-values, and removes the points with a Z score
       above the chosen threshold.

       Args:
            points: A Vector3 list
    */
    {
        List<Vector3> newPoints = new List<Vector3>();
        List<IPoint> delauneyPoints = new List<IPoint>();
        List<double[]> kDTreePoints = new List<double[]>();
        int n = points.Count;
        bool nearestNeighbourEnabled = db.getNearestNeighbourEnabled();
        double outlierHeightThreshold = db.getOutlierHeightThreshold();
        double mean = 0;
        double standardDeviation = 0;
        double sumMean = 0;
        double sumStd = 0;

        // Summing over all height values to calculate the mean height
        for (int i = 0; i < n; i++)
        {
            sumMean += points[i][1];
        }

        // If there exists more than 1 element in the height list,
        // then the mean and standard deviation can be calculated
        if (n > 0)
        {
            mean = sumMean / n;

            for (int i = 0; i < n; i++)
            {
                sumStd += Math.Pow(points[i][1] - mean, 2);
            }
            standardDeviation = Math.Sqrt(sumStd / n);
        }

        // Checking all points in the height list
        for (int i = 0; i < n; i++)
        {

            // Points with a Z score higher than the defined threshold will not be added to the new point list
            if (Math.Abs((points[i][1] - mean) / standardDeviation) < outlierHeightThreshold)
            {
                newPoints.Add(points[i]);

                if (!nearestNeighbourEnabled)
                {
                    // Adding x and z value to a list for the Delauney triangulation method
                    delauneyPoints.Add(new DelaunatorSharp.Point(points[i][0], points[i][2]));

                    // Finding the new shallow and deep depth values for color height map
                    // only if nearest neighbour is not enabled
                    if (newShallowDepth < points[i].y)
                    {
                        newShallowDepth = (int)Math.Ceiling(points[i][1]);
                    }

                    else if (newDeepDepth > points[i].y)
                    {
                        newDeepDepth = (int)Math.Floor(points[i][1]);
                    }

                } 
                else
                {   
                    // Setting up list for kDtree creation in nearest neighbour method
                    double[] kDTreePoint = new double[] { points[i][0], points[i][1], points[i][2] };
                    kDTreePoints.Add(kDTreePoint);   
                }

            }

        }

        // Storing values in database, unless they will be changed in nearest neighbour method
        if (!nearestNeighbourEnabled)
        {
            db.setPoints(newPoints);
            db.setPointsDelauney(delauneyPoints);
            db.setNewShallowDepth(newShallowDepth);
            db.setNewDeepDepth(newDeepDepth);
        } 
        else if (newPoints.Count > 0)
        {
            NearestNeighbourDetection(newPoints, kDTreePoints.ToArray());
        }

    }

    private void NearestNeighbourDetection(List<Vector3> points, double[][] initialKDTree)
    /* Removes points that have less than the chosen amount of neighbours, within a sphere with
       a specified radius.

       Args:
            points: A Vector3 list
            initial_kDTree: A 2 dimensional double array
    */
    {
        List<Vector3> newPoints = new List<Vector3>();
        List<IPoint> pointsDelaunay = new List<IPoint>();
        KDTree<int> kDTree = KDTree.FromData<int>(initialKDTree);
 
        int numberOfNeighbours = db.getNumberOfNeighbours();
        double neighbourDistance = db.getNeighbourDistance();

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
        // Storing the final values in the database
        db.setPoints(newPoints);
        db.setPointsDelauney(pointsDelaunay);
        db.setNewShallowDepth(newShallowDepth);
        db.setNewDeepDepth(newDeepDepth);
    }

    private Controller() { }
    public static Controller getInstance()
    {
        return controller;
    }

    public void setPath(string newPath)
    {
        fileName = newPath;
    }

}
