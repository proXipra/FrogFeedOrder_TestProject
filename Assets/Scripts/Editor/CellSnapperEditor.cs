using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(CellScript))]
public class CellSnapperEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI(); 

        if (GUILayout.Button("Snap New Cells to Nodes"))
        {
            CheckForNewCells();
        }
    }

    private static void CheckForNewCells()
    {
        foreach (var cell in CheckUnAssignedCells())
        {
            SnapToNearestNode(cell);
        }
    }

    private static List<CellScript> CheckUnAssignedCells()
    {
        CellScript[] allCells = FindObjectsOfType<CellScript>();

        List<CellScript> unassignedCells = new List<CellScript>();
        foreach (var cell in allCells)
        {
            if (cell.IsAssignedCell == false)
            {
                unassignedCells.Add(cell);
            }
        }
        Debug.Log("Assign Listesi: " + unassignedCells);
        return unassignedCells;
    }

    private static void SnapToNearestNode(CellScript cell)
    {
        if (cell.IsAssignedCell)
        {
            Debug.Log($"Cell {cell.name} zaten atanmış.");
            return;
        }

        NodeScript[] nodes = Object.FindObjectsOfType<NodeScript>();
        if (nodes.Length == 0)
        {
            //Debug.LogWarning("Sahnede herhangi bir Node bulunamadı.");
            return;
        }

        NodeScript closestNode = null;
        float closestDistance = Mathf.Infinity;

        foreach (var node in nodes)
        {
            float distance = Vector3.Distance(cell.transform.position, node.transform.position);
            if (distance < closestDistance && node.cells.Count < node.maxCells)
            {
                closestNode = node;
                closestDistance = distance;
            }
        }

        if (closestNode != null)
        {
            Vector3 locationToSnap = new Vector3(
                closestNode.transform.position.x,
                (closestNode.cells.Count * (cell.GetComponent<BoxCollider>().size.y * 1.5f)) +
                (closestNode.GetComponent<BoxCollider>().bounds.min.y + cell.GetComponent<BoxCollider>().size.y),
                closestNode.transform.position.z
            );

            cell.transform.position = locationToSnap;
            cell.transform.SetParent(closestNode.transform);
            cell.IsAssignedCell = true;
            closestNode.AddCell(cell);

            Debug.Log($"Cell {cell.name}, {closestNode.name} node'una snaplendi.");
        }
        else
        {
            Debug.LogWarning("En yakın uygun Node bulunamadı!");
        }
    }

}
