using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyExercises : MonoBehaviour
{
    [SerializeField] Vector3 a1;
    [SerializeField] Vector3 b1;
    [SerializeField] Vector3 c1;


    public Vector3 Exercise1()
    {
        return c1 = a1 + b1;
    }

    public Vector3 Exercise2()
    {
        return c1 = b1 - a1;
    }

    //x*x y*y z*z
    public Vector3 Exercise3()
    {
        c1 = a1;

        c1.Scale(b1);

        return c1;
    }

    //esta mal!!!
    public Vector3 Exercise4()
    {
        return c1 = Vector3.Cross(a1, b1);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + a1);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + b1);

        c1 = Exercise4();

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + c1);
    }
}
