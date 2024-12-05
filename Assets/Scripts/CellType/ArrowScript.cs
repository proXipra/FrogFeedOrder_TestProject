using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour, IHasColor, IHasDirection
{
    [SerializeField] private Material blueArrowMaterial;
    [SerializeField] private Material greenArrowMaterial;
    [SerializeField] private Material purpleArrowMaterial;
    [SerializeField] private Material redArrowMaterial;
    [SerializeField] private Material yellowArrowMaterial;

    public void AssignColor(CellScript.CellColor color)
    {
        Material materialToAssign = null;
        Debug.Log("Here!");
        switch (color)
        {
            case CellScript.CellColor.Blue:
                materialToAssign = blueArrowMaterial;
                break;
            case CellScript.CellColor.Green:
                materialToAssign = greenArrowMaterial;
                break;
            case CellScript.CellColor.Purple:
                materialToAssign = purpleArrowMaterial;
                break;
            case CellScript.CellColor.Red:
                materialToAssign = redArrowMaterial;
                break;
            case CellScript.CellColor.Yellow:
                materialToAssign = yellowArrowMaterial;
                break;
            default:
                Debug.LogError("Invalid color!");
                return;
        }

        if (materialToAssign == null)
        {
            Debug.Log("Material cannot found!");
            return;
        }
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            //Kopyasını almadan değişiklik yapılamadığı 
            //için meshRenderer.materials'in kopyasını alıp daha sonra atama yaptım.
            Material[] materials = meshRenderer.materials;
            materials[0] = materialToAssign;
            meshRenderer.materials = materials;
        }
        else
        {
            Debug.Log("There is no Meshrenderer component!");
        }
    }

    public void AssignDirection(CellScript.CellDirection direction)
    {
        switch (direction)
        {
            case CellScript.CellDirection.Up:
                transform.rotation = Quaternion.Euler(-90,-180,90);
                break;
            case CellScript.CellDirection.Down:
                transform.rotation = Quaternion.Euler(-90,-180,270);
                break;
            case CellScript.CellDirection.Left:
                transform.rotation = Quaternion.Euler(-90,-180,0);
                break;
            case CellScript.CellDirection.Right:
                transform.rotation = Quaternion.Euler(-90,-180,180);
                break;
            default:
                Debug.Log("Something went wrong!, AssignDirectin(Frog)");
                break;
        }
    }
}
