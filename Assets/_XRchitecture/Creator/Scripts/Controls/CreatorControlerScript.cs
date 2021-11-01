using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CreatorControlerScript : MonoBehaviour
{
    public Texture2D randomtexture = null;
    public GameObject selectedObject;

    /*[SerializeField]
    private GameObject Gizmo;
    [SerializeField]
    private Transform XEmpty;
    [SerializeField]
    private Transform YEmpty;
    [SerializeField]
    private Transform ZEmpty;

    */public Rect XRect;/*
    public RectTransform YRect;
    public RectTransform ZRect;*/

    public bool movingObject = false;
    // Start is called before the first frame update
    void Start()
    {
        XRect = new Rect(0,0,200,200);
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateGizmoPosition();

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
        Vector3 MouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseposition);
        MouseWorldPosition.z = 0;
        Vector3 oldObjectPosition = selectedObject.transform.position;
        Vector3 newObjectPosition = new Vector3(MouseWorldPosition.x, oldObjectPosition.y, oldObjectPosition.z);

        selectedObject.transform.position = newObjectPosition;

    }
    /*public void UpdateGizmoPosition()
    {
        if (selectedObject != null)
        {
            Gizmo.transform.position = selectedObject.transform.position;
            UpdateGizmoUI();
        }
        
    }

    public void UpdateGizmoUI()
    {
        Vector3 screenCalc = new Vector3(Screen.width / 2, Screen.height / 2,0);
        Vector3 centerposition = Camera.main.WorldToScreenPoint(Gizmo.transform.position);
        Vector3 Xposition = Camera.main.WorldToScreenPoint(XEmpty.position);
        Vector3 Yposition = Camera.main.WorldToScreenPoint(YEmpty.position);
        Vector3 Zposition = Camera.main.WorldToScreenPoint(ZEmpty.position);

  //      Debug.Log("Center: " + centerposition);
//        Debug.Log("X: " + Xposition);

        
        XRect.x = centerposition[0];
        XRect.y = Screen.height-Xposition[1];
        XRect.height = centerposition[1] - Xposition[1];
        XRect.width =  Xposition[0] - centerposition[0];

        
        YRect.localPosition = centerposition-screenCalc;

    }*/

    void OnGUI()
    {
        GUI.color = Color.black;
        GUI.DrawTexture(XRect,randomtexture);
    }

    public void leftClicktoSelect(InputAction.CallbackContext context)
    {
        //TODO check if infront of ui object
        if (EventSystem.current.IsPointerOverGameObject()) return;
        
        if (movingObject)
        {
            movingObject = false;
            return;
        };
        
        
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        //Check if Gizmoclicked
        int layerMask = 1 << 8;
        if (Physics.Raycast(ray, out hit, 100,layerMask))
        {
            Debug.Log("hit the gizmo");

            movingObject = true;
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
            /*Gizmo.transform.position = Camera.main.WorldToScreenPoint(selectedObject.transform.position);*/
            //Gizmo.GetComponent<SceneGizmoRenderer>().ReferenceTransform = selectedObject.transform;

        }
    }
}
