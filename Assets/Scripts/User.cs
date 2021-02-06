using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class User : House
{
    
    private string sceneToLoad;
    [SerializeField]private AudioSource Fail;
    [SerializeField]private AudioSource Correct;
    //public GameObject permanentUIobj;

    
    
    // Update is called once per frame
    private void Update()
    {
        //permanentUIobj.SetActive(false);
        if (Input.GetKey(KeyCode.A))
        {
            Correct.Play();
            //ui.SetActive(true);
            StartCoroutine(goToLevel2());
        }
        else if (Input.GetKey(KeyCode.B))
        {
            Fail.Play();
            //ui.SetActive(true);
            StartCoroutine(goToSampleScene());
        }
        else if (Input.GetKey(KeyCode.C))
        {
            Fail.Play();
            //ui.SetActive(true);
            StartCoroutine(goToSampleScene());
        }
    }    

    IEnumerator goToSampleScene()
    {
        yield return new WaitWhile(() => Fail.isPlaying);
        sceneToLoad = "SampleScene";
        SceneManager.LoadScene(sceneToLoad);
    }

    IEnumerator goToLevel2()
    {
        yield return new WaitWhile(() => Correct.isPlaying);
        sceneToLoad = "Loading";        
        //permanentUIobj.SetActive(true);
        SceneManager.LoadScene(sceneToLoad);
    }
}
