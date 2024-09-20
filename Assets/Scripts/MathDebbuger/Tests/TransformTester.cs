using UnityEngine;
using CustomMath;
using UnityEngine.UIElements;

public class TransformTester : MonoBehaviour
{
    [SerializeField] private Transform unityTransform;
    [SerializeField] private Transform testTransform;
    [SerializeField] private Transform point;

    [SerializeField] private float rotationX;
    [SerializeField] private float rotationY;
    [SerializeField] private float rotationZ;
    [SerializeField] private float inputSize;

    [SerializeField] private Vector3 unityLocalEuler;
    [SerializeField] private Vector3 myLocalEuler;

    [SerializeField] private Vector3 unityGlobalEuler;
    [SerializeField] private Vector3 myGlobalEuler;


    [SerializeField] private Vector3 unityLocalRotation;
    [SerializeField] private Vector3 myLocalRotation;

    [SerializeField] private Vector3 unityGlobalRotation;
    [SerializeField] private Vector3 myGlobalRotation;


    public My_Transform myTransform = new My_Transform();


    void Start()
    {
        myTransform.SetLocalPositionAndRotation(new Vec3(testTransform.localPosition), new My_Quaternion(testTransform.localRotation));
        myTransform.SetPositionAndRotation(new Vec3(testTransform.position), new My_Quaternion(testTransform.rotation));
    }

    void Update()
    {
        GetInput();

        //unityTransform.Rotate(rotationX, rotationY, rotationZ, Space.World);
        //myTransform.Rotate(rotationX, rotationY, rotationZ, Space.World);

        unityTransform.LookAt(point, Vec3.Up);
        myTransform.LookAt(point, Vec3.Up);

        testTransform.SetLocalPositionAndRotation(myTransform.localPosition, myTransform.localRotation.ToQuaternion());
        testTransform.SetPositionAndRotation(myTransform.position, myTransform.rotation.ToQuaternion());

        UpdateHood();
    }

    void GetInput()
    {
        float normalizedInput = inputSize * Time.deltaTime;

        if (Input.GetKey(KeyCode.W))
            point.Translate(0,0, normalizedInput);

        if (Input.GetKey(KeyCode.S))
            point.Translate(0, 0, -normalizedInput);

        if (Input.GetKey(KeyCode.A))
            point.Translate(-normalizedInput, 0, 0);

        if (Input.GetKey(KeyCode.D))
            point.Translate(normalizedInput, 0, 0);

        if (Input.GetKey(KeyCode.Q))
            point.Translate(0, normalizedInput, 0);

        if (Input.GetKey(KeyCode.E))
            point.Translate(0, -normalizedInput, 0);
    }

    void UpdateHood()
    {
        unityLocalEuler.x = unityTransform.localEulerAngles.x;
        unityLocalEuler.y = unityTransform.localEulerAngles.y;
        unityLocalEuler.z = unityTransform.localEulerAngles.z;
        unityGlobalEuler.x = unityTransform.eulerAngles.x;
        unityGlobalEuler.y = unityTransform.eulerAngles.y;
        unityGlobalEuler.z = unityTransform.eulerAngles.z;

        myLocalEuler.x = myTransform.localEulerAngles.x;
        myLocalEuler.y = myTransform.localEulerAngles.y;
        myLocalEuler.z = myTransform.localEulerAngles.z;
        myGlobalEuler.x = myTransform.eulerAngles.x;
        myGlobalEuler.y = myTransform.eulerAngles.y;
        myGlobalEuler.z = myTransform.eulerAngles.z;

        unityLocalRotation.x = unityTransform.localRotation.x;
        unityLocalRotation.y = unityTransform.localRotation.y;
        unityLocalRotation.z = unityTransform.localRotation.z;
        unityGlobalRotation.x = unityTransform.rotation.x;
        unityGlobalRotation.y = unityTransform.rotation.y;
        unityGlobalRotation.z = unityTransform.rotation.z;

        myLocalRotation.x = myTransform.localRotation.x;
        myLocalRotation.y = myTransform.localRotation.y;
        myLocalRotation.z = myTransform.localRotation.z;
        myGlobalRotation.x = myTransform.rotation.x;
        myGlobalRotation.y = myTransform.rotation.y;
        myGlobalRotation.z = myTransform.rotation.z;
    }
}
