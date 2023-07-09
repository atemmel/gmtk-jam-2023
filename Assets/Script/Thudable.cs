using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thudable : MonoBehaviour
{

	public AudioSource[] thuds;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
		var index = Random.Range (0, thuds.Length);
		var thud = thuds[index];
		thud.Play();
    }
}
