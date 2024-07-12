using System;
using UnityEngine;
using CustomMath;

public struct My_Quaternion
{
    private const float Epsilon = 1e-6f;

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
        //tabla de multiplicacion de quat

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

    public static Vec3 operator *(My_Quaternion rotation, Vec3 point)
    {
        My_Quaternion pureVectorQuaternion = new My_Quaternion(point, 0);
        //cnjugated para transformar correctamente el quaternion de vuelta al espacio original
        My_Quaternion appliedPureQuaternion = rotation * pureVectorQuaternion * Conjugated(rotation);

        return new Vec3(appliedPureQuaternion.x, appliedPureQuaternion.y, appliedPureQuaternion.z);
    }

    public static My_Quaternion operator /(My_Quaternion value1, My_Quaternion value2)
    {
        // Calcula el conjugado de value2
        My_Quaternion conjugate = Conjugated(value2);

        // Divide value1 por el cuadrado de la magnitud de value2
        My_Quaternion result = value1 * conjugate / value2.SquaredMagnitude();

        return result;
    }

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

    public My_Quaternion Normalized
    {
        get
        {
            My_Quaternion result = this;
            result.Normalize();
            return result;
        }
    }

    public Vec3 EulerAngles
    {
        get
        {
            //devuelvo un vec3 con los angulos de rotacion en cada eje
            float sinrCosp = 2 * (w * x + y * z);
            float cosrCosp = 1 - 2 * (x * x + y * y);
            float angleX = Mathf.Atan2(sinrCosp, cosrCosp);

            float sinp = Mathf.Sqrt(1 + 2 * (w * y - x * z));
            float cosp = Mathf.Sqrt(1 - 2 * (w * y - x * z));
            float angleY = 2 * Mathf.Atan2(sinp, cosp) - Mathf.Rad2Deg;

            float sinyCosp = 2 * (w * z + x * y);
            float cosyCosp = 1 - 2 * (y * y + z * z);
            float angleZ = Mathf.Atan2(sinyCosp, cosyCosp);

            return new Vector3(angleX, angleY, angleZ);
        }

        set
        {
            My_Quaternion q = Euler(value);
            Set(q.x, q.y, q.z, q.w);
        }
    }

