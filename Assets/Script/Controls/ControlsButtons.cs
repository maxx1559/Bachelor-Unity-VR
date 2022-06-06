using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlsButtons : MonoBehaviour
{
    DataBase db = DataBase.getInstance();

    //Method to change scene from pointcloud to options menu scene
    public void backToOptions()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        db.setFromPoints(true);
    }

}
