using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildController : MonoBehaviour
{
    [SerializeField] private List<GameObject> buildingsList;

    private bool buildMode = false;
    private GameObject selectBuilding;

    public void SetBuilding(int buildingID)
    {
        selectBuilding = buildingsList[buildingID];
        SetBuildMode(true);
    }
    public void SetBuildMode(bool mode)
    {
        buildMode = mode;
    }
    public bool GetBuildMode()
    {
        return buildMode;
    }
    public GameObject GetSelectBuilding()
    {
        return selectBuilding;
    }
}
