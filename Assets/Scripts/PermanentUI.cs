using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PermanentUI : MonoBehaviour
{
    public int Health = 3;
    [SerializeField] public TextMeshProUGUI healthText;

    public int gems = 0;
    [SerializeField] public TextMeshProUGUI gemText;

    public int cherries = 0;
    [SerializeField] public TextMeshProUGUI cherryText;

    public static PermanentUI perm;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        //Singleton
        if (!perm)
        {
            perm = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reset()
    {
        Health = 3;
        healthText.text = Health.ToString();
        cherries = 0;
        cherryText.text = cherries.ToString();
        gems = 0;
        gemText.text = gems.ToString();
    }
}
