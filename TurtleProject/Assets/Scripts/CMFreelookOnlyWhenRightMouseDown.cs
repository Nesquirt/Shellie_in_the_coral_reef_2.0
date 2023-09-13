using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CMFreelookOnlyWhenRightMouseDown : MonoBehaviour
{
    private CinemachineFreeLook cm;
    void Start()
    {
        CinemachineCore.GetInputAxis = GetAxisCustom;
        cm = GetComponent<CinemachineFreeLook>();
    }
    public float GetAxisCustom(string axisName)
    {
        if (axisName == "Mouse X")
        {
            if (Input.GetMouseButton(1))
            {
                if (Cursor.visible)
                    Cursor.visible = false;
                if (Cursor.lockState == CursorLockMode.None)
                    Cursor.lockState = CursorLockMode.Locked;

                //Disabilita il ricentramento automatico della telecamera finché tieni premuto tasto destro
                cm.m_RecenterToTargetHeading.m_enabled = false;
                cm.m_YAxisRecentering.m_enabled = false;
                return UnityEngine.Input.GetAxis("Mouse X");
            }
            else
            {   
                if (!Cursor.visible)
                    Cursor.visible = true;
                if (Cursor.lockState == CursorLockMode.Locked)
                    Cursor.lockState = CursorLockMode.None;

                cm.m_RecenterToTargetHeading.m_enabled = true;
                cm.m_YAxisRecentering.m_enabled = true;
                return 0;
            }
        }
        else if (axisName == "Mouse Y")
        {
            if (Input.GetMouseButton(1))
            {

                return UnityEngine.Input.GetAxis("Mouse Y");
            }
            else
            {
                return 0;
            }
        }
        return UnityEngine.Input.GetAxis(axisName);
    }
}

