using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class intro : MonoBehaviour
{
    float timeLine;
    [SerializeField] private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        timeLine = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - timeLine > 2.65)
        {
            Instantiate(player, new Vector3(1.817f, -0.488f, 0f), new Quaternion(0,0,0,0));
            Destroy(this.gameObject);
        }
    }
}
