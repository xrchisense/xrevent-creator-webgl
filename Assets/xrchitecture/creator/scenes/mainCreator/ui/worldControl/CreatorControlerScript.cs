using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CreatorControlerScript : MonoBehaviour
{
    public Texture2D randomtexture = null;
    public GameObject selectedObject;

    [SerializeField]
    private GameObject Gizmo;
    [SerializeField]
    private Transform XEmpty;
    [SerializeField]
    private Transform YEmpty;
    [SerializeField]
    private Transform ZEmpty;

    public bool movingObject = false;
    public float movingObjectDirection;

    private Vector2 _previousMousePosition;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateGizmoPosition();

        if (movingObject)
        {
            MoveObjectToMousePosition();
        }

        

    }

    public void UpdateMousePosition(InputAction.CallbackContext context)
    {
        
    }

    public void MoveObjectToMousePosition()
    {
        Vector3 mouseposition = Mouse.current.position.ReadValue();
        mouseposition.z = 20;
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseposition);
        Vector3 oldObjectPosition = selectedObject.transform.position;
        Vector3 newObjectPosition;
        Debug.Log(movingObjectDirection);
        movingObjectDirection = (int)Math.Round(movingObjectDirection);
        switch (movingObjectDirection)
        {
            case 45:
                Debug.Log("HitX");
                newObjectPosition = new Vector3(mouseWorldPosition.x, oldObjectPosition.y, oldObjectPosition.z);
                break;
            case 90:
                Debug.Log("HitZ");
                newObjectPosition = new Vector3(oldObjectPosition.x, oldObjectPosition.y, mouseWorldPosition.z);
                break;
            case 0:
                newObjectPosition = new Vector3(oldObjectPosition.x, mouseWorldPosition.y, oldObjectPosition.z);
                break;
            default:
                Debug.Log("default");
                newObjectPosition = new Vector3(oldObjectPosition.x, oldObjectPosition.y, oldObjectPosition.z);
                break;
        }
        
        selectedObject.transform.position = newObjectPosition;

    }
    public void UpdateGizmoPosition()
    {
        if (selectedObject != null)
        {
            Gizmo.transform.position = selectedObject.transform.position;
        }
        
    }

    /*public void UpdateGizmoUI()
    {
        Vector3 screenCalc = new Vector3(Screen.width / 2, Screen.height / 2,0);
        Vector3 centerposition = Camera.main.WorldToScreenPoint(Gizmo.transform.position);
        Vector3 Xposition = Camera.main.WorldToScreenPoint(XEmpty.position);
        Vector3 Yposition = Camera.main.WorldToScreenPoint(YEmpty.position);
        Vector3 Zposition = Camera.main.WorldToScreenPoint(ZEmpty.position);

        Debug.Log("Center: " + centerposition);
        Debug.Log("X: " + Xposition);

        
        XRect.x = centerposition[0];
        XRect.y = Screen.height-Xposition[1];
        XRect.height = centerposition[1] - Xposition[1];
        XRect.width =  Xposition[0] - centerposition[0];

        
        YRect.localPosition = centerposition-screenCalc;

    }*/

    /*void OnGUI()
    {
        GUI.color = Color.black;
        GUI.DrawTexture(XRect,randomtexture);
    }*/

    public void leftClicktoSelect(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed){return;} //ignore "performed" status
        
        if (context.phase == InputActionPhase.Canceled) //check if mouse is let go, if movign drop the item, else do nothing
        {
            if (movingObject) { movingObject = false;};
            return;
        }
        
        if (EventSystem.current.IsPointerOverGameObject()) return;  //check if infront of ui object
        
        //Check if Gizmoclicked
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        int layerMask = 1 << 8;
        if (Physics.Raycast(ray, out hit, 300,layerMask))
        {
            Debug.Log("hit the gizmo");
            if (selectedObject !=null) {
                movingObject = true;
                movingObjectDirection = hit.transform.gameObject.transform.rotation.eulerAngles.x;
            }
            return;
        }
        
        
        if (selectedObject != null)
        {
            Destroy(selectedObject.GetComponent<Outline>());
            
        }
        
        if (Physics.Raycast(ray, out hit, 100))
        {
            selectedObject = hit.transform.gameObject;
            //outline
            if (!selectedObject.TryGetComponent(out Outline ll))
            {
                var outline = selectedObject.AddComponent<Outline>();
                outline.OutlineMode = Outline.Mode.OutlineAll;
                outline.OutlineColor = Color.yellow;
                outline.OutlineWidth = 9f;
            }
            //gizmo
            Gizmo.transform.position = Camera.main.WorldToScreenPoint(selectedObject.transform.position);
            //Gizmo.GetComponent<SceneGizmoRenderer>().ReferenceTransform = selectedObject.transform;

        }
    }
}
