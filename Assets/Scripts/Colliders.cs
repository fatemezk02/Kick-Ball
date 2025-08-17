using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colliders : MonoBehaviour
{
    public float colliderThickness = 0.1f; 
    public float zPosition = 0f;
    [SerializeField] private PhysicMaterial Ground_mat;
    private Vector2 screenSize;
    // Start is called before the first frame update
    void Start()
    {

        Dictionary<string, Transform> colliders = new Dictionary<string, Transform>();

        colliders.Add("Bottom", new GameObject().transform);
        colliders.Add("Right", new GameObject().transform);
        colliders.Add("Left", new GameObject().transform);

   
        Vector3 cameraPos = Camera.main.transform.position;
        screenSize.x = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0))) * 0.5f;
        screenSize.y = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height))) * 0.5f;


        foreach (KeyValuePair<string, Transform> valPair in colliders)
        {

            BoxCollider boxCollider = valPair.Value.gameObject.AddComponent<BoxCollider>();


            valPair.Value.name = valPair.Key + "Collider";


            valPair.Value.parent = transform;


            if (valPair.Key == "Left" || valPair.Key == "Right")
            {
                valPair.Value.localScale = new Vector3(colliderThickness, screenSize.y * 5, colliderThickness);
            }
            else
            {
                valPair.Value.localScale = new Vector3(screenSize.x * 2, colliderThickness, colliderThickness);
            }

        }


        colliders["Right"].position = new Vector3(cameraPos.x + screenSize.x + (colliders["Right"].localScale.x * 0.5f), cameraPos.y, zPosition);
        colliders["Left"].position = new Vector3(cameraPos.x - screenSize.x - (colliders["Left"].localScale.x * 0.5f), cameraPos.y, zPosition);
        //colliders["Top"].position = new Vector3(cameraPos.x, cameraPos.y + screenSize.y + (colliders["Top"].localScale.y * 0.5f), zPosition);
        colliders["Bottom"].position = new Vector3(cameraPos.x, cameraPos.y - screenSize.y - (colliders["Bottom"].localScale.y * 0.5f), zPosition);
        colliders["Bottom"].tag = "Ground";
        colliders["Bottom"].GetComponent<BoxCollider>().material = Ground_mat;
    }
}
