using System;
using UnityEngine;

public class FrogScript : MonoBehaviour, IHasColor, IHasDirection
{
    //public enum FrogDirection {Up, Down, Left, Right}
    //public enum FrogColor {Blue, Green, Purple, Red, Yellow}
    
    //private readonly FrogDirection _direction;
    [SerializeField] private Material blueFrogMaterial;
    [SerializeField] private Material greenFrogMaterial;
    [SerializeField] private Material purpleFrogMaterial;
    [SerializeField] private Material redFrogMaterial;
    [SerializeField] private Material yellowFrogMaterial;




    public void AssignColor(CellScript.CellColor color)
    {
        Material materialToAssign = null;
        Debug.Log("Here!");
        switch (color)
        {
            case CellScript.CellColor.Blue:
                materialToAssign = blueFrogMaterial;
                break;
            case CellScript.CellColor.Green:
                materialToAssign = greenFrogMaterial;
                break;
            case CellScript.CellColor.Purple:
                materialToAssign = purpleFrogMaterial;
                break;
            case CellScript.CellColor.Red:
                materialToAssign = redFrogMaterial;
                break;
            case CellScript.CellColor.Yellow:
                materialToAssign = yellowFrogMaterial;
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
        Transform meshTransform = transform.Find("Mesh");
        SkinnedMeshRenderer meshRenderer = meshTransform.GetComponent<SkinnedMeshRenderer>();
        if (meshRenderer != null)
        {
            //Kopyasını almadan değişiklik yapılamadığı 
            //için materiallerin kopyasını alıp daha sonra atama yaptım
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
                transform.rotation = Quaternion.Euler(0,-180,0);
                break;
            case CellScript.CellDirection.Down:
                transform.rotation = Quaternion.Euler(0,0,0);
                break;
            case CellScript.CellDirection.Left:
                transform.rotation = Quaternion.Euler(0,90,0);
                break;
            case CellScript.CellDirection.Right:
                transform.rotation = Quaternion.Euler(0,-90,0);
                break;
            default:
                Debug.Log("Something went wrong!, AssignDirectin(Frog)");
                break;
        }
    }

}

