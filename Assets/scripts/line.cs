using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class line : MonoBehaviour {

	public float _tileScale = 0.1f;
	public float _uvScale = 0.1f;
	public float _length = 10;


	Vector3[] _vertices = null;
	int _groundWidth = 0;

	public void InitialiseVertices(){
		InitialiseVertices (_length);
	}

	public void InitialiseVertices(float length){

		if (_vertices != null || length == 0) {
			return;
		}

		_groundWidth = (int)(length / _tileScale);

		_vertices = new Vector3[_groundWidth * 4];

		Mesh mesh = GetComponent<MeshFilter>().mesh;
		int[] indices_ = new int[_groundWidth * 6];
		Vector2[] uvs_ = new Vector2[_groundWidth * 4];
		Vector3[] template_ = { new Vector3(0, 0, 0), new Vector3(0, _tileScale, 0), new Vector3(_tileScale, 0, 0), new Vector3 (_tileScale, _tileScale, 0) };

		for (int i = 0; i < _groundWidth * 4; i++ ) {
			_vertices [i] = template_ [i % 4] + Vector3.right * (int)(i/4) * _tileScale;
			uvs_ [i] = new Vector2(_vertices [i].x * _uvScale, _vertices[i].y * _uvScale);
		}

		int index_ = 0;
		for (int i = 0; i < _groundWidth * 4; i+=4 ) {
			indices_ [index_++] = i;
			indices_ [index_++] = i+1;
			indices_ [index_++] = i+2;
			indices_ [index_++] = i+2;
			indices_ [index_++] = i+1;
			indices_ [index_++] = i+3;
		}

		mesh.vertices = _vertices;
		mesh.triangles = indices_;
		mesh.uv = uvs_;
		mesh.RecalculateBounds();
		GetComponent<MeshFilter>().mesh = mesh;
	}


	public float GetLength(){
		return _groundWidth * _tileScale;
	}

	public Vector3[] PullVertices(){
		return _vertices;
	}

	public void PushVertices(){
		GetComponent<MeshFilter> ().mesh.vertices = _vertices;
		GetComponent<MeshFilter>().mesh.RecalculateBounds();
	}
}
