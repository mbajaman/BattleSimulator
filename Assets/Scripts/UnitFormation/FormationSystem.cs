using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FormationSystem : MonoBehaviour
{
    public static FormationSystem current;

    public GridLayout gridLayout;
    private Grid grid;

    [SerializeField]
    private Tilemap MainTilemap;
    [SerializeField] private TileBase tileBase;

    public GameObject prefab1;
    public GameObject prefab2;

    private PlaceableObject objectToPlace;

    #region Unity methods

    private void Awake()
    {
        current = this;
        grid = gridLayout.gameObject.GetComponent<Grid>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            InitializeWithObject(prefab1);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            InitializeWithObject(prefab2);
        }
    }

    #endregion

    #region Utils

    public static Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            return raycastHit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }

    public Vector3 SnapCoordinateToGrid(Vector3 position)
    {
        Vector3Int cellPos = gridLayout.WorldToCell(position);
        position = grid.GetCellCenterWorld(cellPos);
        position += new Vector3(0f, 0.5f, 0);
        return position;
    }

    #endregion

    #region Building Placement

    public void InitializeWithObject(GameObject prefab)
    {
        Vector3 position = SnapCoordinateToGrid(Vector3.zero);

        GameObject obj = Instantiate(prefab, position, Quaternion.identity);
        objectToPlace = obj.GetComponent<PlaceableObject>();
        obj.AddComponent<ObjectDrag>();
    }

    #endregion
}

