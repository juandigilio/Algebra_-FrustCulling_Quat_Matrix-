using System;
using UnityEngine;

public class My_Matrix4x4 : IEquatable<My_Matrix4x4>, IFormattable
{
    public float m00, m01, m02, m03;
    public float m10, m11, m12, m13;
    public float m20, m21, m22, m23;
    public float m30, m31, m32, m33;

    public My_Matrix4x4(Vector4 column0, Vector4 column1, Vector4 column2, Vector4 column3)
    {
        m00 = column0.x; m01 = column1.x; m02 = column2.x; m03 = column3.x;
        m10 = column0.y; m11 = column1.y; m12 = column2.y; m13 = column3.y;
        m20 = column0.z; m21 = column1.z; m22 = column2.z; m23 = column3.z;
        m30 = column0.w; m31 = column1.w; m32 = column2.w; m33 = column3.w;
    }

    public float this[int index]
    {
        get
        {
            switch (index)
            {
                case 0: return m00;
                case 1: return m10;
                case 2: return m20;
                case 3: return m30;
                case 4: return m01;
                case 5: return m11;
                case 6: return m21;
                case 7: return m31;
                case 8: return m02;
                case 9: return m12;
                case 10: return m22;
                case 11: return m32;
                case 12: return m03;
                case 13: return m13;
                case 14: return m23;
                case 15: return m33;
                default: throw new IndexOutOfRangeException("Invalid matrix index!");
            }
        }
        set
        {
            switch (index)
            {
                case 0: m00 = value; break;
                case 1: m10 = value; break;
                case 2: m20 = value; break;
                case 3: m30 = value; break;
                case 4: m01 = value; break;
                case 5: m11 = value; break;
                case 6: m21 = value; break;
                case 7: m31 = value; break;
                case 8: m02 = value; break;
                case 9: m12 = value; break;
                case 10: m22 = value; break;
                case 11: m32 = value; break;
                case 12: m03 = value; break;
                case 13: m13 = value; break;
                case 14: m23 = value; break;
                case 15: m33 = value; break;
                default: throw new IndexOutOfRangeException("Invalid matrix index!");
            }
        }
    }

    public float this[int row, int column]
    {
        get { return this[row + column * 4]; }
        set { this[row + column * 4] = value; }
    }

    public static My_Matrix4x4 zero => new My_Matrix4x4(Vector4.zero, Vector4.zero, Vector4.zero, Vector4.zero);
    public static My_Matrix4x4 identity => new My_Matrix4x4(new Vector4(1, 0, 0, 0), new Vector4(0, 1, 0, 0), new Vector4(0, 0, 1, 0), new Vector4(0, 0, 0, 1));

    public Quaternion rotation => Quaternion.LookRotation(GetColumn(2), GetColumn(1));
    public Vector3 lossyScale => new Vector3(GetColumn(0).magnitude, GetColumn(1).magnitude, GetColumn(2).magnitude);
    public bool isIdentity => this == identity;
    public float determinant => Determinant(this);
    public My_Matrix4x4 transpose => Transpose(this);
    public My_Matrix4x4 inverse => Inverse(this);


    public static My_Matrix4x4 operator *(My_Matrix4x4 matrix, float scalar)
    {
        Vector4 column_0= new Vector4(matrix.m00 * scalar, matrix.m10 * scalar, matrix.m20 * scalar, matrix.m30 * scalar);
        Vector4 column_1 = new Vector4(matrix.m01 * scalar, matrix.m11 * scalar, matrix.m21 * scalar, matrix.m31 * scalar);
        Vector4 column_2 = new Vector4(matrix.m02 * scalar, matrix.m12 * scalar, matrix.m22 * scalar, matrix.m32 * scalar);
        Vector4 column_3 = new Vector4(matrix.m03 * scalar, matrix.m13 * scalar, matrix.m23 * scalar, matrix.m33 * scalar);

        return new My_Matrix4x4(column_0, column_1, column_2, column_3);
    }

