using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{

    [SerializeField] string newscene;
    Collider2D coll; 

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        Debug.Log("Hit");
        if(coll.gameObject.tag == "Princess")
        {
            Debug.Log("Princess");
            SceneManager.LoadScene(newscene);
        }
    }
}
