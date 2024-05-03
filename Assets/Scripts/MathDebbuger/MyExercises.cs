using MathDebbuger;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Exercices
{
    Uno = 1,
    Dos = 2,
    Tres = 3,
    Cuatro = 4,
    Cinco = 5,
    Seis = 6,
    Siete = 7,
    Ocho = 8,
    Nueve = 9,
    Diez = 10
}

public class MyExercises : MonoBehaviour
{
    [SerializeField] private Exercices exercise;

    [SerializeField] private Vector3 a1;
    [SerializeField] private Vector3 b1;
    [SerializeField] private Vector3 c1;

    [SerializeField] private Vector3 hisResult;
    [SerializeField] private float t;
    [SerializeField] private float distance_A1B1;
    [SerializeField] private float resultMagnitude;
    [SerializeField] private float hisResultMagnitude;
    [SerializeField] private float magnitude_A1;
    [SerializeField] private float magnitude_B1;

    [SerializeField] private float angle_A1B1;
    [SerializeField] private float angle_A1_hisResult;
    [SerializeField] private float angle_B1_hisResult;
    [SerializeField] private float resultingAngle;
    [SerializeField] private float lastAngle_a1;
    [SerializeField] private float lastAngle_b1;



    [SerializeField] private float duration = 1.0f;
    private float elapsedTime = 0.0f;

    private List<string> vectorId = new List<string>();

    private void Start()
    {
        vectorId.Add("Vector 1");
        vectorId.Add("Vector 2");
        vectorId.Add("Vector 3");

        Vector3Debugger.AddVector(a1, Color.white, vectorId[0]);
        Vector3Debugger.AddVector(b1, Color.black, vectorId[1]);
        Vector3Debugger.AddVector(c1, Color.red, vectorId[2]);

        a1.x = 1;
        a1.y = 2;
        a1.z = 1;

        b1.x = 4;
        b1.y = 3;
        b1.z = 2;
    }

    private void Update()
    {
        switch ((int)exercise)
        {
            case 1:
                {
                    c1 = Exercise1();
                    break;
                }
            case 2:
                {
                    c1 = Exercise2();
                    break;
                }
            case 3:
                {
                    c1 = Exercise3();
                    break;
                }
            case 4:
                {
                    c1 = Exercise4();
                    break;
                }
            case 5:
                {
                    c1 = Exercise5();
                    break;
                }
            case 6:
                {
                    c1 = Exercise6();
                    break;
                }
            case 7:
                {
                    c1 = Exercise7();
                    break;
                }
            case 8:
                {
                    c1 = Exercise8();
                    break;
                }
            case 9:
                {
                    c1 = Exercise9();
                    break;
                }
            case 10:
                {
                    c1 = Exercise10();
                    break;
                }
        }

        Vector3Debugger.UpdatePosition(vectorId[0], a1);
        Vector3Debugger.UpdatePosition(vectorId[1], b1);
        Vector3Debugger.UpdatePosition(vectorId[2], c1);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + a1);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + b1);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + c1);
    }

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
        Vector3 result = a1;

        result.Scale(b1);

        return result;
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

        Vector3 result = Vector3.Lerp(a1, b1, t);

        transform.position = result;

        if (t >= 1.0f)
        {
            elapsedTime = 0.0f;
        }

        return result;
    }

    public Vector3 Exercise6()
    {
        return Vector3.Max(a1, b1);
    }

    public Vector3 Exercise7()
    {
        return Vector3.Project(a1, b1);
    }

    public Vector3 Exercise8()
    {
        t = 0.5f;

        Vector3 result = Vector3.Lerp(a1, b1, t);
        result.Normalize();

        distance_A1B1 = Vector3.Distance(a1, b1);

        Vector3 scaledResult = result * distance_A1B1;
        resultMagnitude = scaledResult.magnitude;

        return scaledResult;
    }

    public Vector3 Exercise9()
    {
        angle_A1_hisResult = Vector3.Angle(hisResult, a1);
        angle_B1_hisResult = Vector3.Angle(hisResult, b1);
        hisResultMagnitude = hisResult.magnitude;
        magnitude_A1 = a1.magnitude;
        magnitude_B1 = b1.magnitude;

        angle_A1B1 = Vector3.Angle(a1, b1);

        resultingAngle = 180 - angle_A1B1;

        Vector3 normalized_a1 = a1.normalized;
        Vector3 normalized_b1 = b1.normalized;

        Vector3 direccionPerpendicular = Vector3.Cross(normalized_a1, normalized_b1);

        Quaternion rotation = Quaternion.AngleAxis(180 - resultingAngle * 2, direccionPerpendicular);

        Vector3 result = rotation * normalized_a1;

        result *= a1.magnitude;

        lastAngle_a1 = Vector3.Angle(result, a1);
        lastAngle_b1 = Vector3.Angle(result, b1);
        return result;
    }

    public Vector3 Exercise10()
    {
        duration = 10;

        Vector3 result;

        if (elapsedTime < duration)
        {          
            elapsedTime += Time.deltaTime;

            t = elapsedTime;

            result = Vector3.LerpUnclamped(b1, a1, t);

            transform.position = result;  
            
            return result;
        }
        else
        {
            elapsedTime = 0.0f;
           
            return Vector3.zero;
        }
    }
}
