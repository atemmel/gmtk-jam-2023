using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class draggable : MonoBehaviour
{
    bool grabbed = false;

    Rigidbody2D rigid;

    float originalGrav = 0;
    float interpolator = 0;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        originalGrav = rigid.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        checkIfGrab();
        moveGrab();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            grabbed = false;
        }
    }

    //Mouse actions
    Vector3 getWorldMouse()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void checkIfGrab()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = getWorldMouse();
            Debug.Log(mousePos);
            float topY = transform.position.y + transform.localScale.y/2;
            float botY = transform.position.y - transform.localScale.y/2;
            float rightX = transform.position.x + transform.localScale.x/2;
            float leftX = transform.position.x - transform.localScale.x/2;

            if(mousePos.y < topY && mousePos.y > botY && mousePos.x < rightX && mousePos.x > leftX)
            {
                originalGrav = rigid.gravityScale;
                rigid.gravityScale = 0;
                grabbed = true;
            }
        }
    }

    void moveGrab()
    {
        if(grabbed && Input.GetMouseButton(0))
        {
            Debug.Log("it gets in");
            Vector3 newPos = getWorldMouse();
            newPos.z = 0;
            transform.position = Vector3.Lerp(transform.position, newPos, interpolator);
            transform.rotation = new Quaternion(0, 0, 0, 0);

            if((transform.position - newPos) == Vector3.zero)
            {
                interpolator = 0;
            }
            if(interpolator < 1.0f)
            {
                interpolator += 0.5f * Time.deltaTime;
            }
        }
        else
        {
            grabbed = false;
            interpolator = 0;
            rigid.gravityScale = originalGrav;
        }
    }
}