    public static float Determinant(My_Matrix4x4 m)
    {
        return m.m00 * m.m11 * m.m22 * m.m33 + m.m01 * m.m12 * m.m23 * m.m30 + m.m02 * m.m13 * m.m20 * m.m31 +
               m.m03 * m.m10 * m.m21 * m.m32 - m.m03 * m.m12 * m.m21 * m.m30 - m.m02 * m.m11 * m.m20 * m.m33 -
               m.m01 * m.m10 * m.m23 * m.m32 - m.m00 * m.m13 * m.m22 * m.m31;
    }

    public static My_Matrix4x4 Frustum(float left, float right, float bottom, float top, float zNear, float zFar)
    {
        My_Matrix4x4 result = zero;
        result.m00 = 2.0F * zNear / (right - left);
        result.m11 = 2.0F * zNear / (top - bottom);
        result.m02 = (right + left) / (right - left);
        result.m12 = (top + bottom) / (top - bottom);
        result.m22 = -(zFar + zNear) / (zFar - zNear);
        result.m32 = -1.0F;
        result.m23 = -(2.0F * zFar * zNear) / (zFar - zNear);
        return result;
    }

    public static My_Matrix4x4 Frustum(FrustumPlanes fp)
    {
        return Frustum(fp.left, fp.right, fp.bottom, fp.top, fp.zNear, fp.zFar);
    }

