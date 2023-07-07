using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SunScript : MonoBehaviour
{
	private Mesh mesh;

    void Start()
    {
		mesh = new Mesh();
		GetComponent<MeshFilter>().mesh = mesh;
    }

    // Update is called once per frame
    void LateUpdate()
    {
		var origin = gameObject.transform.position;
		var fov = 90f;
		var rayCount = 40;
		var angle = 0f;
		var angleIncrease = fov / rayCount;
		var viewDistance = 5f;

		var vertices = new Vector3[rayCount + 1 + 1];
		var uv = new Vector2[vertices.Length];
		var triangles = new int[rayCount * 3];

		vertices[0] = origin;

		var vertexIndex = 1;
		var triangleIndex = 0;
		for (int i = 0; i <= rayCount; i++) {
			Vector3 vertex;
			var hit = Physics2D.Raycast(origin, GetVectorFromAngle(angle), viewDistance);
			if (hit.collider == null) {
				vertex = origin + GetVectorFromAngle(angle) * viewDistance;
			} else {
				vertex = new Vector3(hit.point.x, hit.point.y);
			}
			vertices[vertexIndex] = vertex;

			if (i > 0) {
				triangles[triangleIndex + 0] = 0;
				triangles[triangleIndex + 1] = vertexIndex - 1;
				triangles[triangleIndex + 2] = vertexIndex;
				triangleIndex += 3;
			}

			vertexIndex += 1;
			angle -= angleIncrease;
		}

		mesh.vertices = vertices;
		mesh.uv = uv;
		mesh.triangles = triangles;
    }

	static Vector3 GetVectorFromAngle(float angle) {
		var angleRad = angle * (Mathf.PI/180f);
		return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
	}

	static float GetAngleFromVector(Vector3 dir) {
		dir = dir.normalized;
		var n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
		if (n < 0) n += 360f;
		return n;
	}
}
