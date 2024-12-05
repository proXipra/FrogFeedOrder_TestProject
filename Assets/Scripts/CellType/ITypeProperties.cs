using UnityEngine;

public interface IHasDirection
{
    void AssignDirection(CellScript.CellDirection direction);
}

public interface IHasColor
{
    void AssignColor(CellScript.CellColor color);
}
