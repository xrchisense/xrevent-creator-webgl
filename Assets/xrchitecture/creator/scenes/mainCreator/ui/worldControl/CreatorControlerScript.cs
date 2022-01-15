using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Xrchitecture.Creator.Common.Data;

public class CreatorControlerScript : MonoBehaviour
{
    public Texture2D randomtexture = null;
    public GameObject selectedObject;

    [SerializeField]
    private GameObject Gizmo;
    /*[SerializeField]
    private Transform XEmpty;
    [SerializeField]
    private Transform YEmpty;
    [SerializeField]
    private Transform ZEmpty;*/

    public bool movementLocal;
    public bool movingObject = false;
    public float movingObjectDirection;

    public bool rotatingObject;
    public Vector3 rotateObjectDirection;
    public Vector3 lastScreenMousePosition;
    
    private Vector3 initMouseOffset;
    private Plane m_plane;
    
    // Start is called before the first frame update
    void Start()
    {
        Gizmo = Instantiate(Gizmo);
        Gizmo.SetActive(false);
        
        //stupid fix so my GPU is not at 100% all the time :D
        #if UNITY_STANDALONE || UNITY_EDITOR
        Application.targetFrameRate = 60;
        #endif

    }

    // Update is called once per frame
    void Update()
    {
        

        if (movingObject)
        {
            
            MoveObjectToMousePosition();
            UpdateGizmoPosition();
        }

        if (rotatingObject)
        {
            RotateObjectToMousePosition();
        }

        

    }

    public void RotateObjectToMousePosition()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        selectedObject.transform.Rotate(rotateObjectDirection,Vector3.Distance(mousePosition,lastScreenMousePosition),Space.World);
        if (Vector3.Distance(mousePosition,lastScreenMousePosition) != 0 )
            Debug.Log("Distance: "+ Vector3.Distance(mousePosition,lastScreenMousePosition) +" Minus: " + (mousePosition-lastScreenMousePosition));
        lastScreenMousePosition = mousePosition;

    }

    public void MoveObjectToMousePosition()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        mousePosition -= initMouseOffset;
        //create a movement plane
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        float zdistance = 0.0f;
        if (m_plane.Raycast(ray, out zdistance))
        {
            Vector3 mouseWorldPosition = ray.GetPoint(zdistance);
            Vector3 oldObjectPosition = selectedObject.transform.position;
            movingObjectDirection = (int) Math.Round(movingObjectDirection);

            Vector3 newObjectPosition = movingObjectDirection switch
            {
                45 => new Vector3(mouseWorldPosition.x, oldObjectPosition.y, oldObjectPosition.z),
                90 => new Vector3(oldObjectPosition.x, oldObjectPosition.y, mouseWorldPosition.z),
                0 => new Vector3(oldObjectPosition.x, mouseWorldPosition.y, oldObjectPosition.z),
                _ => new Vector3(mouseWorldPosition.x, mouseWorldPosition.y, mouseWorldPosition.z)
            };

            selectedObject.transform.position = newObjectPosition;
            //TODO: Send Item Info in intervals!
            //WebGL.ItemInfoToWebGL(selectedObject.name,(int)selectedObject.transform.position.x);
        }
    }
    public void UpdateGizmoPosition()
    {
        if (selectedObject != null)
        {
            Gizmo.transform.position = selectedObject.transform.position;
            if (movementLocal)
            {
                Gizmo.transform.rotation = selectedObject.transform.rotation;
            }
            
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

    public GameObject GetRoot(GameObject gO)
    {
        return gO.GetComponentInParent<CreatorItem>().GetThisObjectRoot();
    }

    public void leftClicktoSelect(InputAction.CallbackContext context)
    {
        
        if(context.phase == InputActionPhase.Performed){return;} //ignore "performed" status
        
        if (context.phase == InputActionPhase.Canceled) //check if mouse is let go, if moving drop the item, else do nothing
        {
            if (movingObject) { movingObject = false;};
            if (rotatingObject) { rotatingObject = false;};
            return;
        }

        //check if infront of ui object
        if (FindObjectOfType<EventSystem>()) {if (EventSystem.current.IsPointerOverGameObject()){return;}} 

        //Check if Gizmoclicked
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        int layerMask = 1 << 8;
        if (Physics.Raycast(ray, out hit, 300,layerMask))
        {
            if (selectedObject == null) { return; }
            Debug.Log("hit the gizmo");
            //Check if moving:
            if (hit.transform.TryGetComponent<GizmoArrow>(out GizmoArrow arrow))
            {
                movingObject = true;
                movingObjectDirection = hit.transform.gameObject.transform.rotation.eulerAngles.x;
                m_plane = new Plane(arrow.PlaneNormalDirection, selectedObject.transform.position);
                Vector3 mouseposition = Mouse.current.position.ReadValue();
                initMouseOffset = mouseposition - Camera.main.WorldToScreenPoint(selectedObject.transform.position);
            }
            //Check if rotating:
            if (hit.transform.TryGetComponent<GizmoRotator>(out GizmoRotator rotator))
            {
                rotatingObject = true;
                rotateObjectDirection = rotator.PlaneNormalDirection;
                lastScreenMousePosition = Mouse.current.position.ReadValue();
            }
                
            
            return;
        }
        
        
        if (selectedObject != null)
        {
            Destroy(selectedObject.GetComponent<Outline>());
            Gizmo.SetActive(false);
        }

        if (Physics.Raycast(ray, out hit, 100))
        {
            if (selectedObject == GetRoot(hit.transform.gameObject))
            {
                selectedObject = null;
                Gizmo.SetActive(false);
                return;
            }
            Gizmo.SetActive(true);
            selectedObject = GetRoot(hit.transform.gameObject);
            //outline
            if (!selectedObject.TryGetComponent(out Outline ll))
            {
                var outline = selectedObject.AddComponent<Outline>();
                outline.OutlineMode = Outline.Mode.OutlineAll;
                outline.OutlineColor = Color.yellow;
                outline.OutlineWidth = 9f;
                //TODO: Send Item Info in intervals!
                //WebGL.ItemInfoToWebGL(selectedObject.name,(int)selectedObject.transform.position.x);
            }

            //gizmo
            UpdateGizmoPosition();
            //Gizmo.transform.position = Camera.main.WorldToScreenPoint(selectedObject.transform.position);
            //Gizmo.GetComponent<SceneGizmoRenderer>().ReferenceTransform = selectedObject.transform;
            return;
        }
        
        //if nothing is clicked:
        selectedObject = null;
        Gizmo.SetActive(false);
        
    }
    
    
}
