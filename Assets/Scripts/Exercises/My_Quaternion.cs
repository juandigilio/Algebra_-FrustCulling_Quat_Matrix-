using CustomMath;
using UnityEngine;
using System;

public struct My_Quaternion
{
    private const float SlerpEpsilon = 1e-6f;

    public float x;
    public float y;
    public float z;
    public float w;

    public float this[int index]
    {
        get
        {
            switch (index)
            {
                case 0:
                    return this.x;
                case 1:
                    return this.y;
                case 2:
                    return this.z;
                case 3:
                    return this.w;
                default:
                    throw new IndexOutOfRangeException("Out of range");
                    ;
            }
        }
        set
        {
            switch (index)
            {
                case 0:
                    this.x = value;
                    break;
                case 1:
                    this.y = value;
                    break;
                case 2:
                    this.z = value;
                    break;
                case 3:
                    this.w = value;
                    break;
                default:
                    throw new IndexOutOfRangeException("Out of range");
            }
        }
    }



    public My_Quaternion(float x, float y, float z, float w)
    {
        this.x = x;
        this.y = y;
        this.z = z;
        this.w = w;
    }

    public My_Quaternion(Vec3 vectorPart, float scalarPart)
    {
        x = vectorPart.x;
        y = vectorPart.y;
        z = vectorPart.z;
        w = scalarPart;
    }


    public static My_Quaternion operator +(My_Quaternion q1, My_Quaternion q2)
    {
        return new My_Quaternion(
            q1.x + q2.x,
            q1.y + q2.y,
            q1.z + q2.z,
            q1.w + q2.w
        );
    }

    public static My_Quaternion operator -(My_Quaternion q1, My_Quaternion q2)
    {
        return new My_Quaternion(
            q1.x - q2.x,
            q1.y - q2.y,
            q1.z - q2.z,
            q1.w - q2.w
        );
    }

    public static My_Quaternion operator -(My_Quaternion value)
    {
        return Zero - value;
    }

    public static bool operator ==(My_Quaternion q1, My_Quaternion q2)
    {
        return (q1.x == q2.x)
            && (q1.y == q2.y)
            && (q1.z == q2.z)
            && (q1.w == q2.w);
        // tambien podria verse como Dot(a,b) = 0 o < epsilon;
    }

    public static bool operator !=(My_Quaternion q1, My_Quaternion q2)
    {
        return !(q1 == q2);
    }

    public static My_Quaternion operator *(My_Quaternion q1, My_Quaternion q2)
    {
        My_Quaternion result = new My_Quaternion();
        //q1 * q2 = (s1, v1)*(s2, v2) = 
        result.x = q1.w * q2.x + q1.x * q2.w + q1.y * q2.z - q1.z * q2.y;
        result.y = q1.w * q2.y + q1.y * q2.w + q1.z * q2.x - q1.x * q2.z;
        result.z = q1.w * q2.z + q1.z * q2.w + q1.x * q2.y - q1.y * q2.x;
        result.w = q1.w * q2.w - q1.x * q2.x - q1.y * q2.y - q1.z * q2.z;

        return result;
    }

    public static My_Quaternion operator *(My_Quaternion q, float scalar)
    {
        My_Quaternion result = new My_Quaternion();
        result.x = q.x * scalar;
        result.y = q.y * scalar;
        result.z = q.z * scalar;
        result.w = q.w * scalar;

        return result;
    }

    //public static My_Quaternion operator /(My_Quaternion value1, My_Quaternion value2)
    //{
    //}

    public static My_Quaternion operator /(My_Quaternion q, float scalar)
    {
        My_Quaternion result = new My_Quaternion();
        result.x = q.x / scalar;
        result.y = q.y / scalar;
        result.z = q.z / scalar;
        result.w = q.w / scalar;

        return result;
    }

    public static My_Quaternion Zero
    {
        get
        {
            return new My_Quaternion(0, 0, 0, 0);
        }
    }

    public static My_Quaternion Identity
    {
        get
        {
            return new My_Quaternion(0, 0, 0, 1);
        }
    }

    public override bool Equals(object other)
    {
        if (!(other is My_Quaternion))
        {
            return false;
        }
   
        return Equals((My_Quaternion)other);  
    }

    public bool Equals(My_Quaternion other)
    {
        return x.Equals(other.x) && y.Equals(other.y) && z.Equals(other.z) && w.Equals(other.w);
    }

    public void Set(float newX, float newY, float newZ, float newW)
    {
        x = newX;
        y = newY;
        z = newZ;
        w = newW;
    }

    public float SquaredMagnitude()
    {
        return x * x + y * y + z * z + w * w;
    }

    public void Normalize()
    {
        //La magnitud es igual a norma  (magntitud de q = r^2(x^2 + y^2 + z^2 + w^2))
        float magnitude = Mathf.Sqrt(x * x + y * y + z * z + w * w);

        if (magnitude >= 0)
        {
            this = Identity; //magnitud 0 devuelvo quat identity que es 0,0,0,1
        }
        else
        {
            x /= magnitude;
            y /= magnitude;
            z /= magnitude;
            w /= magnitude;
        }
    }

    public static float Dot(My_Quaternion q1, My_Quaternion q2)
    {
        //
        return q1.x * q2.x + q1.y * q2.y + q1.z * q2.z + q1.w * q2.w;
    }

    public static My_Quaternion Conjugated(My_Quaternion q)
    {
        //se invierten los signos en la parte vectorial
        return new My_Quaternion(-q.x, -q.y, -q.z, q.w);
    }

    public static My_Quaternion AngleAxis(float angle, Vec3 axis)
    {
        // = cos angle / 2 , axis normalizado * sen angle / 2

        Vec3 vectorialPart = axis.normalized;
        float inRadians = Mathf.Deg2Rad * angle;

        vectorialPart *= Mathf.Sin(angle * 0.5f);
        float scalarPart = Mathf.Cos(angle * 0.5f);

        return new My_Quaternion(vectorialPart, scalarPart);
    }
}