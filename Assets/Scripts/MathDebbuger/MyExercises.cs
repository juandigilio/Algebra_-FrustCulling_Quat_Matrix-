using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyExercises : MonoBehaviour
{
    [SerializeField] Vector3 a1;
    [SerializeField] Vector3 b1;
    [SerializeField] Vector3 c1;

    [SerializeField] float duration = 1.0f;
    private float elapsedTime = 0.0f;


    public Vector3 Exercise1()
    {
        return a1 + b1;
    }

    public Vector3 Exercise2()
    {
        return b1 - a1;
    }

    //x*x y*y z*z
    public Vector3 Exercise3()
    {
        Vector3 newVec = a1;

        newVec.Scale(b1);

        return newVec;
    }

    //producto cruz
    public Vector3 Exercise4()
    {
        return Vector3.Cross(b1, a1);
    }

    public Vector3 Exercise5()
    {
        elapsedTime += Time.deltaTime;

        float t = Mathf.Clamp01(elapsedTime / duration);

        Vector3 newVec = Vector3.Lerp(a1, b1, t);

        transform.position = newVec;

        if (t >= 1.0f)
        {
            elapsedTime = 0.0f;
        }

        return newVec;
    }

    public Vector3 Exercise6()
    {
        return Vector3.Max(a1, b1);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + a1);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + b1);

        c1 = Exercise6();

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + c1);
    }
}
