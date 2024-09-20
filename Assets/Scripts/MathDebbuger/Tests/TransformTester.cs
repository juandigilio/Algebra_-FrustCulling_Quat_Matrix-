using UnityEngine;
using CustomMath;
using UnityEngine.UIElements;

public class TransformTester : MonoBehaviour
{
    [SerializeField] private Transform unityTransform;
    [SerializeField] private Transform testTransform;
    [SerializeField] private Transform children_1;
    [SerializeField] private Transform children_2;
    [SerializeField] private Transform childOfChild;
    [SerializeField] private Transform point;

    public My_Transform myTransform = new My_Transform();
    public My_Transform myChildren_1 = new My_Transform();
    public My_Transform myChildren_2 = new My_Transform();
    public My_Transform myGrandChildren = new My_Transform();

    [SerializeField] private float rotationX;
    [SerializeField] private float rotationY;
    [SerializeField] private float rotationZ;
    [SerializeField] private float inputSize;

    //[SerializeField] private Vector3 unityLocalEuler;
    [SerializeField] private Vector3 myLocalEuler;
    [SerializeField] private Vector3 mychildrenLocalEuler;

    //[SerializeField] private Vector3 unityGlobalEuler;
    [SerializeField] private Vector3 myGlobalEuler;
    [SerializeField] private Vector3 mychildrenGlobalEuler;

    //[SerializeField] private Vector3 unityLocalRotation;
    [SerializeField] private Vector3 myLocalRotation;
    [SerializeField] private Vector3 mychildrenLocalRotation;

    //[SerializeField] private Vector3 unityGlobalRotation;
    [SerializeField] private Vector3 myGlobalRotation;
    [SerializeField] private Vector3 mychildrenGlobalRotation;

    [SerializeField] private Vector3 childrenLocalRotation;
    [SerializeField] private Vector3 childrenGlobalRotation;

    [SerializeField] private Vector3 childrenLocalPosition;
    [SerializeField] private Vector3 childrenGlobalPosition;


    void Start()
    {
        myTransform.SetLocalPositionAndRotation(new Vec3(testTransform.localPosition), new My_Quaternion(testTransform.localRotation));
        myTransform.SetPositionAndRotation(new Vec3(testTransform.position), new My_Quaternion(testTransform.rotation));

        myChildren_1.SetLocalPositionAndRotation(new Vec3(children_1.localPosition), new My_Quaternion(children_1.localRotation));
        myChildren_1.SetPositionAndRotation(new Vec3(children_1.position), new My_Quaternion(children_1.rotation));

        myChildren_2.SetLocalPositionAndRotation(new Vec3(children_2.localPosition), new My_Quaternion(children_2.localRotation));
        myChildren_2.SetPositionAndRotation(new Vec3(children_2.position), new My_Quaternion(children_2.rotation));

        myGrandChildren.SetLocalPositionAndRotation(new Vec3(childOfChild.localPosition), new My_Quaternion(childOfChild.localRotation));
        myGrandChildren.SetPositionAndRotation(new Vec3(childOfChild.position), new My_Quaternion(childOfChild.rotation));

        myChildren_1.SetParent(myTransform);
        myChildren_2.SetParent(myTransform);

        myGrandChildren.SetParent(myChildren_1);
    }

    void Update()
    {
        GetInput();

        //unityTransform.Rotate(rotationX, rotationY, rotationZ, Space.World);
        //myTransform.Rotate(rotationX, rotationY, rotationZ, Space.World);

        unityTransform.LookAt(point, Vec3.Up);
        myTransform.LookAt(point, Vec3.Up);

        unityTransform.RotateAround(point.position, Vec3.Up, rotationX);
        myTransform.RotateAround(point.position, Vec3.Up, rotationX);

        testTransform.SetLocalPositionAndRotation(myTransform.localPosition, myTransform.localRotation.ToQuaternion());
        testTransform.SetPositionAndRotation(myTransform.position, myTransform.rotation.ToQuaternion());

        //children_1.SetLocalPositionAndRotation(myChildren_1.localPosition, myChildren_1.localRotation.ToQuaternion());
        children_1.SetPositionAndRotation(myChildren_1.position, myChildren_1.rotation.ToQuaternion());

        //children_2.SetLocalPositionAndRotation(myChildren_2.localPosition, myChildren_2.localRotation.ToQuaternion());
        children_2.SetPositionAndRotation(myChildren_2.position, myChildren_2.rotation.ToQuaternion());

        //childOfChild.SetLocalPositionAndRotation(myGrandChildren.localPosition, myGrandChildren.localRotation.ToQuaternion());
        childOfChild.SetPositionAndRotation(myGrandChildren.position, myGrandChildren.rotation.ToQuaternion());


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
        //unityLocalEuler.x = unityTransform.localEulerAngles.x;
        //unityLocalEuler.y = unityTransform.localEulerAngles.y;
        //unityLocalEuler.z = unityTransform.localEulerAngles.z;
        //unityGlobalEuler.x = unityTransform.eulerAngles.x;
        //unityGlobalEuler.y = unityTransform.eulerAngles.y;
        //unityGlobalEuler.z = unityTransform.eulerAngles.z;

        myLocalEuler.x = myTransform.localEulerAngles.x;
        myLocalEuler.y = myTransform.localEulerAngles.y;
        myLocalEuler.z = myTransform.localEulerAngles.z;
        myGlobalEuler.x = myTransform.eulerAngles.x;
        myGlobalEuler.y = myTransform.eulerAngles.y;
        myGlobalEuler.z = myTransform.eulerAngles.z;

        //unityLocalRotation.x = unityTransform.localRotation.x;
        //unityLocalRotation.y = unityTransform.localRotation.y;
        //unityLocalRotation.z = unityTransform.localRotation.z;
        //unityGlobalRotation.x = unityTransform.rotation.x;
        //unityGlobalRotation.y = unityTransform.rotation.y;
        //unityGlobalRotation.z = unityTransform.rotation.z;

        myLocalRotation.x = myTransform.localRotation.x;
        myLocalRotation.y = myTransform.localRotation.y;
        myLocalRotation.z = myTransform.localRotation.z;
        myGlobalRotation.x = myTransform.rotation.x;
        myGlobalRotation.y = myTransform.rotation.y;
        myGlobalRotation.z = myTransform.rotation.z;

        childrenLocalRotation.x = myChildren_1.localRotation.x;
        childrenLocalRotation.y = myChildren_1.localRotation.y;
        childrenLocalRotation.z = myChildren_1.localRotation.z;
        childrenGlobalRotation.x = myChildren_1.rotation.x;
        childrenGlobalRotation.y = myChildren_1.rotation.y;
        childrenGlobalRotation.z = myChildren_1.rotation.z;

        childrenLocalPosition.x = myChildren_1.localPosition.x;
        childrenLocalPosition.y = myChildren_1.localPosition.y;
        childrenLocalPosition.z = myChildren_1.localPosition.z;
        childrenGlobalPosition.x = myChildren_1.position.x;
        childrenGlobalPosition.y = myChildren_1.position.y;
        childrenGlobalPosition.z = myChildren_1.position.z;
    }
}
