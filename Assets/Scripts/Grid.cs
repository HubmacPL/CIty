using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public static int cellsX = 10;
    public static int cellsY = 10;
    public float cellSize = 10f;
    public GameObject testObject;

    Vector3[,] cornersTransforms = new Vector3[cellsX * 2, cellsY * 2];
    private Vector3 transformPossition;

    private void Start()
    {
        transformPossition = transform.position;
        CreateGrid();
    }
    public void CreateGrid()
    {
        CreateGridCorners();
        DrawGridLines();
    }
    public void CreateGridCorners()
    {

        for (int i = 0; i < cellsX * 2; i++)
        {
            for (int j = 0; j < cellsY * 2; j++)
            {
                cornersTransforms[i, j] = new Vector3((transformPossition.x + i) * cellSize, transformPossition.y, (transformPossition.z + j) * cellSize);
            }
        }
    }
    public void DrawGridLines()
    {
        for (int i = 0; i < cellsX * 2; i++)
        {
            for (int j = 0; j < cellsY * 2; j++)
            {
                Instantiate(testObject, cornersTransforms[i, j], Quaternion.identity, gameObject.transform);
            };
        }
    }


}
