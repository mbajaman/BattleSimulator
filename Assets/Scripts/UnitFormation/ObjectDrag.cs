using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDrag : MonoBehaviour
{
    private Vector3 offset;

    private void OnMouseDown()
    {
        offset = transform.position - FormationSystem.GetMouseWorldPosition();
    }

    private void OnMouseDrag()
    {
        Vector3 pos = FormationSystem.GetMouseWorldPosition() + offset;
        transform.position = FormationSystem.current.SnapCoordinateToGrid(pos);
    }
}
