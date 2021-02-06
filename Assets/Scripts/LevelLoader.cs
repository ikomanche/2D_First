using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : House
{
    public GameObject loadingScreen;
    public Slider slider;
    
    
    public void Loadlevel(string sceneName)
    {
        
        StartCoroutine(LoadAsynchrounously(sceneName));
        //ui.SetActive(true);
    }

    IEnumerator LoadAsynchrounously (string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
           
            float progress = Mathf.Clamp01(operation.progress / .9f);

            slider.value = progress;

            yield return null;
        }
        
    }
}
