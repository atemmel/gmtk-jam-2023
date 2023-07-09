using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

	public Transform toTrack;
	public Vector3 offset;
	public float followSpeed = 2f;
	private Vector3 oldPos;

    // Start is called before the first frame update
    void Start()
    {
		transform.position = calcTarget();
    }

	Vector3 calcTarget() {
		var t = transform.position;
		var p = toTrack.position;
		return new Vector3(p.x, p.y, t.z) + offset;
	}

    // Update is called once per frame
    void Update()
    {
		var target = calcTarget();
		transform.position = Vector3.Lerp(transform.position, target, followSpeed * Time.deltaTime);
    }
}
