using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILayer : MonoBehaviour
{
    [SerializeField] private List<GameObject> LayerChilds;

    public void SetChildsActive(bool active)
    {
        for(int i = 0; i < LayerChilds.Count; i++)
        {
            LayerChilds[i].SetActive(active);
        }
    }
}
