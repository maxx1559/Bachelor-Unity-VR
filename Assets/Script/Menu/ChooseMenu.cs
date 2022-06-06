using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Diagnostics;
using System.IO;

public class ChooseMenu : MonoBehaviour
{
    Controller controller = Controller.getInstance();
    DataBase db = DataBase.getInstance();

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

    //Creates a Json file from the .s7k file
    public void RunVisualsWithPath()
    {
        string path = inputField.GetComponent<TMP_InputField>().text;
        print(path);
        print(File.Exists(path));

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
                p.StartInfo.FileName = Application.streamingAssetsPath + @"/CreateJSON.exe";
                p.StartInfo.WorkingDirectory = Application.streamingAssetsPath;
                p.StartInfo.Arguments = @"""" + path + @"""";
                p.Start();
                p.WaitForExit();

                controller.setPath(Application.streamingAssetsPath + @"/7k_data_extracted_rotated.json");

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

    //Changes the background to a new image (only used for when we switch to TemplateMenu)
    public void changeBackground()
    {
        panel.sprite = sprite;
    }

    //Quits the program
    public void quitButton()
    {
        Application.Quit();
    }

}
