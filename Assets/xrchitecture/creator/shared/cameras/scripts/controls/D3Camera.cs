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
    private bool rightClickPressed = false;
    private bool middleClickPressed = false;
    private bool altPressed = false;
    private bool shiftPressed = false;

    public int MaxTilt = 85;
    public int MinTilt = 10;
    public float DragSpeed = 0.1f;
    public float RotateSpeed = 0.5f;
    Vector3 prev_Mousepostition;

    public void rightClick(InputAction.CallbackContext context)
    {
        rightClickPressed = context.ReadValueAsButton();
        Debug.Log(rightClickPressed);
    }
    public void middleClick(InputAction.CallbackContext context)
    {
        middleClickPressed = context.ReadValueAsButton();
        Debug.Log(middleClickPressed);
    }
    
    public void altClick(InputAction.CallbackContext context)
    {
        altPressed = context.ReadValueAsButton();
        LevelManager.movingObject = false;
        Debug.Log(shiftPressed);
    }
    public void shiftClick(InputAction.CallbackContext context)
    {
        shiftPressed = context.ReadValueAsButton();
        Debug.Log(shiftPressed);
    }

    public void lookAround(InputAction.CallbackContext context)
    {
        if (middleClickPressed)
        {
            Vector2 mouseMoevement = context.ReadValue<Vector2>();
            if (shiftPressed)
            {
                //drag
                Quaternion rotationCorrection = Quaternion.Euler(CameraBase.rotation.eulerAngles);
                
                //Solved by Kevin H.
                CameraBase.position -= Quaternion.AngleAxis(rotationCorrection.eulerAngles[1], Camera.up) * Quaternion.AngleAxis(rotationCorrection.eulerAngles[0], Camera.right) *
                                       (DragSpeed * new Vector3(mouseMoevement[0], mouseMoevement[1],0
                                           ));

            }else {
                //rotate

                Quaternion currentRotaiaon;
                Quaternion goalRotation;
                currentRotaiaon = CameraBase.rotation;
                goalRotation = currentRotaiaon;
                Vector3 toRotate = new Vector3(-mouseMoevement[1], mouseMoevement[0], 0);


                goalRotation.eulerAngles += toRotate;
                if (goalRotation.eulerAngles[0] > MaxTilt && MaxTilt >0)
                {
                    toRotate[0] = 0;
                }
                else if (goalRotation.eulerAngles[0] < MinTilt && MinTilt >0)
                {
                    toRotate[0] = 0;
                }

                currentRotaiaon.eulerAngles += toRotate*RotateSpeed;
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
}
