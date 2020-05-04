using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PutObjects : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private BuildController buildController;

    private GameObject buildingsManager;

    public Color transparentCanPut;
    public Color transparentCantPut;

    [SerializeField] private LayerMask layerMask;

    private GameObject previewBuilding = null;
    private List<GameObject> previewRoads = new List<GameObject>();
    private bool mouseOnUI = false;

    private Vector3 endRoadBuildPoint = new Vector3();
    private Vector3 firstRoadBuildPoint = new Vector3();

    List<Vector3> firstPath = new List<Vector3>();
    List<Vector3> secondPath = new List<Vector3>();
    //Preview
    List<Vector3> firstPathPreview = new List<Vector3>();
    List<Vector3> secondPathPreview = new List<Vector3>();

    void Start()
    {
        buildingsManager = GameObject.FindGameObjectWithTag("BuildingsManager");
    }

    // Update is called once per frame
    private void Update()
    {
        PutBuilding();
    }
    private void PutBuilding()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, layerMask))
        {
            if (buildController.GetBuildMode())
            {
                switch(buildController.GetSelectBuilding().GetComponentInChildren<PlacementObject>().GetObjectType())
                {
                    case PlacementObjectType.Default:
                        PutDefaultObject(hit);
                        break;
                    case PlacementObjectType.Road:
                        PutRoad(hit);
                        break;
                    case PlacementObjectType.Tree:
                        Debug.Log("We're working on it");
                        break;
                }
            }
            else
            {
                if(previewBuilding != null)
                {
                    Destroy(previewBuilding);
                }
            }
        }
    }
    public void PutDefaultObject(RaycastHit hit)
    {
        Vector3 placeVector = new Vector3(Mathf.RoundToInt(hit.point.x), hit.point.y, Mathf.RoundToInt(hit.point.z));
        //Jesli budynek podglądowy istnieje
        if (previewBuilding != null)
        {//to sprawdz czy ma takie samo id jak budynek wybrany | jesli nie utworz nowy 
            if (previewBuilding.GetComponentInChildren<PlacementObject>().GetObjectID() != buildController.GetSelectBuilding().GetComponentInChildren<PlacementObject>().GetObjectID())
            {
                Destroy(previewBuilding);
                previewBuilding = Instantiate(buildController.GetSelectBuilding(), placeVector, Quaternion.identity);
                previewBuilding.tag = "PreviewObject";

                GameObject[] childs = new GameObject[previewBuilding.transform.childCount];

                for (int i = 0; i < childs.Length; i++)
                {
                    childs[i] = previewBuilding.transform.GetChild(i).gameObject;
                    childs[i].tag = "PreviewObject";
                }

                Rigidbody rb = previewBuilding.AddComponent<Rigidbody>();
                rb.isKinematic = false;
                rb.useGravity = false;
                rb.constraints = RigidbodyConstraints.FreezeAll;

            }//jesli tak zmien jego kolor i pozycje
            else
            {
                //SetTransparentColor(previewBuilding, previewBuilding.GetComponentInChildren<PlacementObject>().GetCanPut());
                previewBuilding.transform.position = placeVector;
            }
        }//jesli nie istnieje to utworz go
        else
        {
            previewBuilding = Instantiate(buildController.GetSelectBuilding(), placeVector, Quaternion.identity);
            previewBuilding.tag = "PreviewObject";

            GameObject[] childs = new GameObject[previewBuilding.transform.childCount];

            for(int i =0; i < childs.Length; i++)
            {
                childs[i] = previewBuilding.transform.GetChild(i).gameObject;
                childs[i].tag = "PreviewObject";
            }


            Rigidbody rb = previewBuilding.AddComponent<Rigidbody>();
            rb.isKinematic = false;
            rb.useGravity = false;
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && mouseOnUI == false && previewBuilding.GetComponentInChildren<PlacementObject>().GetCanPut())
        {
            Destroy(previewBuilding);
            GameObject gm = Instantiate(buildController.GetSelectBuilding(), placeVector, Quaternion.identity);
            gm.transform.parent = buildingsManager.transform;
        }
    }
    public void PutRoad(RaycastHit hit)
    {
        Vector3 joinPathPossition = new Vector3();


        if(Input.GetKeyDown(KeyCode.Mouse0) && mouseOnUI == false)
        {
            firstRoadBuildPoint.x = RoundNumb.RoundToMiddle(hit.point.x);
            firstRoadBuildPoint.y = hit.point.y;
            firstRoadBuildPoint.z = RoundNumb.RoundToMiddle(hit.point.z);
        }
        if(Input.GetKey(KeyCode.Mouse0) && mouseOnUI == false)
        {
            endRoadBuildPoint.x = RoundNumb.RoundToMiddle(hit.point.x);
            endRoadBuildPoint.y = hit.point.y;
            endRoadBuildPoint.z = RoundNumb.RoundToMiddle(hit.point.z);

            firstPath.Clear();
            firstPath.Capacity = 0;
            secondPath.Clear();
            secondPath.Capacity = 0;

            if (firstRoadBuildPoint != endRoadBuildPoint)
            {
                joinPathPossition = new Vector3(endRoadBuildPoint.x, 0, firstRoadBuildPoint.z);

                //Sprawdza czy oba punkty sa w tej samej lini
                if (firstRoadBuildPoint.x == endRoadBuildPoint.x || firstRoadBuildPoint.z == endRoadBuildPoint.z)
                {
                    //Sprawdza na której osi leżą
                    if (firstRoadBuildPoint.x == endRoadBuildPoint.x)
                    {
                        //sprawdza który punkt ma większą wartość
                        if (firstRoadBuildPoint.z > endRoadBuildPoint.z)
                        {
                            for (float i = firstRoadBuildPoint.z; i >= endRoadBuildPoint.z; i--)
                            {
                                firstPath.Add(new Vector3(firstRoadBuildPoint.x, firstRoadBuildPoint.y, i));
                            }
                        }
                        else
                        {//jeśi koncowy punkt miał wieksza wartosc
                            for (float i = firstRoadBuildPoint.z; i <= endRoadBuildPoint.z; i++)
                            {
                                firstPath.Add(new Vector3(firstRoadBuildPoint.x, firstRoadBuildPoint.y, i));
                            }
                        }
                    }
                    else
                    {
                        //sprawdza który punkt ma większą wartość
                        if (firstRoadBuildPoint.x > endRoadBuildPoint.x)
                        {
                            for (float i = firstRoadBuildPoint.x; i >= endRoadBuildPoint.x; i--)
                            {
                                firstPath.Add(new Vector3(i, firstRoadBuildPoint.y, firstRoadBuildPoint.z));
                            }
                        }
                        else
                        {//jeśi koncowy punkt miał wieksza wartosc
                            for (float i = firstRoadBuildPoint.x; i <= endRoadBuildPoint.x; i++)
                            {
                                firstPath.Add(new Vector3(i, firstRoadBuildPoint.y, firstRoadBuildPoint.z));
                            }
                        }
                    }
                }//Jeśli punkty nie leza na jednej lini to wtedy robimy dwie ściezki
                else
                {
                    //joinPathPossition = new Vector3(firstRoadBuildPoint.x, firstRoadBuildPoint.y, endRoadBuildPoint.z);

                    //Tu obliczam ktora roznica punktow jest wieksza firstx endx czy firstz endz

                    bool xIsLonger;

                    float diffrenceX;
                    float diffrenceY;

                    //Roznica osi x
                    if(firstRoadBuildPoint.x - endRoadBuildPoint.x > 0)
                    {
                        diffrenceX = firstRoadBuildPoint.x - endRoadBuildPoint.x;
                    }
                    else
                    {
                        diffrenceX = firstRoadBuildPoint.x - endRoadBuildPoint.x;
                        diffrenceX = diffrenceX * -1;
                    }
                    //Roznica osi y
                    if(firstRoadBuildPoint.z - endRoadBuildPoint.z > 0)
                    {
                        diffrenceY = firstRoadBuildPoint.z - endRoadBuildPoint.z;
                    }
                    else
                    {
                        diffrenceY = firstRoadBuildPoint.z - endRoadBuildPoint.z;
                        diffrenceY = diffrenceY * -1;
                    }

                    if(diffrenceX >= diffrenceY)
                    {
                        xIsLonger = true;
                    }
                    else
                    {
                        xIsLonger = false;
                    }

                    if(xIsLonger)
                    {
                        joinPathPossition = new Vector3(endRoadBuildPoint.x, firstRoadBuildPoint.y, firstRoadBuildPoint.z);

                        //Tworzenie pierwszej ściezki
                        if (firstRoadBuildPoint.x > joinPathPossition.x)
                        {
                            for (float i = firstRoadBuildPoint.x; i >= joinPathPossition.x; i--)
                            {
                                firstPath.Add(new Vector3(i, firstRoadBuildPoint.y, firstRoadBuildPoint.z));
                            }
                        }
                        else
                        {
                            for (float i = firstRoadBuildPoint.x; i <= joinPathPossition.x; i++)
                            {
                                firstPath.Add(new Vector3(i, firstRoadBuildPoint.y, firstRoadBuildPoint.z));
                            }
                        }
                        //Tworzenie drugiej ścieżki
                        if (joinPathPossition.z > endRoadBuildPoint.z)
                        {
                            for (float i = joinPathPossition.z; i >= endRoadBuildPoint.z; i--)
                            {
                                secondPath.Add(new Vector3(joinPathPossition.x, firstRoadBuildPoint.y, i));
                            }
                        }
                        else
                        {
                            for (float i = joinPathPossition.z; i <= endRoadBuildPoint.z; i++)
                            {
                                secondPath.Add(new Vector3(joinPathPossition.x, firstRoadBuildPoint.y, i));
                            }
                        }
                    }
                    else
                    {
                        joinPathPossition = new Vector3(firstRoadBuildPoint.x, firstRoadBuildPoint.y, endRoadBuildPoint.z);

                        //Tworzenie pierwszej ściezki
                        if (firstRoadBuildPoint.z > joinPathPossition.z)
                        {
                            for (float i = firstRoadBuildPoint.z; i >= joinPathPossition.z; i--)
                            {
                                firstPath.Add(new Vector3(firstRoadBuildPoint.x, firstRoadBuildPoint.y, i));
                            }
                        }
                        else
                        {
                            for (float i = firstRoadBuildPoint.z; i <= joinPathPossition.z; i++)
                            {
                                firstPath.Add(new Vector3(firstRoadBuildPoint.x, firstRoadBuildPoint.y, i));
                            }
                        }
                        //Tworzenie drugiej ścieżki
                        if (joinPathPossition.x > endRoadBuildPoint.x)
                        {
                            for (float i = joinPathPossition.x; i >= endRoadBuildPoint.x; i--)
                            {
                                secondPath.Add(new Vector3(i, firstRoadBuildPoint.y, joinPathPossition.z));
                            }
                        }
                        else
                        {
                            for (float i = joinPathPossition.x; i <= endRoadBuildPoint.x; i++)
                            {
                                secondPath.Add(new Vector3(i, firstRoadBuildPoint.y, joinPathPossition.z));
                            }
                        }
                    }
                }
            }
            else
            {
                firstPath.Add(firstRoadBuildPoint);
            }

            //Tutaj implementacja rysowania podglądu drogi

            //Rysuje droge podgladowa
            if (firstPath.Count != 0 && !CompareVector3List(firstPath, firstPathPreview) || !CompareVector3List(secondPath, secondPathPreview) && secondPath.Count != 0 )
            {
                for (int i = 0; i < previewRoads.Count; i++)
                {
                    Destroy(previewRoads[i]);
                }
                previewRoads.Clear();
                previewRoads.Capacity = 0;

                if (secondPath.Count != 0)
                {
                    //Rysuje obie ściezki
                    for (int i = 0; i < firstPath.Count; i++)
                    {
                        GameObject gm = Instantiate(buildController.GetSelectBuilding(), firstPath[i], Quaternion.identity);
                        gm.tag = "PreviewObject";

                        int cout = gm.transform.childCount;
                        GameObject[] gmChilds = new GameObject[cout];
                        for(int j = 0; j < gmChilds.Length; j++)
                        {
                            gmChilds[j] = gm.transform.GetChild(j).gameObject;
                            gmChilds[j].tag = "PreviewObject";
                        }

                        Rigidbody rb = gm.AddComponent<Rigidbody>();
                        rb.isKinematic = false;
                        rb.useGravity = false;
                        rb.constraints = RigidbodyConstraints.FreezeAll;

                        previewRoads.Add(gm);

                        //SetTransparentColor(previewRoads[i], previewRoads[i].GetComponentInChildren<PlacementObject>().GetCanPut());
                    }
                    for (int i = firstPath.Count; i < secondPath.Count + firstPath.Count; i++)
                    {
                        GameObject gm = Instantiate(buildController.GetSelectBuilding(), secondPath[i-firstPath.Count], Quaternion.identity);
                        gm.tag = "PreviewObject";

                        int cout = gm.transform.childCount;
                        GameObject[] gmChilds = new GameObject[cout];
                        for(int j = 0; j < gmChilds.Length; j++)
                        {
                            gmChilds[j] = gm.transform.GetChild(j).gameObject;
                            gmChilds[j].tag = "PreviewObject";
                        }

                        Rigidbody rb = gm.AddComponent<Rigidbody>();
                        rb.isKinematic = false;
                        rb.useGravity = false;
                        rb.constraints = RigidbodyConstraints.FreezeAll;

                        previewRoads.Add(gm);

                        //SetTransparentColor(previewRoads[i], previewRoads[i].GetComponentInChildren<PlacementObject>().GetCanPut());
                    }
                }
                else
                {
                    for (int i = 0; i < firstPath.Count; i++)
                    {
                        GameObject gm = Instantiate(buildController.GetSelectBuilding(), firstPath[i], Quaternion.identity);
                        gm.tag = "PreviewObject";

                        int cout = gm.transform.childCount;
                        GameObject[] gmChilds = new GameObject[cout];
                        for(int j = 0; j < gmChilds.Length; j++)
                        {
                            gmChilds[j] = gm.transform.GetChild(j).gameObject;
                            gmChilds[j].tag = "PreviewObject";
                        }

                        Rigidbody rb = gm.AddComponent<Rigidbody>();
                        rb.isKinematic = false;
                        rb.useGravity = false;
                        rb.constraints = RigidbodyConstraints.FreezeAll;

                        previewRoads.Add(gm);

                        //SetTransparentColor(previewRoads[i], previewRoads[i].GetComponentInChildren<PlacementObject>().GetCanPut());

                    }
                }
            }

            //Przypisuje sciezki do sciezek podgladowych

            //Ale pierw oczyszcza sciezki podgladowe
            firstPathPreview.Clear();
            firstPathPreview.Capacity = 0;
            secondPathPreview.Clear();
            secondPathPreview.Capacity = 0;

            for (int i = 0; i < firstPath.Count; i++)
            {
                float x = firstPath[i].x;
                float y = firstPath[i].y;
                float z = firstPath[i].z;
                firstPathPreview.Add(new Vector3(x, y, z));
            }
            for (int i = 0; i < secondPath.Count; i++)
            {
                float x = secondPath[i].x;
                float y = secondPath[i].y;
                float z = secondPath[i].z;
                secondPathPreview.Add(new Vector3(x, y, z));
            }

        }
        //jesli wcisne mysz i drogi podglądowe mozna postawic wtedy je stawia
        if (Input.GetKeyUp(KeyCode.Mouse0) && mouseOnUI == false && AllPreviewRoadsCanPut(previewRoads))
        {
            //Usuwa podglądowe ścieżki jest to zabieg konieczny
            if (previewRoads.Count != 0)
            {
                for (int i = 0; i < previewRoads.Count; i++)
                {
                    Destroy(previewRoads[i]);
                }
                previewRoads.Clear();
                previewRoads.Capacity = 0;
            }
            
            //Jeśli druga sciezna istnieje
            if (secondPath.Count != 0)
            {
                //Rysuje obie ściezki
                for (int i = 0; i < firstPath.Count; i++)
                {
                    GameObject gm = Instantiate(buildController.GetSelectBuilding(), firstPath[i], Quaternion.identity);
                    gm.transform.parent = buildingsManager.transform;
                }
                for (int i = 0; i < secondPath.Count; i++)
                {
                    GameObject gm = Instantiate(buildController.GetSelectBuilding(), secondPath[i], Quaternion.identity);
                    gm.transform.parent = buildingsManager.transform;
                }
            }
            else
            {
                for (int i = 0; i < firstPath.Count; i++)
                {
                    GameObject gm = Instantiate(buildController.GetSelectBuilding(), firstPath[i], Quaternion.identity);
                    gm.transform.parent = buildingsManager.transform;
                }
            }
        }//Jesli nie mozna postawic drogi to usuwam drogi podgladowe
        else if(Input.GetKeyUp(KeyCode.Mouse0) && mouseOnUI == false && !AllPreviewRoadsCanPut(previewRoads))
        {
            //Usuwa podglądowe ścieżki jest to zabieg konieczny
            if (previewRoads.Count != 0)
            {
                for (int i = 0; i < previewRoads.Count; i++)
                {
                    Destroy(previewRoads[i]);
                }
                previewRoads.Clear();
                previewRoads.Capacity = 0;
            }
        }
    }
    public void SetTransparentColor(GameObject gm, bool canPut)
    {
        Renderer renderer = gm.GetComponentInChildren<MeshRenderer>();
        Material[] materials = renderer.materials;

        if(canPut)
        {
            for(int i =0; i<materials.Length; i++)
            {
                materials[i].SetColor("_Color", transparentCanPut);
            }
        }
        else
        {
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].SetColor("_Color", transparentCantPut);
            }
        }
        renderer.materials = materials;
        gm.GetComponentInChildren<MeshRenderer>().materials = materials;
    }
    public void SetMouseOnUI(bool b)
    {
        this.mouseOnUI = b;
        Debug.Log("Mouse on ui " + b);
    }
    private bool CompareVector3List(List<Vector3> first, List<Vector3> second)
    {
        if(first.Count != second.Count)
        {
            return false;
        }
        else
        {
            for(int i = 0; i < first.Count; i++)
            {
                if(first[i].x == second[i].x && first[i].y == second[i].y && first[i].z == second[i].z)
                {
                    return true;
                }
            }
        }
        return false;
    }
    private bool AllPreviewRoadsCanPut(List<GameObject> pR)
    {
        for(int i=0; i < pR.Count; i++)
        {
            if(!pR[i].GetComponentInChildren<PlacementObject>().GetCanPut())
            {
                return false;
            }
        }
        return true;
    }
}
