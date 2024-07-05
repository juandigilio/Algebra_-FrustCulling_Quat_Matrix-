using System;
using UnityEngine;
using System.Numerics;
using CustomMath;
using Vector4 = UnityEngine.Vector4;

public class My_Matrix4x4 : IEquatable<My_Matrix4x4>, IFormattable
{
    public float M00, M01, M02, M03;
    public float M10, M11, M12, M13;
    public float M20, M21, M22, M23;
    public float M30, M31, M32, M33;

    public My_Matrix4x4(Vector4 column0, Vector4 column1, Vector4 column2, Vector4 column3)
    {
        M00 = column0.x; M01 = column1.x; M02 = column2.x; M03 = column3.x;
        M10 = column0.y; M11 = column1.y; M12 = column2.y; M13 = column3.y;
        M20 = column0.z; M21 = column1.z; M22 = column2.z; M23 = column3.z;
        M30 = column0.w; M31 = column1.w; M32 = column2.w; M33 = column3.w;
    }

    public float this[int index]
    {
        get
        {
            switch (index)
            {
                case 0: return M00;
                case 1: return M10;
                case 2: return M20;
                case 3: return M30;
                case 4: return M01;
                case 5: return M11;
                case 6: return M21;
                case 7: return M31;
                case 8: return M02;
                case 9: return M12;
                case 10: return M22;
                case 11: return M32;
                case 12: return M03;
                case 13: return M13;
                case 14: return M23;
                case 15: return M33;
                default: throw new IndexOutOfRangeException("Invalid matrix index!");
            }
        }
        set
        {
            switch (index)
            {
                case 0: M00 = value; break;
                case 1: M10 = value; break;
                case 2: M20 = value; break;
                case 3: M30 = value; break;
                case 4: M01 = value; break;
                case 5: M11 = value; break;
                case 6: M21 = value; break;
                case 7: M31 = value; break;
                case 8: M02 = value; break;
                case 9: M12 = value; break;
                case 10: M22 = value; break;
                case 11: M32 = value; break;
                case 12: M03 = value; break;
                case 13: M13 = value; break;
                case 14: M23 = value; break;
                case 15: M33 = value; break;
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

    public My_Quaternion rotation => My_Quaternion.LookRotation((Vector4)GetColumn(2), GetColumn(1));
    public Vec3 lossyScale => new Vec3(GetColumn(0).magnitude, GetColumn(1).magnitude, GetColumn(2).magnitude);
    public bool isIdentity => this == identity;
    public float determinant => Determinant(this);
    public My_Matrix4x4 transpose => Transpose(this);
    public My_Matrix4x4 inverse => Inverse(this);


    public static My_Matrix4x4 operator *(My_Matrix4x4 matrix, float scalar)
    {
        Vector4 column_0 = new Vector4(matrix.M00 * scalar, matrix.M10 * scalar, matrix.M20 * scalar, matrix.M30 * scalar);
        Vector4 column_1 = new Vector4(matrix.M01 * scalar, matrix.M11 * scalar, matrix.M21 * scalar, matrix.M31 * scalar);
        Vector4 column_2 = new Vector4(matrix.M02 * scalar, matrix.M12 * scalar, matrix.M22 * scalar, matrix.M32 * scalar);
        Vector4 column_3 = new Vector4(matrix.M03 * scalar, matrix.M13 * scalar, matrix.M23 * scalar, matrix.M33 * scalar);

        return new My_Matrix4x4(column_0, column_1, column_2, column_3);
    }

    public static float Determinant(My_Matrix4x4 m)
    {
        return m.M00 * m.M11 * m.M22 * m.M33 + m.M01 * m.M12 * m.M23 * m.M30 + m.M02 * m.M13 * m.M20 * m.M31 +
               m.M03 * m.M10 * m.M21 * m.M32 - m.M03 * m.M12 * m.M21 * m.M30 - m.M02 * m.M11 * m.M20 * m.M33 -
               m.M01 * m.M10 * m.M23 * m.M32 - m.M00 * m.M13 * m.M22 * m.M31;
    }

    public static My_Matrix4x4 Frustum(float left, float right, float bottom, float top, float zNear, float zFar)
    {
        My_Matrix4x4 result = zero;
        result.M00 = 2.0F * zNear / (right - left);
        result.M11 = 2.0F * zNear / (top - bottom);
        result.M02 = (right + left) / (right - left);
        result.M12 = (top + bottom) / (top - bottom);
        result.M22 = -(zFar + zNear) / (zFar - zNear);
        result.M32 = -1.0F;
        result.M23 = -(2.0F * zFar * zNear) / (zFar - zNear);
        return result;
    }

    public static My_Matrix4x4 Frustum(FrustumPlanes fp)
    {
        return Frustum(fp.left, fp.right, fp.bottom, fp.top, fp.zNear, fp.zFar);
    }

    public static My_Matrix4x4 LookAt(Vec3 from, Vec3 to, Vec3 up)
    {
        My_Quaternion rotation = My_Quaternion.LookRotation(to - from, up);
        return My_Matrix4x4.TRS(from, rotation, Vec3.One);
    }

    public static My_Matrix4x4 Inverse(My_Matrix4x4 m)
    {
        float determinant = Determinant(m);

        if (Mathf.Abs(determinant) < Mathf.Epsilon)
            throw new InvalidOperationException("Matrix is not invertible.");

        My_Matrix4x4 result = zero;

        result.M00 = m.M11 * m.M22 * m.M33 - m.M11 * m.M23 * m.M32 - m.M21 * m.M12 * m.M33 + m.M21 * m.M13 * m.M32 + m.M31 * m.M12 * m.M23 - m.M31 * m.M13 * m.M22;
        result.M01 = -m.M01 * m.M22 * m.M33 + m.M01 * m.M23 * m.M32 + m.M21 * m.M02 * m.M33 - m.M21 * m.M03 * m.M32 - m.M31 * m.M02 * m.M23 + m.M31 * m.M03 * m.M22;
        result.M02 = m.M01 * m.M12 * m.M33 - m.M01 * m.M13 * m.M32 - m.M11 * m.M02 * m.M33 + m.M11 * m.M03 * m.M32 + m.M31 * m.M02 * m.M13 - m.M31 * m.M03 * m.M12;
        result.M03 = -m.M01 * m.M12 * m.M23 + m.M01 * m.M13 * m.M22 + m.M11 * m.M02 * m.M23 - m.M11 * m.M03 * m.M22 - m.M21 * m.M02 * m.M13 + m.M21 * m.M03 * m.M12;
        result.M10 = -m.M10 * m.M22 * m.M33 + m.M10 * m.M23 * m.M32 + m.M20 * m.M12 * m.M33 - m.M20 * m.M13 * m.M32 - m.M30 * m.M12 * m.M23 + m.M30 * m.M13 * m.M22;
        result.M11 = m.M00 * m.M22 * m.M33 - m.M00 * m.M23 * m.M32 - m.M20 * m.M02 * m.M33 + m.M20 * m.M03 * m.M32 + m.M30 * m.M02 * m.M23 - m.M30 * m.M03 * m.M22;
        result.M12 = -m.M00 * m.M12 * m.M33 + m.M00 * m.M13 * m.M32 + m.M10 * m.M02 * m.M33 - m.M10 * m.M03 * m.M32 - m.M30 * m.M02 * m.M13 + m.M30 * m.M03 * m.M12;
        result.M13 = m.M00 * m.M12 * m.M23 - m.M00 * m.M13 * m.M22 - m.M10 * m.M02 * m.M23 + m.M10 * m.M03 * m.M22 + m.M20 * m.M02 * m.M13 - m.M20 * m.M03 * m.M12;
        result.M20 = m.M10 * m.M21 * m.M33 - m.M10 * m.M23 * m.M31 - m.M20 * m.M11 * m.M33 + m.M20 * m.M13 * m.M31 + m.M30 * m.M11 * m.M23 - m.M30 * m.M13 * m.M21;
        result.M21 = -m.M00 * m.M21 * m.M33 + m.M00 * m.M23 * m.M31 + m.M20 * m.M01 * m.M33 - m.M20 * m.M03 * m.M31 - m.M30 * m.M01 * m.M23 + m.M30 * m.M03 * m.M21;
        result.M22 = m.M00 * m.M11 * m.M33 - m.M00 * m.M13 * m.M31 - m.M10 * m.M01 * m.M33 + m.M10 * m.M03 * m.M31 + m.M30 * m.M01 * m.M13 - m.M30 * m.M03 * m.M11;
        result.M23 = -m.M00 * m.M11 * m.M23 + m.M00 * m.M13 * m.M21 + m.M10 * m.M01 * m.M23 - m.M10 * m.M03 * m.M21 - m.M20 * m.M01 * m.M13 + m.M20 * m.M03 * m.M11;
        result.M30 = -m.M10 * m.M21 * m.M32 + m.M10 * m.M22 * m.M31 + m.M20 * m.M11 * m.M32 - m.M20 * m.M12 * m.M31 - m.M30 * m.M11 * m.M22 + m.M30 * m.M12 * m.M21;
        result.M31 = m.M00 * m.M21 * m.M32 - m.M00 * m.M22 * m.M31 - m.M20 * m.M01 * m.M32 + m.M20 * m.M02 * m.M31 + m.M30 * m.M01 * m.M22 - m.M30 * m.M02 * m.M21;
        result.M32 = -m.M00 * m.M11 * m.M32 + m.M00 * m.M12 * m.M31 + m.M10 * m.M01 * m.M32 - m.M10 * m.M02 * m.M31 - m.M30 * m.M01 * m.M12 + m.M30 * m.M02 * m.M11;
        result.M33 = m.M00 * m.M11 * m.M22 - m.M00 * m.M12 * m.M21 - m.M10 * m.M01 * m.M22 + m.M10 * m.M02 * m.M21 + m.M20 * m.M01 * m.M12 - m.M20 * m.M02 * m.M11;
        return result * (1.0F / determinant);
    }

    public static bool Inverse3DAffine(My_Matrix4x4 input, ref My_Matrix4x4 result)
    {
        Vec3 s = input.lossyScale;

        if (Mathf.Abs(s.x) < Mathf.Epsilon || Mathf.Abs(s.y) < Mathf.Epsilon || Mathf.Abs(s.z) < Mathf.Epsilon)
        {
            result = identity;
            return false;
        }

        My_Quaternion r = input.rotation;
        Vec3 t = input.GetPosition();

        result = TRS(Vec3.Zero, My_Quaternion.Inverse(r), new Vec3(1 / s.x, 1 / s.y, 1 / s.z));
        result = Translate(-t) * result;
        return true;
    }

    public Vec3 GetPosition()
    {
        return new Vec3(M03, M13, M23);
    }

    public static My_Matrix4x4 Ortho(float left, float right, float bottom, float top, float zNear, float zFar)
    {
        My_Matrix4x4 result = identity;
        result.M00 = 2.0F / (right - left);
        result.M11 = 2.0F / (top - bottom);
        result.M22 = -2.0F / (zFar - zNear);
        result.M03 = -(right + left) / (right - left);
        result.M13 = -(top + bottom) / (top - bottom);
        result.M23 = -(zFar + zNear) / (zFar - zNear);
        return result;
    }

    public static My_Matrix4x4 Perspective(float fov, float aspect, float zNear, float zFar)
    {
        float tanHalfFov = Mathf.Tan(0.5F * Mathf.Deg2Rad * fov);
        My_Matrix4x4 result = zero;
        result.M00 = 1.0F / (aspect * tanHalfFov);
        result.M11 = 1.0F / tanHalfFov;
        result.M22 = -(zFar + zNear) / (zFar - zNear);
        result.M32 = -1.0F;
        result.M23 = -(2.0F * zFar * zNear) / (zFar - zNear);
        return result;
    }

    public static My_Matrix4x4 Rotate(My_Quaternion q)
    {
        float x = q.X * 2.0F;
        float y = q.Y * 2.0F;
        float z = q.Z * 2.0F;
        float xx = q.X * x;
        float yy = q.Y * y;
        float zz = q.Z * z;
        float xy = q.X * y;
        float xz = q.X * z;
        float yz = q.Y * z;
        float wx = q.W * x;
        float wy = q.W * y;
        float wz = q.W * z;

        My_Matrix4x4 result = identity;
        result.M00 = 1.0F - (yy + zz);
        result.M10 = xy + wz;
        result.M20 = xz - wy;
        result.M01 = xy - wz;
        result.M11 = 1.0F - (xx + zz);
        result.M21 = yz + wx;
        result.M02 = xz + wy;
        result.M12 = yz - wx;
        result.M22 = 1.0F - (xx + yy);
        return result;
    }

    public static My_Matrix4x4 Scale(Vec3 vector)
    {
        My_Matrix4x4 result = zero;
        result.M00 = vector.x;
        result.M11 = vector.y;
        result.M22 = vector.z;
        result.M33 = 1.0F;
        return result;
    }

    public static My_Matrix4x4 Translate(Vec3 vector)
    {
        My_Matrix4x4 result = identity;
        result.M03 = vector.x;
        result.M13 = vector.y;
        result.M23 = vector.z;
        return result;
    }

    public static My_Matrix4x4 Transpose(My_Matrix4x4 m)
    {
        My_Matrix4x4 result = My_Matrix4x4.zero;

        result.M00 = m.M00; result.M01 = m.M10; result.M02 = m.M20; result.M03 = m.M30;
        result.M10 = m.M01; result.M11 = m.M11; result.M12 = m.M21; result.M13 = m.M31;
        result.M20 = m.M02; result.M21 = m.M12; result.M22 = m.M22; result.M23 = m.M32;
        result.M30 = m.M03; result.M31 = m.M13; result.M32 = m.M23; result.M33 = m.M33;

        return result;
    }

    public static My_Matrix4x4 TRS(Vec3 pos, My_Quaternion q, Vec3 s)
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
        return M00 == other.M00 && M01 == other.M01 && M02 == other.M02 && M03 == other.M03 &&
               M10 == other.M10 && M11 == other.M11 && M12 == other.M12 && M13 == other.M13 &&
               M20 == other.M20 && M21 == other.M21 && M22 == other.M22 && M23 == other.M23 &&
               M30 == other.M30 && M31 == other.M31 && M32 == other.M32 && M33 == other.M33;
    }

    public override string ToString()
    {
        return ToString(null, System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
    }

    public string ToString(string format, IFormatProvider formatProvider)
    {
        return $"({M00.ToString(format, formatProvider)}, {M01.ToString(format, formatProvider)}, {M02.ToString(format, formatProvider)}, {M03.ToString(format, formatProvider)})\n" +
               $"({M10.ToString(format, formatProvider)}, {M11.ToString(format, formatProvider)}, {M12.ToString(format, formatProvider)}, {M13.ToString(format, formatProvider)})\n" +
               $"({M20.ToString(format, formatProvider)}, {M21.ToString(format, formatProvider)}, {M22.ToString(format, formatProvider)}, {M23.ToString(format, formatProvider)})\n" +
               $"({M30.ToString(format, formatProvider)}, {M31.ToString(format, formatProvider)}, {M32.ToString(format, formatProvider)}, {M33.ToString(format, formatProvider)})";
    }

    public static My_Matrix4x4 operator *(My_Matrix4x4 lhs, My_Matrix4x4 rhs)
    {
        My_Matrix4x4 result = My_Matrix4x4.zero;

        result.M00 = lhs.M00 * rhs.M00 + lhs.M01 * rhs.M10 + lhs.M02 * rhs.M20 + lhs.M03 * rhs.M30;
        result.M01 = lhs.M00 * rhs.M01 + lhs.M01 * rhs.M11 + lhs.M02 * rhs.M21 + lhs.M03 * rhs.M31;
        result.M02 = lhs.M00 * rhs.M02 + lhs.M01 * rhs.M12 + lhs.M02 * rhs.M22 + lhs.M03 * rhs.M32;
        result.M03 = lhs.M00 * rhs.M03 + lhs.M01 * rhs.M13 + lhs.M02 * rhs.M23 + lhs.M03 * rhs.M33;
        result.M10 = lhs.M10 * rhs.M00 + lhs.M11 * rhs.M10 + lhs.M12 * rhs.M20 + lhs.M13 * rhs.M30;
        result.M11 = lhs.M10 * rhs.M01 + lhs.M11 * rhs.M11 + lhs.M12 * rhs.M21 + lhs.M13 * rhs.M31;
        result.M12 = lhs.M10 * rhs.M02 + lhs.M11 * rhs.M12 + lhs.M12 * rhs.M22 + lhs.M13 * rhs.M32;
        result.M13 = lhs.M10 * rhs.M03 + lhs.M11 * rhs.M13 + lhs.M12 * rhs.M23 + lhs.M13 * rhs.M33;
        result.M20 = lhs.M20 * rhs.M00 + lhs.M21 * rhs.M10 + lhs.M22 * rhs.M20 + lhs.M23 * rhs.M30;
        result.M21 = lhs.M20 * rhs.M01 + lhs.M21 * rhs.M11 + lhs.M22 * rhs.M21 + lhs.M23 * rhs.M31;
        result.M22 = lhs.M20 * rhs.M02 + lhs.M21 * rhs.M12 + lhs.M22 * rhs.M22 + lhs.M23 * rhs.M32;
        result.M23 = lhs.M20 * rhs.M03 + lhs.M21 * rhs.M13 + lhs.M22 * rhs.M23 + lhs.M23 * rhs.M33;
        result.M30 = lhs.M30 * rhs.M00 + lhs.M31 * rhs.M10 + lhs.M32 * rhs.M20 + lhs.M33 * rhs.M30;
        result.M31 = lhs.M30 * rhs.M01 + lhs.M31 * rhs.M11 + lhs.M32 * rhs.M21 + lhs.M33 * rhs.M31;
        result.M32 = lhs.M30 * rhs.M02 + lhs.M31 * rhs.M12 + lhs.M32 * rhs.M22 + lhs.M33 * rhs.M32;
        result.M33 = lhs.M30 * rhs.M03 + lhs.M31 * rhs.M13 + lhs.M32 * rhs.M23 + lhs.M33 * rhs.M33;

        return result;
    }

    public static Vector4 operator *(My_Matrix4x4 lhs, Vector4 vector)
    {
        Vector4 result;
        result.x = lhs.M00 * vector.x + lhs.M01 * vector.y + lhs.M02 * vector.z + lhs.M03 * vector.w;
        result.y = lhs.M10 * vector.x + lhs.M11 * vector.y + lhs.M12 * vector.z + lhs.M13 * vector.w;
        result.z = lhs.M20 * vector.x + lhs.M21 * vector.y + lhs.M22 * vector.z + lhs.M23 * vector.w;
        result.w = lhs.M30 * vector.x + lhs.M31 * vector.y + lhs.M32 * vector.z + lhs.M33 * vector.w;
        return result;
    }

    public Vec3 MultiplyPoint(Vec3 point)
    {
        Vec3 result;
        result.x = M00 * point.x + M01 * point.y + M02 * point.z + M03;
        result.y = M10 * point.x + M11 * point.y + M12 * point.z + M13;
        result.z = M20 * point.x + M21 * point.y + M22 * point.z + M23;
        float w = M30 * point.x + M31 * point.y + M32 * point.z + M33;
        w = 1.0F / w;
        result.x *= w;
        result.y *= w;
        result.z *= w;
        return result;
    }

    public Vec3 MultiplyPoint3x4(Vec3 point)
    {
        Vec3 result;
        result.x = M00 * point.x + M01 * point.y + M02 * point.z + M03;
        result.y = M10 * point.x + M11 * point.y + M12 * point.z + M13;
        result.z = M20 * point.x + M21 * point.y + M22 * point.z + M23;
        return result;
    }

    public Vec3 MultiplyVector(Vec3 vector)
    {
        Vec3 result;
        result.x = M00 * vector.x + M01 * vector.y + M02 * vector.z;
        result.y = M10 * vector.x + M11 * vector.y + M12 * vector.z;
        result.z = M20 * vector.x + M21 * vector.y + M22 * vector.z;
        return result;
    }

    public Vector4 GetColumn(int index)
    {
        switch (index)
        {
            case 0: return new Vector4(M00, M10, M20, M30);
            case 1: return new Vector4(M01, M11, M21, M31);
            case 2: return new Vector4(M02, M12, M22, M32);
            case 3: return new Vector4(M03, M13, M23, M33);
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
            case 0: return new Vector4(M00, M01, M02, M03);
            case 1: return new Vector4(M10, M11, M12, M13);
            case 2: return new Vector4(M20, M21, M22, M23);
            case 3: return new Vector4(M30, M31, M32, M33);
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
