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
    private WebGLConnection WebGL;

    [SerializeField]
    private GameObject Gizmo;
    /*[SerializeField]
    private Transform XEmpty;
    [SerializeField]
    private Transform YEmpty;
    [SerializeField]
    private Transform ZEmpty;*/

    public bool movingObject = false;
    public float movingObjectDirection;
    
    private Vector3 initMouseOffset;
    private Plane m_plane;
    
    // Start is called before the first frame update
    void Start()
    {
        Gizmo = Instantiate(Gizmo);
        Gizmo.SetActive(false);

        // Unity WebGL captures all Keyboard Inputs, but we need the keystrokes for the React GUI
        // This may help to switch them off for WebGL container
        //#if !UNITY_EDITOR && UNITY_WEBGL
        //    WebGLInput.captureAllKeyboardInput = false;
        //#endif

    }

    // Update is called once per frame
    void Update()
    {
        

        if (movingObject)
        {
            MoveObjectToMousePosition();
            UpdateGizmoPosition();
        }

        

    }

    public void UpdateMousePosition(InputAction.CallbackContext context)
    {
        
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
            WebGL.ItemInfoToWebGL(selectedObject.name,(int)selectedObject.transform.position.x);
        }
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
            Debug.Log("hit the gizmo");
            if (selectedObject !=null) {
                movingObject = true;
                movingObjectDirection = hit.transform.gameObject.transform.rotation.eulerAngles.x;
                m_plane = new Plane(hit.transform.gameObject.GetComponent<GizmoArrow>().PlaneNormalDirection, selectedObject.transform.position);
                Vector3 mouseposition = Mouse.current.position.ReadValue();
                initMouseOffset = mouseposition - Camera.main.WorldToScreenPoint(selectedObject.transform.position);
            }
            return;
        }
        
        
        if (selectedObject != null)
        {
            Destroy(selectedObject.GetComponent<Outline>());
            
        }

        if (Physics.Raycast(ray, out hit, 100))
        {
            Gizmo.SetActive(true);
            selectedObject = GetRoot(hit.transform.gameObject);
            //outline
            if (!selectedObject.TryGetComponent(out Outline ll))
            {
                var outline = selectedObject.AddComponent<Outline>();
                outline.OutlineMode = Outline.Mode.OutlineAll;
                outline.OutlineColor = Color.yellow;
                outline.OutlineWidth = 9f;
                WebGL.ItemInfoToWebGL(selectedObject.name,(int)selectedObject.transform.position.x);
            }

            //gizmo
            UpdateGizmoPosition();
            //Gizmo.transform.position = Camera.main.WorldToScreenPoint(selectedObject.transform.position);
            //Gizmo.GetComponent<SceneGizmoRenderer>().ReferenceTransform = selectedObject.transform;
            return;
        }

        selectedObject = null;
        Gizmo.SetActive(false);
    }
    
    
}
