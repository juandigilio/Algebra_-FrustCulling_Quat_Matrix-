using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformTester : MonoBehaviour
{
    [SerializeField] private Transform unityTransform;
    [SerializeField] private Transform testTransform;
    [SerializeField] private float rotation;
    [SerializeField] private float unityLocalRotation;
    [SerializeField] private float unityGlobalRotation;
    [SerializeField] private float myLocalRotation;
    [SerializeField] private float myGlobalRotation;

    public My_Transform myTransform = new My_Transform();



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //unityTransform.rotation = Quaternion.Euler(0, rotation, 0);
        //myTransform.rotation = My_Quaternion.Euler(0, rotation, 0);

        unityTransform.Rotate(0, rotation, 0);
        myTransform.Rotate(0, rotation, 0);

        unityLocalRotation = unityTransform.localEulerAngles.y;
        unityGlobalRotation = unityTransform.eulerAngles.y;
        myLocalRotation = myTransform.localEulerAngles.y;
        myGlobalRotation = myTransform.eulerAngles.y;
    }
}
