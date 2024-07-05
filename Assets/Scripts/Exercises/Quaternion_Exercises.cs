using MathDebbuger;
using UnityEngine;
using CustomMath;
using System.Numerics;
using System.Collections.Generic;
using Quaternion = UnityEngine.Quaternion;

public enum Quat_Exercices
{
    Uno = 1,
    Dos = 2,
    Tres = 3,
}

public class Quaternion_Exercises : MonoBehaviour
{
    [SerializeField] private Quat_Exercices exercise;

    private Vec3 A = new Vec3();
    private Vec3 B = new Vec3();
    private Vec3 C = new Vec3();
    private Vec3 D = new Vec3();


    [SerializeField] private float angle;

    private List<string> vectorId = new List<string>();


    void Start()
    {
        vectorId.Add("Vec A");
        vectorId.Add("Vec B");
        vectorId.Add("Vec C");
        vectorId.Add("Vec D");

        Vector3Debugger.AddVector(A, Color.black, vectorId[0]);
        Vector3Debugger.AddVector(B, Color.black, vectorId[1]);
        Vector3Debugger.AddVector(B, C, Color.black, vectorId[2]);
        Vector3Debugger.AddVector(C, D, Color.black, vectorId[3]);

        SetInitValues();
    }

    void FixedUpdate()
    {
        UpdateExercises();
    }

    private void Exercise1()
    {
        TurnOfVectors();
        Vector3Debugger.TurnOnVector(vectorId[0]);

        My_Quaternion rotation = My_Quaternion.Euler(0, angle, 0);
        A = rotation * A;

        Vector3Debugger.UpdatePosition(vectorId[0], A);
    }

    private void Exercise2()
    {
        TurnOfVectors();
        Vector3Debugger.TurnOnVector(vectorId[0]);
        Vector3Debugger.TurnOnVector(vectorId[1]);
        Vector3Debugger.TurnOnVector(vectorId[2]);

        My_Quaternion rotation = My_Quaternion.Euler(0, angle, 0);
        A = rotation * A;
        B = rotation * B;
        C = rotation * C;

        Vector3Debugger.UpdatePosition(vectorId[0], A);
        Vector3Debugger.UpdatePosition(vectorId[1], A, B);
        Vector3Debugger.UpdatePosition(vectorId[2], B, C);
    }

    private void Exercise3()
    {
        TurnOfVectors();
        Vector3Debugger.TurnOnVector(vectorId[0]);
        Vector3Debugger.TurnOnVector(vectorId[1]);
        Vector3Debugger.TurnOnVector(vectorId[2]);
        Vector3Debugger.TurnOnVector(vectorId[3]);

        Vec3 newAxis = D - B;
        My_Quaternion rotation = My_Quaternion.AngleAxis(angle, newAxis);
        A = rotation * A;

        rotation = My_Quaternion.AngleAxis(angle, newAxis);
        C = rotation * C;

        Vector3Debugger.UpdatePosition(vectorId[0], A);
        Vector3Debugger.UpdatePosition(vectorId[1], A, B);
        Vector3Debugger.UpdatePosition(vectorId[2], B, C);
        Vector3Debugger.UpdatePosition(vectorId[3], C, D);
    }

    private void UpdateExercises()
    {
        switch ((int)exercise)
        {
            case 1:
                {
                    Exercise1();
                    break;
                }
            case 2:
                {
                    Exercise2();
                    break;
                }
            case 3:
                {
                    Exercise3();
                    break;
                }
        }

        
    }

    private void SetInitValues()
    {
        A = Vec3.Zero;
        B = Vec3.Zero;
        C = Vec3.Zero;
        D = Vec3.Zero;

        A.x = 10;
        B.x = 10;
        B.y = 10;
        C.x = 20;
        C.y = 10;
        D.x = 20;
        D.y = 20;

    }

    private void TurnOfVectors()
    {
        foreach (string id in vectorId)
        {
            Vector3Debugger.TurnOffVector(id);
        }
    }
}
