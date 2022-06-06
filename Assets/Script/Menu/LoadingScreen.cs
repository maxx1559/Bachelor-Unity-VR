using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    Controller controller = Controller.getInstance();

    public GameObject loadingScreen;
    public GameObject optionsScreen;
    public Slider slider;
    public Image panel;
    public Sprite sprite;

    public void loadingScene(int sceneIndex)
    {
        panel.sprite = sprite;
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    IEnumerator LoadAsynchronously (int sceneIndex)
    {
        yield return new WaitForSeconds(1);
        controller.PointLoader();
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            yield return null;
        }
        
    }

}