    public void Set(float newX, float newY, float newZ, float newW)
    {
        x = newX;
        y = newY;
        z = newZ;
        w = newW;
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

    public static bool IsEqualUsingDot(float dotValue)
    {
        return dotValue > 1 - float.Epsilon && dotValue < 1 + float.Epsilon;
    }

    public float SquaredMagnitude()
    {
        return x * x + y * y + z * z + w * w;
    }

    public float Magnitude()
    {
        return Mathf.Sqrt(SquaredMagnitude());
    }
    
    public void Normalize()
    {
        //La magnitud es igual a norma  (magntitud de q = r^2(x^2 + y^2 + z^2 + w^2))
        float magnitude = Magnitude();

        if (magnitude < Epsilon)
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

    public float NormalizeAngle(float angle)
    {
        float normalizedAngle = angle;

        while (normalizedAngle > 360.0f)
        {
            normalizedAngle -= 360.0f;
        }

        while (normalizedAngle < 0.0f)
        {
            normalizedAngle += 360.0f;
        }

        return normalizedAngle;
    }

    public Vec3 NormalizeAngles(Vec3 angles)
    {
        Vec3 result = new Vec3();

        result.x = NormalizeAngle(angles.x);
        result.y = NormalizeAngle(angles.y);
        result.z =NormalizeAngle(angles.z);

        return result;
    }

    public static My_Quaternion Inverse(My_Quaternion q)
    {
        return Conjugated(q) / q.SquaredMagnitude();
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

    public static float Angle(My_Quaternion q1, My_Quaternion q2)
    {
        float dotValue = Dot(q1.Normalized, q2.Normalized);

        if (IsEqualUsingDot(dotValue))
        {
            return 0f;
        }

        float absValue = Mathf.Abs(dotValue);

        return Mathf.Acos(absValue) * 2.0f * Mathf.Rad2Deg;
        //acos es la inversa al coseno, me da el angulo para el coseno requerido (me da el angulo en radianes)
        //El producto punto de dos quaternions normalizados es el coseno de la mitad del ángulo entre ellos
    }

    public static My_Quaternion AngleAxis(float angle, Vec3 axis)
    {
        // = cos angle / 2 , axis normalizado * sen angle / 2

        Vec3 vectorialPart = axis.normalized;
        float inRadians = Mathf.Deg2Rad * angle;

        vectorialPart *= Mathf.Sin(inRadians * 0.5f);
        float scalarPart = Mathf.Cos(inRadians * 0.5f);

        return new My_Quaternion(vectorialPart, scalarPart);
    }

    public static My_Quaternion FromToRotation(Vec3 fromDirection, Vec3 toDirection)
    {
        //calculo el eje de rotacion
        Vec3 axis = Vec3.Cross(fromDirection, toDirection);
        //calculo el angulo
        float angle = Vec3.Angle(fromDirection, toDirection);
        //ya tengo la parte vectorial y la parte escalar
        return AngleAxis(angle, axis);
    }

    public void SetFromToRotation(Vec3 fromDirection, Vec3 toDirection)
    {
        My_Quaternion result = FromToRotation(fromDirection, toDirection).Normalized;

        x = result.x;
        y = result.y;
        z = result.z;
        w = result.w;
    }

    public static My_Quaternion LerpUnclamped(My_Quaternion q1, My_Quaternion q2, float t)
    {
        My_Quaternion result = Identity;

        float timeLeft = 1 - t;

        if (Dot(q1, q2) >= 0)
        {
            result.x = (timeLeft * q1.x) + (t * q2.x);
            result.y = (timeLeft * q1.y) + (t * q2.y);
            result.z = (timeLeft * q1.z) + (t * q2.z);
            result.w = (timeLeft * q1.w) + (t * q2.w);
        }
        else
        {
            result.x = (timeLeft * q1.x) - (t * q2.x);
            result.y = (timeLeft * q1.y) - (t * q2.y);
            result.z = (timeLeft * q1.z) - (t * q2.z);
            result.w = (timeLeft * q1.w) - (t * q2.w);
        }
        return result.Normalized;
    }

    public static My_Quaternion Lerp(My_Quaternion q1, My_Quaternion q2, float t)
    {
        if (t < 0)
        {
            t = 0;
        }
        else if (t > 1)
        {
            t = 1;
        }

        return LerpUnclamped(q1, q2, t);
    }

    public static My_Quaternion SlerpUnclamped(My_Quaternion q1, My_Quaternion q2, float t)
    {
        My_Quaternion normalizedQ1 = q1.Normalized;
        My_Quaternion normalizedQ2 = q2.Normalized;

        float cosOmega = Dot(normalizedQ1, normalizedQ2);

        //saco el abs porque quiero el valor del anuglo no me importa la direccion
        cosOmega = Mathf.Abs(cosOmega);


        float coeff1;
        float coeff2;

        //hago Acos para pasar del coseno omega a omega que es el angulo de ese coseno
        float omega = Mathf.Acos(cosOmega);

        //                                      lo divido por el seno de omega para normalizarlo
        coeff1 = Mathf.Sin((1 - t) * omega) / Mathf.Sin(omega);

        //          busco el camino mas corto
        coeff2 = (cosOmega < 0.0f ? -1 : 1) * (Mathf.Sin(t * omega) / Mathf.Sin(omega));


        //multiplico cada quat por la incidencia que tiene
        My_Quaternion result = new My_Quaternion();
        result.x = coeff1 * normalizedQ1.x + coeff2 * normalizedQ2.x;
        result.y = coeff1 * normalizedQ1.y + coeff2 * normalizedQ2.y;
        result.z = coeff1 * normalizedQ1.z + coeff2 * normalizedQ2.z;
        result.w = coeff1 * normalizedQ1.w + coeff2 * normalizedQ2.w;

        return result;
    }

    public static My_Quaternion Slerp(My_Quaternion q1, My_Quaternion q2, float t)
    {
        if (t < 0)
        {
            t = 0;
        }
        else if (t > 1)
        {
            t = 1;
        }

        return SlerpUnclamped(q1, q2, t);
    }

    public static My_Quaternion RotateTowards(My_Quaternion from, My_Quaternion to, float maxDegreesDelta)
    {
        float angle = Angle(from, to);

        if (angle == 0.0f)
        {
            return to;
        }

        return SlerpUnclamped(from, to, Mathf.Min(1.0f, maxDegreesDelta / angle));
    }

    public static My_Quaternion LookRotation(Vec3 forward, Vec3 upwards)
    {
        Vec3 tempForward = forward.normalized;
        Vec3 tempRight = Vec3.Cross(upwards, forward).normalized;
        Vec3 tempUp = upwards.normalized;

        //asigno los vectores a una matriz 3x3
        float m00 = tempRight.x;
        float m01 = tempRight.y;
        float m02 = tempRight.z;

        float m10 = tempUp.x;
        float m11 = tempUp.y;
        float m12 = tempUp.z;

        float m20 = tempForward.x;
        float m21 = tempForward.y;
        float m22 = tempForward.z;

        Vector4 column0 = new Vector4(m00, m10, m20, 0);
        Vector4 column1 = new Vector4(m01, m11, m21, 0);
        Vector4 column2 = new Vector4(m02, m12, m22, 0);
        Vector4 column3 = Vector4.zero;

        My_Matrix4x4 result = new My_Matrix4x4(column0, column1, column2, column3);

        return result.Rotation;
        //My_Quaternion result;
        //float factor;

        //if (m22 < 0)
        //{
        //    if (m00 > m11)
        //    {
        //        factor = 1 + m00 - m11 - m22;

        //        result = new My_Quaternion(factor, m10 + m01, m20 + m02, m12 - m21);
        //    }
        //    else
        //    {
        //        factor = 1 - m00 + m11 - m22;

        //        result = new My_Quaternion(m01 + m10, factor, m12 + m21, m20 - m02);
        //    }
        //}
        //else
        //{
        //    if (m00 < -m11)
        //    {
        //        factor = 1 - m00 - m11 + m22;

        //        result = new My_Quaternion(m20 + m02, m12 + m21, factor, m01 - m10);
        //    }
        //    else
        //    {
        //        factor = 1 + m00 + m11 + m22;

        //        result = new My_Quaternion(m12 - m21, m20 - m02, m01 - m10, factor);
        //    }
        //}

        //result *= 0.5f / Mathf.Sqrt(factor);

        //return result;
    }

    public static My_Quaternion LookRotation(Vec3 forward)
    {
        return LookRotation(forward, Vec3.Up);
    }

    public void SetLookRotation(Vec3 view, Vec3 up)
    {
        My_Quaternion lookRotationQuaternion = LookRotation(view, up);

        x = lookRotationQuaternion.x;
        y = lookRotationQuaternion.y;
        z = lookRotationQuaternion.z;
    }

    public void SetLookRotation(Vec3 view)
    {
        Vec3 up = Vec3.Up;
        SetLookRotation(view, up);
    }

    public static My_Quaternion Euler(float x, float y, float z)
    {
        float sin;
        float cos;

        My_Quaternion qX;
        My_Quaternion qY;
        My_Quaternion qZ;

        float xRadians = Mathf.Deg2Rad * x;
        float yRadians = Mathf.Deg2Rad * y;
        float zRadians = Mathf.Deg2Rad * z;

        My_Quaternion result = Identity;

        //creo el quat para cada eje con su parte real y su parte imaginaria
        sin = Mathf.Sin(xRadians * 0.5f);
        cos = Mathf.Cos(xRadians * 0.5f);
        qX = new My_Quaternion(sin, 0, 0, cos);

        sin = Mathf.Sin(yRadians * 0.5f);
        cos = Mathf.Cos(yRadians * 0.5f);
        qY = new My_Quaternion(0, sin, 0, cos);

        sin = Mathf.Sin(zRadians * 0.5f);
        cos = Mathf.Cos(zRadians * 0.5f);
        qZ = new My_Quaternion(0, 0, sin, cos);

        result = (qX * qY) * qZ;

        return result;
    }

    public static My_Quaternion Euler(Vec3 euler)
    {
        return Euler(euler.x, euler.y, euler.z);
    }
}