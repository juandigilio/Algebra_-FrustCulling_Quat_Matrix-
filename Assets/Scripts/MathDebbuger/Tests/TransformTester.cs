using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomMath;

public class TransformTester : MonoBehaviour
{
    [SerializeField] private Transform unityTransform;
    [SerializeField] private Transform testTransform;

    [SerializeField] private float rotationX;
    [SerializeField] private float rotationY;
    [SerializeField] private float rotationZ;

    [SerializeField] private float unityLocalRotationX;
    [SerializeField] private float unityLocalRotationY;
    [SerializeField] private float unityLocalRotationZ;
    [SerializeField] private float unityGlobalRotationX;
    [SerializeField] private float unityGlobalRotationY;
    [SerializeField] private float unityGlobalRotationZ;

    [SerializeField] private float myLocalRotationX;
    [SerializeField] private float myLocalRotationY;
    [SerializeField] private float myLocalRotationZ;
    [SerializeField] private float myGlobalRotationX;
    [SerializeField] private float myGlobalRotationY;
    [SerializeField] private float myGlobalRotationZ;

    public My_Transform myTransform = new My_Transform();


    void Start()
    {
        myTransform.SetLocalPositionAndRotation(new Vec3(testTransform.localPosition), new My_Quaternion(testTransform.localRotation));
        myTransform.SetPositionAndRotation(new Vec3(testTransform.position), new My_Quaternion(testTransform.rotation));
    }

    void Update()
    {
        unityTransform.Rotate(rotationX, rotationY, rotationZ);
        myTransform.Rotate(rotationX, rotationY, rotationZ, Space.World);

        testTransform.SetPositionAndRotation(myTransform.position, myTransform.rotation.ToQuaternion());
        testTransform.SetLocalPositionAndRotation(myTransform.localPosition, myTransform.localRotation.ToQuaternion());

        unityLocalRotationX = unityTransform.localEulerAngles.x;
        unityLocalRotationY = unityTransform.localEulerAngles.y;
        unityLocalRotationZ = unityTransform.localEulerAngles.z;
        unityGlobalRotationX = unityTransform.eulerAngles.x;
        unityGlobalRotationY = unityTransform.eulerAngles.y;
        unityGlobalRotationZ = unityTransform.eulerAngles.z;


        myLocalRotationX = myTransform.localEulerAngles.x;
        myLocalRotationY = myTransform.localEulerAngles.y;
        myLocalRotationZ = myTransform.localEulerAngles.z;
        myGlobalRotationX = myTransform.eulerAngles.x;
        myGlobalRotationY = myTransform.eulerAngles.y;
        myGlobalRotationZ = myTransform.eulerAngles.z;
    }
}
