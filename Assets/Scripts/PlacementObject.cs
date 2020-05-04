using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlacementObjectType
{
    Default,
    Road,
    Tree
}

public class PlacementObject : MonoBehaviour
{
    [SerializeField] private int objectID;

    [SerializeField] private PlacementObjectType placementObjectType;

    private PutObjects putObjects;
    private bool isSetUp = false;
    private bool canPut = true;
    private bool isInCollision = false;

    private void Start()
    {
        putObjects = GameObject.FindGameObjectWithTag("GameController").GetComponent<PutObjects>();
        WhenIsPreview();
    }

    private void Update()
    {
        WhenIsPreview();
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag != "Terrain" && other.gameObject.tag != "PreviewObject")
        {
            isInCollision = true;
            canPut = false;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Terrain" && other.gameObject.tag != "PreviewObject")
        {
            isInCollision = false;
            canPut = true;
        }
    }
    private void WhenIsPreview()
    {
        if (gameObject.tag == "PreviewObject")
        {
            SetTransparentColor();
        }
    }
    public void SetTransparentColor()
    {
        Material[] materials = gameObject.GetComponent<MeshRenderer>().materials;
        if(canPut && gameObject.tag == "PreviewObject" )
        {
            for(int i =0; i < materials.Length; i++)
            {
                materials[i].SetColor("_Color", putObjects.transparentCanPut);
            }
        }
        else if(canPut == false && gameObject.tag == "PreviewObject")
        {
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].SetColor("_Color", putObjects.transparentCantPut);
            }
        }
    }
    public bool GetCanPut()
    {
        return canPut;
    }
    public int GetObjectID()
    {
        return objectID;
    }
    public PlacementObjectType GetObjectType()
    {
        return placementObjectType;
    }
}
