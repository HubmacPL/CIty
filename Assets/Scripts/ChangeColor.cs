using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    public Material[] materials;
    Color colorStandard;
    public Color colorRed;
    // Start is called before the first frame update
    void Start()
    {
        materials = gameObject.GetComponent<MeshRenderer>().materials;
        colorStandard = materials[0].GetColor("_Color");
    }

    // Update is called once per frame
    void Update()
    {
        materials[0].SetColor("_Color", colorRed);
    }
}
