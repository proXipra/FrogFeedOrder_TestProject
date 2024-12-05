using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Update()
    {
        CheckMouseClick();
    }

    public void CheckMouseClick()
    {
        if (Input.GetMouseButtonDown(0)) // Sol tık
        {
            Debug.Log("Here2");
            Camera camera = Camera.main;
            if (camera == null)
            {
                Debug.Log("no cam");
                return;
            }

            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            // Frog katmanı hedeflendi
            int layerMask = LayerMask.GetMask("Frog");

            //Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 2.0f); // Ray'i görselleştir

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask))
            {
                Debug.Log("Hit: " + hit.collider.name);
                Tongue frogTongue = hit.collider.GetComponent<Tongue>();
                if (frogTongue != null)
                {
                    Debug.Log("Frog clicked");
                    frogTongue.InitiateTongue();
                }
            }
            else
            {
                Debug.Log("No hit");
            }
        }                       
    }


}