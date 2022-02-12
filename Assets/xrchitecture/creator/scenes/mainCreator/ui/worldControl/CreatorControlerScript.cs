using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Xrchitecture.Creator.Common.Data;

public class CreatorControlerScript : MonoBehaviour
{
    public CreatorLevelManager levelManager;
    public GameObject selectedObject;

    [SerializeField]
    private GameObject gizmo;

    public GameObject grid;
    
    public bool movementLocal;
    public bool movingObject;
    public bool rotatingObject;
    
    public float movingObjectDirection;
    public Vector3 rotateObjectDirection;
    
    private Vector3 _initMouseOffset;
    private Vector3 ?_lastRotateVector;
    private Plane _mPlane;
    
    // Start is called before the first frame update
    void Start()
    {
        gizmo = Instantiate(gizmo);
        gizmo.SetActive(false);

        grid = Instantiate(grid);

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

        Plane r_plane = new Plane(rotateObjectDirection, selectedObject.transform.position);
        float zdistance = 0.0f;
        
        if (_lastRotateVector == null)
        {
            Ray fray = Camera.main.ScreenPointToRay(_initMouseOffset);
            if (r_plane.Raycast(fray, out zdistance))
            {
                _lastRotateVector = fray.GetPoint(zdistance) - selectedObject.transform.position;
            }
        }
        
        
        
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        

        if (r_plane.Raycast(ray, out zdistance))
        {
            Vector3 mouseWorldPosition = ray.GetPoint(zdistance);
            Vector3 rotateVector = mouseWorldPosition - selectedObject.transform.position;
            float angleToRotate = Vector3.SignedAngle(rotateVector, (Vector3)_lastRotateVector,rotateObjectDirection);

            selectedObject.transform.Rotate(rotateObjectDirection, -angleToRotate, Space.World);

            _lastRotateVector = rotateVector;
        }
    }

    public void MoveObjectToMousePosition()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        mousePosition -= _initMouseOffset;
        //create a movement plane
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        float zdistance = 0.0f;
        if (_mPlane.Raycast(ray, out zdistance))
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
            gizmo.transform.position = selectedObject.transform.position;
            if (movementLocal)
            {
                gizmo.transform.rotation = selectedObject.transform.rotation;
            }
            return;
        }
        gizmo.SetActive(false);
    }

    private void UnselectItem()
    {
        if (selectedObject == null) {return;}
        
        if (selectedObject.TryGetComponent<Outline>(out Outline t))
        {
            Destroy(selectedObject.GetComponent<Outline>());
        }
        selectedObject = null;
        gizmo.SetActive(false);
        
        levelManager.ReportObjectInfo();
    }
    

    public GameObject GetRoot(GameObject gO)
    {
        return gO.GetComponentInParent<CreatorItem>().GetThisObjectRoot();
    }

    public void leftClicktoSelect(InputAction.CallbackContext context)
    {
        if(levelManager.clickInsideUnityView == false){return;}    //ignore clicks outsided of unity
        if(context.phase == InputActionPhase.Performed){return;} //ignore "performed" status
        
        if (context.phase == InputActionPhase.Canceled) //check if mouse is let go, if moving drop the item, else do nothing
        {
            if (movingObject) { movingObject = false;};
            if (rotatingObject)
            {
                rotatingObject = false;
                _lastRotateVector = null;
            };
            levelManager.ReportObjectInfo();
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
                _mPlane = new Plane(arrow.PlaneNormalDirection, selectedObject.transform.position);
                Vector3 mouseposition = Mouse.current.position.ReadValue();
                _initMouseOffset = mouseposition - Camera.main.WorldToScreenPoint(selectedObject.transform.position);
            }
            //Check if rotating:
            if (hit.transform.TryGetComponent<GizmoRotator>(out GizmoRotator rotator))
            {
                rotatingObject = true;
                rotateObjectDirection = rotator.PlaneNormalDirection;
                _initMouseOffset = Mouse.current.position.ReadValue();
            }
                
            
            return;
        }

        int IgnoreLayerEleven = 1<< 11;
        
        //prepare Object Switch
        if (selectedObject != null)
        {
            Destroy(selectedObject.GetComponent<Outline>());
            gizmo.SetActive(false);
        }

        //check if new Object Hit
        if (Physics.Raycast(ray, out hit, 1000,~IgnoreLayerEleven))
        {
            //if the hit Object is the same as the last one, unselect Item
            if (selectedObject == GetRoot(hit.transform.gameObject))
            {
                UnselectItem();
                return;
            }
            
            //select new Object
            gizmo.SetActive(true);
            selectedObject = GetRoot(hit.transform.gameObject);
            //Add outline
            if (!selectedObject.TryGetComponent(out Outline ll))
            {
                var outline = selectedObject.AddComponent<Outline>();
                outline.OutlineMode = Outline.Mode.OutlineAll;
                outline.OutlineColor = Color.yellow;
                outline.OutlineWidth = 9f;
                
            }
            UpdateGizmoPosition();
            
            //send to WebGL
            levelManager.ReportObjectInfo();
            return;
        }
        
        //if nothing is clicked/hit, unselect Item
        UnselectItem();
        
    }
    
    
}