    public static My_Matrix4x4 Inverse(My_Matrix4x4 m)
    {
        float determinant = Determinant(m);
        if (Mathf.Abs(determinant) < Mathf.Epsilon)
            throw new InvalidOperationException("Matrix is not invertible.");

        My_Matrix4x4 result = zero;
        // Calculate the inverse using the matrix's adjugate and determinant.
        // This is a simplified version for demonstration purposes.
        // You may need a more efficient or numerically stable algorithm for a production environment.
        result.m00 = m.m11 * m.m22 * m.m33 - m.m11 * m.m23 * m.m32 - m.m21 * m.m12 * m.m33 + m.m21 * m.m13 * m.m32 + m.m31 * m.m12 * m.m23 - m.m31 * m.m13 * m.m22;
        result.m01 = -m.m01 * m.m22 * m.m33 + m.m01 * m.m23 * m.m32 + m.m21 * m.m02 * m.m33 - m.m21 * m.m03 * m.m32 - m.m31 * m.m02 * m.m23 + m.m31 * m.m03 * m.m22;
        result.m02 = m.m01 * m.m12 * m.m33 - m.m01 * m.m13 * m.m32 - m.m11 * m.m02 * m.m33 + m.m11 * m.m03 * m.m32 + m.m31 * m.m02 * m.m13 - m.m31 * m.m03 * m.m12;
        result.m03 = -m.m01 * m.m12 * m.m23 + m.m01 * m.m13 * m.m22 + m.m11 * m.m02 * m.m23 - m.m11 * m.m03 * m.m22 - m.m21 * m.m02 * m.m13 + m.m21 * m.m03 * m.m12;
        result.m10 = -m.m10 * m.m22 * m.m33 + m.m10 * m.m23 * m.m32 + m.m20 * m.m12 * m.m33 - m.m20 * m.m13 * m.m32 - m.m30 * m.m12 * m.m23 + m.m30 * m.m13 * m.m22;
        result.m11 = m.m00 * m.m22 * m.m33 - m.m00 * m.m23 * m.m32 - m.m20 * m.m02 * m.m33 + m.m20 * m.m03 * m.m32 + m.m30 * m.m02 * m.m23 - m.m30 * m.m03 * m.m22;
        result.m12 = -m.m00 * m.m12 * m.m33 + m.m00 * m.m13 * m.m32 + m.m10 * m.m02 * m.m33 - m.m10 * m.m03 * m.m32 - m.m30 * m.m02 * m.m13 + m.m30 * m.m03 * m.m12;
        result.m13 = m.m00 * m.m12 * m.m23 - m.m00 * m.m13 * m.m22 - m.m10 * m.m02 * m.m23 + m.m10 * m.m03 * m.m22 + m.m20 * m.m02 * m.m13 - m.m20 * m.m03 * m.m12;
        result.m20 = m.m10 * m.m21 * m.m33 - m.m10 * m.m23 * m.m31 - m.m20 * m.m11 * m.m33 + m.m20 * m.m13 * m.m31 + m.m30 * m.m11 * m.m23 - m.m30 * m.m13 * m.m21;
        result.m21 = -m.m00 * m.m21 * m.m33 + m.m00 * m.m23 * m.m31 + m.m20 * m.m01 * m.m33 - m.m20 * m.m03 * m.m31 - m.m30 * m.m01 * m.m23 + m.m30 * m.m03 * m.m21;
        result.m22 = m.m00 * m.m11 * m.m33 - m.m00 * m.m13 * m.m31 - m.m10 * m.m01 * m.m33 + m.m10 * m.m03 * m.m31 + m.m30 * m.m01 * m.m13 - m.m30 * m.m03 * m.m11;
        result.m23 = -m.m00 * m.m11 * m.m23 + m.m00 * m.m13 * m.m21 + m.m10 * m.m01 * m.m23 - m.m10 * m.m03 * m.m21 - m.m20 * m.m01 * m.m13 + m.m20 * m.m03 * m.m11;
        result.m30 = -m.m10 * m.m21 * m.m32 + m.m10 * m.m22 * m.m31 + m.m20 * m.m11 * m.m32 - m.m20 * m.m12 * m.m31 - m.m30 * m.m11 * m.m22 + m.m30 * m.m12 * m.m21;
        result.m31 = m.m00 * m.m21 * m.m32 - m.m00 * m.m22 * m.m31 - m.m20 * m.m01 * m.m32 + m.m20 * m.m02 * m.m31 + m.m30 * m.m01 * m.m22 - m.m30 * m.m02 * m.m21;
        result.m32 = -m.m00 * m.m11 * m.m32 + m.m00 * m.m12 * m.m31 + m.m10 * m.m01 * m.m32 - m.m10 * m.m02 * m.m31 - m.m30 * m.m01 * m.m12 + m.m30 * m.m02 * m.m11;
        result.m33 = m.m00 * m.m11 * m.m22 - m.m00 * m.m12 * m.m21 - m.m10 * m.m01 * m.m22 + m.m10 * m.m02 * m.m21 + m.m20 * m.m01 * m.m12 - m.m20 * m.m02 * m.m11;
        return result * (1.0F / determinant);
    }

    public static bool Inverse3DAffine(My_Matrix4x4 input, ref My_Matrix4x4 result)
    {
        // Inversión específica para matrices de transformación 3D afines
        Vector3 s = input.lossyScale;
        if (Mathf.Abs(s.x) < Mathf.Epsilon || Mathf.Abs(s.y) < Mathf.Epsilon || Mathf.Abs(s.z) < Mathf.Epsilon)
        {
            result = identity;
            return false;
        }

        Quaternion r = input.rotation;
        Vector3 t = input.GetPosition();

        result = TRS(Vector3.zero, Quaternion.Inverse(r), new Vector3(1 / s.x, 1 / s.y, 1 / s.z));
        result = Translate(-t) * result;
        return true;
    }

    public Vector3 GetPosition()
    {
        return new Vector3(m03, m13, m23);
    }

