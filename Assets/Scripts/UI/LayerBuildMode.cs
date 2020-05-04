using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerBuildMode : UILayer
{
    private BuildController buildController;
    private PutObjects putObjects;

    private void Start()
    {
        buildController = GameObject.FindGameObjectWithTag("BuildController").GetComponent<BuildController>();
        putObjects = GameObject.FindGameObjectWithTag("GameController").GetComponent<PutObjects>();
    }
    private void Update()
    {
        if(buildController.GetBuildMode())
        {
            SetChildsActive(true);
        }
        else
        {
            SetChildsActive(false);
        }
    }
}
