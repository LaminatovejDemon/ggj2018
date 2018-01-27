using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundMngr : baseMngr<groundMngr> {

	public MeshFilter _groundTemplate;
	public int _groundWidth = 100;
	public float _uvScale = 0.1f;

	MeshFilter[] _instances;
	int _instanceCount = 2;

	Vector3[] _vertices = null;

	void InitialiseVertices(MeshFilter target){

	
		if (_vertices == null) {
			_vertices = new Vector3[_groundWidth * 4];
		}

		Mesh mesh = target.mesh;
		int[] indices_ = new int[_groundWidth * 6];
		Vector2[] uvs_ = new Vector2[_groundWidth * 4];
		Vector3[] template_ = { new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(1, 0, 0), new Vector3 (1, 1, 0) };

		for (int i = 0; i < _groundWidth * 4; i++ ) {
			_vertices [i] = template_ [i % 4] + Vector3.right * (int)(i/4);
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
		target.mesh = mesh;
	}

	void InitialiseInstances(){
		if (_instances != null) {
			return;
		}
		_instances = new MeshFilter[_instanceCount];

		for (int i = 0; i < _instanceCount; ++i) {
			_instances [i] = GameObject.Instantiate (_groundTemplate).GetComponent<MeshFilter>();
			_instances [i].name = "Ground_" + i;
			_instances [i].transform.parent = transform;

			InitialiseVertices (_instances [i]);
			_instances [i].transform.position = Camera.main.WorldToViewportPoint (Vector3.down) + Vector3.left * i * _groundWidth;
		}
	}
		
	public void UpdateVertices(){
		InitialiseInstances ();
	}

}