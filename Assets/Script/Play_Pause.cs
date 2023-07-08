using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class Play_Pause : MonoBehaviour
{
    SplineAnimate spline;
    bool playing = true;

    void Awake()
    {
        spline = GetComponent<SplineAnimate>();
    }

    // Update is called once per frame
    void Update()
    {
        playPause();
    }

    void playPause()
    {
        if(Input.GetKeyDown("space"))
        {
            switch(playing)
            {
                case true:
                    spline.Pause();
                    break;
                default:
                    spline.Play();
                    break;
            }
            playing = !playing;
        }
    }
}
