using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fall : MonoBehaviour
{
    //[SerializeField]private string sceneToLoad;
    private AudioSource fallDead;
    private void Start()
    {
        fallDead = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            fallDead.Play();
            PermanentUI.perm.Reset();
            StartCoroutine(goToScene());
        }
    }

    IEnumerator goToScene()
    {
        yield return new WaitWhile(() => fallDead.isPlaying);
        SceneManager.LoadScene("GameOver"); 
    }
}
