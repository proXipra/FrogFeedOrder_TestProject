using UnityEditor;
using UnityEngine;

public class CellScript : MonoBehaviour
{
    //Enums
    public enum CellType {Arrow, Berry, Frog}
    public enum CellDirection {Up, Down, Left, Right}
    public enum CellColor {Blue, Green, Purple, Red, Yellow}
    

    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GameObject berryPrefab;
    [SerializeField] private GameObject frogPrefab;


    [SerializeField] private Material _blueMaterial;
    [SerializeField] private Material _greenMaterial;
    [SerializeField] private Material _purpleMaterial;
    [SerializeField] private Material _redMaterial;
    [SerializeField] private Material _yellowMaterial;


    public bool IsActiveCell = false;
    [SerializeField]public bool IsTypeAssigned = false;
    [SerializeField]private bool _isAssignedCell = false; 
    [SerializeField]private CellType _cellType;
    [SerializeField] private CellColor _color;
    [SerializeField]private CellDirection _direction;

    public void Start()
    {
        AssignCellColor();
    }

    public void Update()
    {
        AssignType();
    }

    public NodeScript GetParentNode()
    {
        return GetComponentInParent<NodeScript>();
    }

    public CellDirection GetDirection()
    {
        return _direction;
    }

    public string GetDirectionAsString()
    {
        return _direction.ToString();
    }
    public CellColor GetColor()
    {
        return _color;
    }

    public string GetCellTypeAsString()
    {
        return _cellType.ToString();
    }


    public void AssignType()
    {
        if (IsTypeAssigned == false && IsActiveCell)
        {
            switch (_cellType)
            {
                
                case CellType.Arrow:
                    SpawnTypePrefab(arrowPrefab);
                    break;
                case CellType.Berry:
                    SpawnTypePrefab(berryPrefab);
                    break;
                case CellType.Frog:
                    SpawnTypePrefab(frogPrefab);
                    break;
                default:
                    Debug.Log("Something went wrong! (AssignType() in CellScript)");
                    break;
            }
            IsTypeAssigned = true;
        }
        
    }

    private void SpawnTypePrefab(GameObject typePrefab)
    {
        if (typePrefab == null)
        {
            Debug.LogWarning("Prefab is not assigned!");
            return;
        }

        float prefabHeight = CalculatePrefabHeight(typePrefab);
        Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y + prefabHeight, transform.position.z);
        Quaternion spawnRotation = Quaternion.identity;
        GameObject typeInstance = Instantiate(typePrefab, spawnPosition, spawnRotation);

        SetParent(typeInstance, this.transform);
        AssignAttributes(typeInstance);
    }


    private void AssignAttributes(GameObject instance)
    {
        if (instance.TryGetComponent<IHasDirection>(out IHasDirection hasDirection))
        {
            hasDirection.AssignDirection(_direction);
        }

        if (instance.TryGetComponent<IHasColor>(out IHasColor hasColor))
        {
            hasColor.AssignColor(_color);
        }
    }
    
    private float CalculatePrefabHeight(GameObject prefab)
    {
        if (prefab.TryGetComponent<BoxCollider>(out BoxCollider boxCollider))
            return boxCollider.size.y;

        Debug.LogError("Height cannot be calculated. No valid component found.");
        return 0f;
    }

    private void SetParent(GameObject instance, Transform parent)
    {
        instance.transform.SetParent(parent, true);
    }


    // Bu fonksiyon Cell in kendi rengini ataması için!
    public void AssignCellColor()
    {
        Material materialToAssign = null;
        switch (_color)
        {
            case CellColor.Blue:
                materialToAssign = _blueMaterial;
                break;
            case CellColor.Green:
                materialToAssign = _greenMaterial;
                break;
            case CellColor.Purple:
                materialToAssign = _purpleMaterial;
                break;
            case CellColor.Red:
                materialToAssign = _redMaterial;
                break;
            case CellColor.Yellow:
                materialToAssign = _yellowMaterial;
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

    public bool IsAssignedCell
    {
        get => _isAssignedCell;
        set
        {
            _isAssignedCell = value;
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
#endif
        }
    }
}
