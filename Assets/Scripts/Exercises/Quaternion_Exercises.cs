using MathDebbuger;
using UnityEngine;
using CustomMath;
using System.Collections.Generic;


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

    private int lastUsed;


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

        lastUsed = (int)exercise;
    }

    void FixedUpdate()
    {
        UpdateExercises();
    }

    private void Exercise1()
    {
        TurnOfVectors();
        TurnOnVectors(1);

        My_Quaternion rotation = My_Quaternion.AngleAxis(angle, Vec3.Up);
        A = rotation * A;
    }

    private void Exercise2()
    {
        TurnOfVectors();
        TurnOnVectors(3);

        My_Quaternion rotation = My_Quaternion.AngleAxis(angle, Vec3.Up);
        A = rotation * A;
        B = rotation * B;
        C = rotation * C;
    }

    private void Exercise3()
    {
        TurnOfVectors();
        TurnOnVectors(4);

        My_Quaternion rotation = My_Quaternion.AngleAxis(angle, D);
        A = rotation * A;

        rotation = My_Quaternion.Inverse(rotation);
        //rotation = My_Quaternion.AngleAxis(angle, D);
        C = rotation * C;
    }

    private void UpdateExercises()
    {
        if (lastUsed != (int)exercise)
        {
            SetInitValues();
            lastUsed = (int)exercise;
        }

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

        UpdateVectors();
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

    private void TurnOnVectors(int qnty)
    {
        for (int i = 0; i < qnty; i ++)
        {
            Vector3Debugger.TurnOnVector(vectorId[i]);
        }
    }

    private void UpdateVectors()
    {
        Vector3Debugger.UpdatePosition(vectorId[0], A);
        Vector3Debugger.UpdatePosition(vectorId[1], A, B);
        Vector3Debugger.UpdatePosition(vectorId[2], B, C);
        Vector3Debugger.UpdatePosition(vectorId[3], C, D);
    }

    private void TurnOfVectors()
    {
        foreach (string id in vectorId)
        {
            Vector3Debugger.TurnOffVector(id);
        }
    }
}
