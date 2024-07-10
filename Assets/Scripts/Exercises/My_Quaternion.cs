using CustomMath;
using UnityEngine;
using System;

public struct My_Quaternion
{
    private const float SlerpEpsilon = 1e-6f;

    public float X;
    public float Y;
    public float Z;
    public float W;

    internal const int Count = 4;

    public My_Quaternion(float x, float y, float z, float w)
    {
        X = x;
        Y = y;
        Z = z;
        W = w;
    }

    public My_Quaternion(Vec3 vectorPart, float scalarPart)
    {
        X = vectorPart.x;
        Y = vectorPart.y;
        Z = vectorPart.z;
        W = scalarPart;
    }


    public static My_Quaternion operator +(My_Quaternion value1, My_Quaternion value2)
    {
        return new My_Quaternion(
            value1.X + value2.X,
            value1.Y + value2.Y,
            value1.Z + value2.Z,
            value1.W + value2.W
        );
    }

    public static My_Quaternion operator -(My_Quaternion value1, My_Quaternion value2)
    {
        return new My_Quaternion(
            value1.X - value2.X,
            value1.Y - value2.Y,
            value1.Z - value2.Z,
            value1.W - value2.W
        );
    }

    public static My_Quaternion operator -(My_Quaternion value)
    {
        return Zero - value;
    }

    public static bool operator ==(My_Quaternion value1, My_Quaternion value2)
    {
        return (value1.X == value2.X)
            && (value1.Y == value2.Y)
            && (value1.Z == value2.Z)
            && (value1.W == value2.W);
    }

    public static bool operator !=(My_Quaternion value1, My_Quaternion value2)
    {
        return !(value1 == value2);
    }

    public static My_Quaternion operator *(My_Quaternion value1, My_Quaternion value2)
    {
  
    }

    public static My_Quaternion operator /(My_Quaternion value1, My_Quaternion value2)
    {
    }

    public static My_Quaternion operator *(My_Quaternion value1, float value2)
    {
        return new My_Quaternion(
            value1.X * value2,
            value1.Y * value2,
            value1.Z * value2,
            value1.W * value2
        );
    }

    






}
