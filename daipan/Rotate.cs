using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    //[SerializeField] private Vector3 _angleVelocity;
    private float target_rotate;
    //public GameObject table;
    private bool OnTheBoard = true;
 
    void Update()
    {

        if (!OnTheBoard)
        {
            
            var target = Quaternion.Euler(new Vector3(target_rotate, 0, 0));
            var now_rot = transform.rotation;
            /*
            Debug.Log(Quaternion.Angle(now_rot, target));
            Debug.Log("x:" + now_rot.x + ", y:" + now_rot.y + ", z:" + now_rot.z);
            Debug.Log("rotate:"+target_rotate);
            Debug.Log(Mathf.RoundToInt(transform.localEulerAngles.x));
            Debug.Log("");
            */
           // Debug.Log("x:" + transform.localEulerAngles.x + ", y:" + transform.localEulerAngles.y + ", z:" + transform.localEulerAngles.z);

            // var target = table.transform.rotation;
            if (Quaternion.Angle(now_rot, target) <= 1)
            {
                transform.rotation = target;
            }
            else
            {
                transform.Rotate(new Vector3(1.6f, 0, 0));
            }





        }
        
    }
    void OnCollisionStay(Collision collision)
    {
        
      

        OnTheBoard = true;
        if (Mathf.RoundToInt(transform.rotation.x)==0)
        {
            target_rotate = 180;
        }
        else
        {
            target_rotate = 360;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (OnTheBoard)
        {
          
            Quaternion angle = transform.rotation;
            angle.y = 0;
            angle.z = 0;
            transform.rotation = angle;
        }

        OnTheBoard = false;
    }

}
