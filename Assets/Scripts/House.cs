using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class House : MonoBehaviour
{
    [SerializeField]private string sceneToLoad = "level1EndQuestions";
    private AudioSource LoadNewLevel;
    //public GameObject ui;
    
    private void Start()
    {
        LoadNewLevel = GetComponent<AudioSource>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            LoadNewLevel.Play();
            StartCoroutine(Load());
        }
    }

    IEnumerator Load()
    {
        yield return new WaitWhile(() => LoadNewLevel.isPlaying);
        //ui.SetActive(false);
        SceneManager.LoadScene(sceneToLoad);
    }
}
