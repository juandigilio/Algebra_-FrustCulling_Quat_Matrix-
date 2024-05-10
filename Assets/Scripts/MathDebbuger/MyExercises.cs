using MathDebbuger;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomMath;

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

    //Uso Vector3 solo para el serializedField y luego lo paso a Vec3
    [SerializeField] private Vector3 A;
    [SerializeField] private Vector3 B;
    [SerializeField] private Vector3 C;

    [SerializeField] private Vec3 hisResult;
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

    private Vec3 a;
    private Vec3 b;
    private Vec3 c;



    [SerializeField] private float duration = 1.0f;
    private float elapsedTime = 0.0f;

    private List<string> vectorId = new List<string>();

    private void Start()
    {
        vectorId.Add("Vector 1");
        vectorId.Add("Vector 2");
        vectorId.Add("Vector 3");

        Vector3Debugger.AddVector(a, Color.white, vectorId[0]);
        Vector3Debugger.AddVector(b, Color.black, vectorId[1]);
        Vector3Debugger.AddVector(c, Color.red, vectorId[2]);

        A.x = 1;
        A.y = 2;
        A.z = 1;

        B.x = 4;
        B.y = 3;
        B.z = 2;
    }

    private void Update()
    {
        GetInspectorData();

        switch ((int)exercise)
        {
            case 1:
                {
                    c = Exercise1();
                    break;
                }
            case 2:
                {
                    c = Exercise2();
                    break;
                }
            case 3:
                {
                    c = Exercise3();
                    break;
                }
            case 4:
                {
                    c = Exercise4();
                    break;
                }
            case 5:
                {
                    c = Exercise5();
                    break;
                }
            case 6:
                {
                    c = Exercise6();
                    break;
                }
            case 7:
                {
                    c = Exercise7();
                    break;
                }
            case 8:
                {
                    c = Exercise8();
                    break;
                }
            case 9:
                {
                    c = Exercise9();
                    break;
                }
            case 10:
                {
                    c = Exercise10();
                    break;
                }
        }

        Vector3Debugger.UpdatePosition(vectorId[0], a);
        Vector3Debugger.UpdatePosition(vectorId[1], b);
        Vector3Debugger.UpdatePosition(vectorId[2], c);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + a);

        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + b);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + c);
    }

    public void GetInspectorData()
    {
        a = new Vec3(A);
        b = new Vec3(B);
        c = new Vec3(C);
        
    }

    public Vec3 Exercise1()
    {
        return a + b;
    }

    public Vec3 Exercise2()
    {
        return b - a;
    }

    //x*x y*y z*z
    public Vec3 Exercise3()
    {
        Vec3 result = a;

        result.Scale(b);

        return result;
    }

    //producto cruz
    public Vec3 Exercise4()
    {
        return Vec3.Cross(b, a);
    }

    public Vec3 Exercise5()
    {
        elapsedTime += Time.deltaTime;

        float t = Mathf.Clamp01(elapsedTime / duration);

        Vec3 result = Vec3.Lerp(a, b, t);

        transform.position = result;

        if (t >= 1.0f)
        {
            elapsedTime = 0.0f;
        }

        return result;
    }

    public Vec3 Exercise6()
    {
        return Vec3.Max(a, b);
    }

    public Vec3 Exercise7()
    {
        return Vec3.Project(a, b);
    }

    public Vec3 Exercise8()
    {
        t = 0.5f;

        Vec3 result = Vec3.Lerp(a, b, t);
        result.Normalize();

        distance_A1B1 = Vec3.Distance(a, b);

        Vec3 scaledResult = result * distance_A1B1;
        resultMagnitude = scaledResult.magnitude;

        return scaledResult;
    }

    public Vec3 Exercise9()
    {
        angle_A1_hisResult = Vec3.Angle(hisResult, a);
        angle_B1_hisResult = Vec3.Angle(hisResult, b);
        hisResultMagnitude = hisResult.magnitude;
        magnitude_A1 = a.magnitude;
        magnitude_B1 = b.magnitude;

        angle_A1B1 = Vec3.Angle(a, b);

        resultingAngle = 180 - angle_A1B1;

        Vec3 normalized_a1 = a.normalized;
        Vec3 normalized_b1 = b.normalized;

        Vec3 direccionPerpendicular = Vec3.Cross(normalized_a1, normalized_b1);

        Quaternion rotation = Quaternion.AngleAxis(180 - resultingAngle * 2, direccionPerpendicular);

        Vec3 result = rotation * normalized_a1;

        result *= a.magnitude;

        lastAngle_a1 = Vec3.Angle(result, a);
        lastAngle_b1 = Vec3.Angle(result, b);
        return result;
    }

    public Vec3 Exercise10()
    {
        duration = 10;

        Vec3 result;

        if (elapsedTime < duration)
        {          
            elapsedTime += Time.deltaTime;

            t = elapsedTime;

            result = Vec3.LerpUnclamped(b, a, t);

            transform.position = result;  
            
            return result;
        }
        else
        {
            elapsedTime = 0.0f;
           
            return Vec3.Zero;
        }
    }
}
