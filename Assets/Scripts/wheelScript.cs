using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wheelScript : MonoBehaviour
{
    private WheelCollider wheel;
    private Transform wheelMesh;
    public Transform debugArrow;

    float steering;
    void Start()
    {
        wheel = GetComponent< WheelCollider >();
        wheelMesh = GetComponentInChildren<Transform>();
    }

    void FixedUpdate()
    {
        //prende i valori di attrito del physic material e li applica alla ruota
        WheelHit hit;
        if (wheel.GetGroundHit(out hit))
        {
            WheelFrictionCurve fFriction = wheel.forwardFriction;
            fFriction.stiffness = hit.collider.material.staticFriction;
            wheel.forwardFriction = fFriction;
            WheelFrictionCurve sFriction = wheel.sidewaysFriction;
            sFriction.stiffness = hit.collider.material.staticFriction;
            wheel.sidewaysFriction = sFriction;
        }

        //rotazione della ruota
        wheelMesh.localEulerAngles = new Vector3(wheelMesh.localEulerAngles.x, wheel.steerAngle - wheelMesh.localEulerAngles.z, wheelMesh.localEulerAngles.z);
        //freccia che ruota insieme alla ruota, utile per il debug
        debugArrow.localEulerAngles = new Vector3(debugArrow.localEulerAngles.x, wheel.steerAngle - debugArrow.localEulerAngles.z, debugArrow.localEulerAngles.z);
        //codice che controlla la rotazione della ruota su se stessa, funziona ma il modello della ruota non è adatto ad essere ruotato
        /*wheelMesh.Rotate(wheel.rpm / 60 * 360 * Time.deltaTime, 0, 0);*/
    }
}
