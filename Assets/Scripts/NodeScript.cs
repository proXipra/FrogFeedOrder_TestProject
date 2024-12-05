using System.Collections.Generic;
using UnityEngine;


public class NodeScript : MonoBehaviour
{
    public Location location;
    public int maxCells = 4;
    public List<CellScript> cells;
    public CellScript activeCell;
    
    public void Update()
    {
        UpdateActiveCell();
    }

    public CellScript GetActiveCell()
    {
        return activeCell;
    }


    public bool RemoveCell(CellScript cell)
    {
        if (cells.Remove(cell))
        {
            cell.IsActiveCell = false; // Durumu sıfırla
            return true;
        }
        Debug.LogWarning("Cell could not be removed as it was not found.");
        return false;
    }


    // public void UpdateActiveCell()
    // {
    //     if (cells.Count == 0)
    //     {
    //         Debug.Log("No cells to update.");
    //         return;
    //     }

    //     float closestDistance = Mathf.Infinity;
    //     CellScript activeCell = null;

    //     foreach (var cell in cells)
    //     {
    //         float distance = Mathf.Abs(cell.transform.position.y - GetComponent<BoxCollider>().bounds.max.y);
    //         if (distance < closestDistance)
    //         {
    //             closestDistance = distance;
    //             activeCell = cell;
    //         }
    //     }

    //     foreach (var cell in cells)
    //     {
    //         cell.IsActiveCell = cell == activeCell;
    //     }
    // }

    public void UpdateActiveCell()
    {
         if (cells.Count == 0)
        {
            Debug.Log("No cells to update.");
            return;
        }
        Transform[] children = gameObject.GetComponentsInChildren<Transform>();
        Transform upperCell = null;
        float closestDistance = Mathf.Infinity;

        foreach (Transform child in children)
        {
            if (child != transform)
            {
                float cellDistance = Mathf.Abs(child.transform.position.y - GetComponent<BoxCollider>().bounds.max.y);
                if (closestDistance > cellDistance)
                {
                    closestDistance = cellDistance;
                    upperCell = child;
                }
            }
        }

        if (upperCell != null)
        {
            // Script'i al ve IsActiveCell'i ayarla
            var cellScript = upperCell.GetComponent<CellScript>();
            if (cellScript != null)
            {
                activeCell = cellScript;
                cellScript.IsActiveCell = true;
            }
        }
    }



    //Editorde bir cell i noda snaplemekte kullanılıyor!!
    public bool AddCell(CellScript cell)
    {
        if (cells.Count >= maxCells)
        {
            Debug.Log("Max cell limit reached.");
            return false;
        }
        else
        {
            cells.Add(cell);
            return true;
        }
    }


}

[System.Serializable]
public class Location 
{
    public int x;
    public int y;
}