    public static My_Matrix4x4 LookAt(Vector3 from, Vector3 to, Vector3 up)
    {
        Vector3 zaxis = (from - to).normalized;
        Vector3 xaxis = Vector3.Cross(up, zaxis).normalized;
        Vector3 yaxis = Vector3.Cross(zaxis, xaxis);

        My_Matrix4x4 result = identity;
        result.m00 = xaxis.x; result.m01 = yaxis.x; result.m02 = zaxis.x; result.m03 = from.x;
        result.m10 = xaxis.y; result.m11 = yaxis.y; result.m12 = zaxis.y; result.m13 = from.y;
        result.m20 = xaxis.z; result.m21 = yaxis.z; result.m22 = zaxis.z; result.m23 = from.z;
        return result;
    }

    public static My_Matrix4x4 Ortho(float left, float right, float bottom, float top, float zNear, float zFar)
    {
        My_Matrix4x4 result = identity;
        result.m00 = 2.0F / (right - left);
        result.m11 = 2.0F / (top - bottom);
        result.m22 = -2.0F / (zFar - zNear);
        result.m03 = -(right + left) / (right - left);
        result.m13 = -(top + bottom) / (top - bottom);
        result.m23 = -(zFar + zNear) / (zFar - zNear);
        return result;
    }

    public static My_Matrix4x4 Perspective(float fov, float aspect, float zNear, float zFar)
    {
        float tanHalfFov = Mathf.Tan(0.5F * Mathf.Deg2Rad * fov);
        My_Matrix4x4 result = zero;
        result.m00 = 1.0F / (aspect * tanHalfFov);
        result.m11 = 1.0F / tanHalfFov;
        result.m22 = -(zFar + zNear) / (zFar - zNear);
        result.m32 = -1.0F;
        result.m23 = -(2.0F * zFar * zNear) / (zFar - zNear);
        return result;
    }

    public static My_Matrix4x4 Rotate(Quaternion q)
    {
        float x = q.x * 2.0F;
        float y = q.y * 2.0F;
        float z = q.z * 2.0F;
        float xx = q.x * x;
        float yy = q.y * y;
        float zz = q.z * z;
        float xy = q.x * y;
        float xz = q.x * z;
        float yz = q.y * z;
        float wx = q.w * x;
        float wy = q.w * y;
        float wz = q.w * z;

        My_Matrix4x4 result = identity;
        result.m00 = 1.0F - (yy + zz);
        result.m10 = xy + wz;
        result.m20 = xz - wy;
        result.m01 = xy - wz;
        result.m11 = 1.0F - (xx + zz);
        result.m21 = yz + wx;
        result.m02 = xz + wy;
        result.m12 = yz - wx;
        result.m22 = 1.0F - (xx + yy);
        return result;
    }

    public static My_Matrix4x4 Scale(Vector3 vector)
    {
        My_Matrix4x4 result = zero;
        result.m00 = vector.x;
        result.m11 = vector.y;
        result.m22 = vector.z;
        result.m33 = 1.0F;
        return result;
    }

    public static My_Matrix4x4 Translate(Vector3 vector)
    {
        My_Matrix4x4 result = identity;
        result.m03 = vector.x;
        result.m13 = vector.y;
        result.m23 = vector.z;
        return result;
    }

    public static My_Matrix4x4 Transpose(My_Matrix4x4 m)
    {
        My_Matrix4x4 result = My_Matrix4x4.zero;

        result.m00 = m.m00; result.m01 = m.m10; result.m02 = m.m20; result.m03 = m.m30;
        result.m10 = m.m01; result.m11 = m.m11; result.m12 = m.m21; result.m13 = m.m31;
        result.m20 = m.m02; result.m21 = m.m12; result.m22 = m.m22; result.m23 = m.m32;
        result.m30 = m.m03; result.m31 = m.m13; result.m32 = m.m23; result.m33 = m.m33;

        return result;
    }

    public static My_Matrix4x4 TRS(Vector3 pos, Quaternion q, Vector3 s)
    {
        My_Matrix4x4 result = Translate(pos) * Rotate(q) * Scale(s);
        return result;
    }

