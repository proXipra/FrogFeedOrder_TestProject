using UnityEngine;

public class BerryScript : MonoBehaviour, IHasColor
{
    [SerializeField] private Material blueBerryMaterial;
    [SerializeField] private Material greenBerryMaterial;
    [SerializeField] private Material purpleBerryMaterial;
    [SerializeField] private Material redBerryMaterial;
    [SerializeField] private Material yellowBerryMaterial;


    public void AssignColor(CellScript.CellColor color)
    {
        Material materialToAssign = null;
        //Debug.Log("Here!");
        switch (color)
        {
            case CellScript.CellColor.Blue:
                materialToAssign = blueBerryMaterial;
                break;
            case CellScript.CellColor.Green:
                materialToAssign = greenBerryMaterial;
                break;
            case CellScript.CellColor.Purple:
                materialToAssign = purpleBerryMaterial;
                break;
            case CellScript.CellColor.Red:
                materialToAssign = redBerryMaterial;
                break;
            case CellScript.CellColor.Yellow:
                materialToAssign = yellowBerryMaterial;
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
}
