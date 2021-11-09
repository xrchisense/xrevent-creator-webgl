using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class D3Camera : MonoBehaviour
{

    public CreatorControlerScript LevelManager;
    public Transform CameraBase;
    public Transform Camera;

    public bool disableScroll = false;
    private bool rightclickPressed = false;
    private bool middleclickPressed = false;
    private bool altPressed = false;
    private bool shiftPressed = false;

    public int MaxTilt = 85;
    public int MinTilt = 10;
    public float DragSpeed = 0.1f;
    Vector3 prev_Mousepostition;

    public void rightClick(InputAction.CallbackContext context)
    {
        rightclickPressed = context.ReadValueAsButton();
    }
    public void middleClick(InputAction.CallbackContext context)
    {
        middleclickPressed = context.ReadValueAsButton();
    }
    
    public void altClick(InputAction.CallbackContext context)
    {
        altPressed = context.ReadValueAsButton();
        LevelManager.movingObject = false;
    }
    public void shiftClick(InputAction.CallbackContext context)
    {
        shiftPressed = context.ReadValueAsButton();
    }

    public void lookAround(InputAction.CallbackContext context)
    {
        if (middleclickPressed)
        {
            Vector2 mouseMoevement = context.ReadValue<Vector2>();
            if (shiftPressed)
            {
                //drag
                Quaternion rotationCorrection = Quaternion.Euler(CameraBase.rotation.eulerAngles);

                CameraBase.position -= Quaternion.AngleAxis(rotationCorrection.eulerAngles[1], Vector3.up) *
                                       (DragSpeed * new Vector3(mouseMoevement[0], 0,
                                           mouseMoevement[1]));

            }else {
                //rotate

                Quaternion currentRotaiaon;
                Quaternion goalRotation;
                currentRotaiaon = CameraBase.rotation;
                goalRotation = currentRotaiaon;
                Vector3 toRotate = new Vector3(-mouseMoevement[1], mouseMoevement[0], 0);


                goalRotation.eulerAngles += toRotate;
                if (goalRotation.eulerAngles[0] > MaxTilt)
                {
                    toRotate[0] = 0;

                }
                else if (goalRotation.eulerAngles[0] < MinTilt)
                {
                    toRotate[0] = 0;
                }

                currentRotaiaon.eulerAngles += toRotate;
                CameraBase.rotation = currentRotaiaon;
                
                
            }
            
        }
    }

    public void Zoom(InputAction.CallbackContext context)
    {
        Vector3 oldPosition = Camera.localPosition;
        oldPosition[2] += (context.ReadValue<Vector2>().y/100);
        if (oldPosition[2] > -1) {return;}
        Camera.localPosition = oldPosition;

        
    }

    public void setRotationOnGizmoClick()
    {
        CameraBase.rotation = Quaternion.Euler(90,0,0);
    }
    
    // Update is called once per frame
    /*void Update()
    {

        if (Input.GetMouseButtonDown(2))
        {
            
            prev_Mousepostition = Input.mousePosition;

        }

        //middle Mouse rotate/+shift drag
        if (Input.GetMouseButton(2))
        {

            if (Input.GetKey(KeyCode.LeftShift))
            { //drag
                Quaternion rotationCorrection = Quaternion.Euler(CameraBase.rotation.eulerAngles);

                CameraBase.position -= Quaternion.AngleAxis(rotationCorrection.eulerAngles[1], Vector3.up) * (0.1f * new Vector3(Input.mousePosition[0] - prev_Mousepostition[0], 0, Input.mousePosition[1] - prev_Mousepostition[1]));

            }
            else
            {//rotate

                Quaternion currentRotaiaon;
                Quaternion goalRotation;
                currentRotaiaon = CameraBase.rotation;
                goalRotation = currentRotaiaon;
                Vector3 toRotate = new Vector3(-(Input.mousePosition[1] - prev_Mousepostition[1]), Input.mousePosition[0] - prev_Mousepostition[0], 0);


                goalRotation.eulerAngles += toRotate;
                if (goalRotation.eulerAngles[0] > MaxTilt)
                {
                    toRotate[0] = 0;

                }
                else if (goalRotation.eulerAngles[0] < MinTilt)
                {
                    toRotate[0] = 0;
                }
                currentRotaiaon.eulerAngles += toRotate;
                CameraBase.rotation = currentRotaiaon;
            }

            prev_Mousepostition = Input.mousePosition;


        }


        //Zoom
        if (disableScroll == false)
        {
            if (Input.mouseScrollDelta != new Vector2(0, 0))
            {
                Vector3 oldPosition = Camera.localPosition;
                oldPosition[2] += Input.mouseScrollDelta[1];
                Camera.localPosition = oldPosition;

            }
        }

    }*/


}