    public override bool Equals(object other)
    {
        return other is My_Matrix4x4 matrix && Equals(matrix);
    }

    public bool Equals(My_Matrix4x4 other)
    {
        return m00 == other.m00 && m01 == other.m01 && m02 == other.m02 && m03 == other.m03 &&
               m10 == other.m10 && m11 == other.m11 && m12 == other.m12 && m13 == other.m13 &&
               m20 == other.m20 && m21 == other.m21 && m22 == other.m22 && m23 == other.m23 &&
               m30 == other.m30 && m31 == other.m31 && m32 == other.m32 && m33 == other.m33;
    }

    public override string ToString()
    {
        return ToString(null, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        return $"({m00.ToString(format, formatProvider)}, {m01.ToString(format, formatProvider)}, {m02.ToString(format, formatProvider)}, {m03.ToString(format, formatProvider)})\n" +
               $"({m10.ToString(format, formatProvider)}, {m11.ToString(format, formatProvider)}, {m12.ToString(format, formatProvider)}, {m13.ToString(format, formatProvider)})\n" +
               $"({m20.ToString(format, formatProvider)}, {m21.ToString(format, formatProvider)}, {m22.ToString(format, formatProvider)}, {m23.ToString(format, formatProvider)})\n" +
               $"({m30.ToString(format, formatProvider)}, {m31.ToString(format, formatProvider)}, {m32.ToString(format, formatProvider)}, {m33.ToString(format, formatProvider)})";
    }

    public static My_Matrix4x4 operator *(My_Matrix4x4 lhs, My_Matrix4x4 rhs)
    {
        My_Matrix4x4 result = My_Matrix4x4.zero;

        result.m00 = lhs.m00 * rhs.m00 + lhs.m01 * rhs.m10 + lhs.m02 * rhs.m20 + lhs.m03 * rhs.m30;
        result.m01 = lhs.m00 * rhs.m01 + lhs.m01 * rhs.m11 + lhs.m02 * rhs.m21 + lhs.m03 * rhs.m31;
        result.m02 = lhs.m00 * rhs.m02 + lhs.m01 * rhs.m12 + lhs.m02 * rhs.m22 + lhs.m03 * rhs.m32;
        result.m03 = lhs.m00 * rhs.m03 + lhs.m01 * rhs.m13 + lhs.m02 * rhs.m23 + lhs.m03 * rhs.m33;
        result.m10 = lhs.m10 * rhs.m00 + lhs.m11 * rhs.m10 + lhs.m12 * rhs.m20 + lhs.m13 * rhs.m30;
        result.m11 = lhs.m10 * rhs.m01 + lhs.m11 * rhs.m11 + lhs.m12 * rhs.m21 + lhs.m13 * rhs.m31;
        result.m12 = lhs.m10 * rhs.m02 + lhs.m11 * rhs.m12 + lhs.m12 * rhs.m22 + lhs.m13 * rhs.m32;
        result.m13 = lhs.m10 * rhs.m03 + lhs.m11 * rhs.m13 + lhs.m12 * rhs.m23 + lhs.m13 * rhs.m33;
        result.m20 = lhs.m20 * rhs.m00 + lhs.m21 * rhs.m10 + lhs.m22 * rhs.m20 + lhs.m23 * rhs.m30;
        result.m21 = lhs.m20 * rhs.m01 + lhs.m21 * rhs.m11 + lhs.m22 * rhs.m21 + lhs.m23 * rhs.m31;
        result.m22 = lhs.m20 * rhs.m02 + lhs.m21 * rhs.m12 + lhs.m22 * rhs.m22 + lhs.m23 * rhs.m32;
        result.m23 = lhs.m20 * rhs.m03 + lhs.m21 * rhs.m13 + lhs.m22 * rhs.m23 + lhs.m23 * rhs.m33;
        result.m30 = lhs.m30 * rhs.m00 + lhs.m31 * rhs.m10 + lhs.m32 * rhs.m20 + lhs.m33 * rhs.m30;
        result.m31 = lhs.m30 * rhs.m01 + lhs.m31 * rhs.m11 + lhs.m32 * rhs.m21 + lhs.m33 * rhs.m31;
        result.m32 = lhs.m30 * rhs.m02 + lhs.m31 * rhs.m12 + lhs.m32 * rhs.m22 + lhs.m33 * rhs.m32;
        result.m33 = lhs.m30 * rhs.m03 + lhs.m31 * rhs.m13 + lhs.m32 * rhs.m23 + lhs.m33 * rhs.m33;

        return result;
    }

    public static Vector4 operator *(My_Matrix4x4 lhs, Vector4 vector)
    {
        Vector4 result;
        result.x = lhs.m00 * vector.x + lhs.m01 * vector.y + lhs.m02 * vector.z + lhs.m03 * vector.w;
        result.y = lhs.m10 * vector.x + lhs.m11 * vector.y + lhs.m12 * vector.z + lhs.m13 * vector.w;
        result.z = lhs.m20 * vector.x + lhs.m21 * vector.y + lhs.m22 * vector.z + lhs.m23 * vector.w;
        result.w = lhs.m30 * vector.x + lhs.m31 * vector.y + lhs.m32 * vector.z + lhs.m33 * vector.w;
        return result;
    }

    public Vector3 MultiplyPoint(Vector3 point)
    {
        Vector3 result;
        result.x = m00 * point.x + m01 * point.y + m02 * point.z + m03;
        result.y = m10 * point.x + m11 * point.y + m12 * point.z + m13;
        result.z = m20 * point.x + m21 * point.y + m22 * point.z + m23;
        float w = m30 * point.x + m31 * point.y + m32 * point.z + m33;
        w = 1.0F / w;
        result.x *= w;
        result.y *= w;
        result.z *= w;
        return result;
    }

    public Vector3 MultiplyPoint3x4(Vector3 point)
    {
        Vector3 result;
        result.x = m00 * point.x + m01 * point.y + m02 * point.z + m03;
        result.y = m10 * point.x + m11 * point.y + m12 * point.z + m13;
        result.z = m20 * point.x + m21 * point.y + m22 * point.z + m23;
        return result;
    }

    public Vector3 MultiplyVector(Vector3 vector)
    {
        Vector3 result;
        result.x = m00 * vector.x + m01 * vector.y + m02 * vector.z;
        result.y = m10 * vector.x + m11 * vector.y + m12 * vector.z;
        result.z = m20 * vector.x + m21 * vector.y + m22 * vector.z;
        return result;
    }

    public Vector4 GetColumn(int index)
    {
        switch (index)
        {
            case 0: return new Vector4(m00, m10, m20, m30);
            case 1: return new Vector4(m01, m11, m21, m31);
            case 2: return new Vector4(m02, m12, m22, m32);
            case 3: return new Vector4(m03, m13, m23, m33);
            default: throw new IndexOutOfRangeException("Invalid column index!");
        }
    }

    public void SetColumn(int index, Vector4 column)
    {
        this[0, index] = column.x;
        this[1, index] = column.y;
        this[2, index] = column.z;
        this[3, index] = column.w;
    }

    public Vector4 GetRow(int index)
    {
        switch (index)
        {
            case 0: return new Vector4(m00, m01, m02, m03);
            case 1: return new Vector4(m10, m11, m12, m13);
            case 2: return new Vector4(m20, m21, m22, m23);
            case 3: return new Vector4(m30, m31, m32, m33);
            default: throw new IndexOutOfRangeException("Invalid row index!");
        }
    }

    public void SetRow(int index, Vector4 row)
    {
        this[index, 0] = row.x;
        this[index, 1] = row.y;
        this[index, 2] = row.z;
        this[index, 3] = row.w;
    }
}
