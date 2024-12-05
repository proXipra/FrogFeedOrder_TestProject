using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tongue : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private float _counter;
    public Transform origin;
    public float lineDrawSpeed = 6f;

    CellScript cell;
    private List<Vector3> berryPathWorld = new List<Vector3>();
    private List<GameObject> collectedBerries = new List<GameObject>();

    public void Start()
    {
        cell = GetComponentInParent<CellScript>();
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 0;
    }

    public void InitiateTongue()
    {
        SearchCells();
        StartCoroutine(DrawTongue());
    }

    public void SearchCells()
    {
        string frogDirection = cell.GetDirectionAsString();
        NodeScript currentNode = cell.GetParentNode();
        int startX = currentNode.location.x;
        int startY = currentNode.location.y;
        CellScript.CellColor frogColor = cell.GetColor();
        List<(int, int)> berryPath = new List<(int, int)>();

        switch (frogDirection)
        {
            case "Up":
                FindBerriesInDirection(0, 1, startX, startY, frogColor, berryPath);
                break;
            case "Down":
                FindBerriesInDirection(0, -1, startX, startY, frogColor, berryPath);
                break;
            case "Left":
                FindBerriesInDirection(-1, 0, startX, startY, frogColor, berryPath);
                break;
            case "Right":
                FindBerriesInDirection(1, 0, startX, startY, frogColor, berryPath);
                break;
        }

        // Convert grid positions to world positions and track berries
        foreach (var berryPos in berryPath)
        {
            Vector3 pos = NodeManager.GetNodeWorldPosition(berryPos.Item1, berryPos.Item2);
            Vector3 worldPosition = new Vector3(pos.x, origin.position.y, pos.z);
            berryPathWorld.Add(worldPosition);

            NodeScript node = NodeManager.GetNodeAt(berryPos.Item1, berryPos.Item2);
            if (node != null)
            {
                CellScript berryCell = node.GetActiveCell();
                if (berryCell != null && berryCell.GetCellTypeAsString() == "Berry")
                {
                    collectedBerries.Add(berryCell.gameObject);
                }
            }
        }
    }

    private IEnumerator DrawTongue()
    {
        _lineRenderer.positionCount = 1;
        _lineRenderer.SetPosition(0, origin.position);

        // Forward drawing
        for (int i = 0; i < berryPathWorld.Count; i++)
        {
            Vector3 startPosition = _lineRenderer.GetPosition(_lineRenderer.positionCount - 1);
            Vector3 endPosition = berryPathWorld[i];

            float distance = Vector3.Distance(startPosition, endPosition);
            float t = 0;

            while (t < 1)
            {
                t += Time.deltaTime * lineDrawSpeed / distance;
                Vector3 currentPosition = Vector3.Lerp(startPosition, endPosition, t);
                _lineRenderer.positionCount++;
                _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, currentPosition);
                yield return null;
            }

            // Final position adjustment
            _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, endPosition);
        }

        Debug.Log("Forward drawing complete!");

        // Pull berries to the frog
        StartCoroutine(PullBerriesAlongPartialPath());

        // Reverse line removal
        int currentPositionCount = _lineRenderer.positionCount;
        for (int i = currentPositionCount - 1; i >= 0; i--)
        {
            _lineRenderer.positionCount = i;
            yield return null;
        }

        Debug.Log("Line removed!");
    }



private IEnumerator PullBerriesAlongPartialPath()
{
    for (int berryIndex = collectedBerries.Count - 1; berryIndex >= 0; berryIndex--)
    {
        GameObject berry = collectedBerries[berryIndex];
        Vector3 startPosition = berry.transform.position;


        List<Vector3> adjustedPath = new List<Vector3> { startPosition };


        bool passedArrow = false; 

        foreach (var pathPoint in berryPathWorld)
        {
            NodeScript node = NodeManager.GetNodeAt((int)pathPoint.x, (int)pathPoint.z); 
            if (node != null)
            {
                CellScript cell = node.GetActiveCell();
                if (cell != null && cell.GetCellTypeAsString() == "Arrow")
                {
                
                    if (!passedArrow)
                    {
                        adjustedPath.Add(pathPoint); 
                    }
                    passedArrow = true; 
                }
            }
        }

    
        if (!passedArrow)
        {
            adjustedPath.Clear();
            adjustedPath.Add(startPosition); 
        }

        adjustedPath.Add(origin.position); 

        for (int i = 0; i < adjustedPath.Count; i++)
        {
            Vector3 targetPosition = adjustedPath[i];
            float t = 0;
            float distance = Vector3.Distance(startPosition, targetPosition);

            while (t < 1)
            {
                t += Time.deltaTime * lineDrawSpeed / distance;
                berry.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
                yield return null;
            }

            startPosition = targetPosition; 
        }

        // GameManager +1 skor
        Destroy(berry);
    }

    // Listeyi temizle
    collectedBerries.Clear();
    Debug.Log("Berries bulundukları noktadan froga doğru path üzerinden çekildi ve yok edildi.");
}


    private void FindBerriesInDirection(int dirX, int dirY, int startX, int startY, CellScript.CellColor frogColor, List<(int, int)> path)
    {
        int currentX = startX;
        int currentY = startY;

        int minCollectableBerry = FindObjectOfType<NodeGenerator>().GetNodeSize() - 1;

        while (true)
        {
            currentX += dirX;
            currentY += dirY;

            NodeScript currentNode = NodeManager.GetNodeAt(currentX, currentY);
            if (currentNode == null)
            {
                Debug.Log($"Reached edge, stopping search. Position: ({currentX}, {currentY})");
                break;
            }

            CellScript currentCell = currentNode.GetActiveCell();
            if (currentCell == null)
            {
                Debug.Log($"No active cell in node. Position: ({currentX}, {currentY})");
                break;
            }

            if (currentCell.GetCellTypeAsString() == "Berry")
            {
                if (currentCell.GetColor() == frogColor)
                {
                    path.Add((currentX, currentY));
                    Debug.Log($"Same color Berry found! Position: ({currentX}, {currentY})");
                }
                else
                {
                    Debug.Log($"Different color Berry found, stopping search. Position: ({currentX}, {currentY})");
                    break;
                }
            }
            else if (currentCell.GetCellTypeAsString() == "Arrow")
            {
                if (currentCell.GetColor() == frogColor)
                {
                    path.Add((currentX, currentY));
                    switch (currentCell.GetDirection())
                    {
                        case CellScript.CellDirection.Up: dirX = 0; dirY = 1; break;
                        case CellScript.CellDirection.Down: dirX = 0; dirY = -1; break;
                        case CellScript.CellDirection.Left: dirX = -1; dirY = 0; break;
                        case CellScript.CellDirection.Right: dirX = 1; dirY = 0; break;
                        default: Debug.LogWarning("Invalid direction specified."); break;
                    }
                    Debug.Log($"Arrow cell found, direction changed! New Direction: ({dirX}, {dirY})");
                    continue;
                }
                else
                {
                    Debug.Log($"Different color Arrow found, stopping search. Position: ({currentX}, {currentY})");
                    break;
                }
            }
            else
            {
                Debug.Log($"Invalid cell type, stopping search. Position: ({currentX}, {currentY})");
                break;
            }
        }

        if (path.Count >= minCollectableBerry)
        {
            Debug.Log("Minimum required berries collected, stopping search.");
            foreach (var pos in path)
            {
                Debug.Log($"Berry Position: {pos}");
            }
        }
    }
}
