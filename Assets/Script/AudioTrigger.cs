using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    AudioSource aud;
    Collider2D coll;
    [SerializeField] string tag;
    [SerializeField] bool playOnce;
    bool played = false;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
        aud = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.tag == tag && !played)
        {
            aud.Play();
            if(playOnce){
                played = true;
            }
        }
    }
}
