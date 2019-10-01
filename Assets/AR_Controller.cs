using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AR_Controller : MonoBehaviour
{
    private Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    float rot = 0f;

    public void ManualCalibration()
    {
        if (ManualCalibrationCanvas.Instance.gameObject.activeSelf == true)
        {
            ManualCalibrationCanvas.Instance.gameObject.SetActive(false);
        }
        else
        {
            ManualCalibrationCanvas.Instance.gameObject.SetActive(true);

        }
    }

    public void R_Plus()
    {

        rot += 0.5f;

        transform.rotation = Quaternion.Euler(0, rot, 0);

    }



    public void R_Minus()
    {

        rot -= 0.5f;

        transform.rotation = Quaternion.Euler(0, rot, 0);
    }


    public void Posx_plus()
    {

        pos.x += 0.5f;
        transform.position = pos;

    }

    public void Posx_minus()
    {
        pos.x -= 0.5f;
        transform.position = pos;

    }

    public void Posz_plus()
    {
        pos.z += 0.5f;
        transform.position = pos;

    }


    public void Posz_minus()
    {
        pos.z -= 0.5f;
        transform.position = pos;

    }
}







