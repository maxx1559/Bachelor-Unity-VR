using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Diagnostics;
using System.IO;

public class ChooseMenu : MonoBehaviour
{
    DataBase db = DataBase.getInstance();
    Controller controller = Controller.getInstance();

    public GameObject inputField;
    public Image panel;
    public GameObject chooseMenu;
    public GameObject optionsMenu;
    public Sprite sprite;
    public GameObject text;
    public TMP_Text errorText;

    private LoadOptions loader;
    private void Start()
    {
        loader = GetComponent<LoadOptions>();

        //Check if we just came back from pointcloud scene
        if (db.getFromPoints())
        {
            chooseMenu.SetActive(false);
            optionsMenu.SetActive(true);
            loader.loadOptions();
        }

    }

    public void RunVisualsWithPath()
    /* Creates a JSON file from the chosen .s7k file
    */
    {
        string path = inputField.GetComponent<TMP_InputField>().text;

        //Check if the file path is valid/exists on the system
        if (File.Exists(path))
        {
            //Check if the file is an s7k file
            if(path.Substring(path.Length - 4) == ".s7k")
            {
                //Create the process that runs the python exe (which creates the json file)
                text.SetActive(false);
                Process p = new Process();
                p.StartInfo = new ProcessStartInfo();
                p.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                p.StartInfo.FileName = Application.streamingAssetsPath + @"/DataExtractor.exe";
                p.StartInfo.WorkingDirectory = Application.streamingAssetsPath;
                p.StartInfo.Arguments = @"""" + path + @"""";
                p.Start();
                p.WaitForExit();

                controller.setPath(Application.streamingAssetsPath + @"/point_cloud_data.json");

                controller.LoadController();
                chooseMenu.SetActive(false);
                optionsMenu.SetActive(true);
            }
            else
            {
                errorText.text = "File is not an .s7k file.";
                text.SetActive(true);
            }
        }
        else
        {
            errorText.text = "Path does not exist.";
            text.SetActive(true);
        }

    }

    public void changeBackground()
    /* Changes the background to a new image, only used for when switching to the template menu
    */
    {
        panel.sprite = sprite;
    }

    public void quitButton()
    /* Quits the program
    */
    {
        Application.Quit();
    }

}
