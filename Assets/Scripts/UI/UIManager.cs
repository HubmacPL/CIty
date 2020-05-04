using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> uiLayers;
    [SerializeField] private BuildController buildController;

    public void SetLayerEnable(int id, bool mode)
    {
        uiLayers[id].SetActive(mode);
    }
    public bool CheckBuildMode(BuildController bc)
    {
       return bc.GetBuildMode();
    }
    public void Update()
    {
    }
}
