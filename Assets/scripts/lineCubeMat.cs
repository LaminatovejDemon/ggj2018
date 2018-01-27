using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lineCubeMat : MonoBehaviour {

    Material mat;
    float posY;
    float newPos;
    int decreament = 255;
    int trans = 0;
    Color32 col;

    void Start () {
        mat = GetComponent<Renderer>().material;
        col = mat.GetColor("_TintColor");
        col.a = (byte)trans;
        mat.SetColor("_TintColor", col);
        posY = gameObject.transform.localPosition.y;
        newPos = posY;
       
    }
	
	
	void Update () {
        newPos = this.gameObject.transform.localPosition.y;
        if (newPos != posY)
        {
            trans = 255;
            posY = newPos;
        }
        else trans-=5;
        if (trans < 5) trans = 0;
        col.a = (byte)trans;
        mat.SetColor("_TintColor", col);
    }
}
