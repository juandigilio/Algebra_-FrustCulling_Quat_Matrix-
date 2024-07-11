using System;
using UnityEngine;
using CustomMath;


public class My_Matrix4x4
{
    public float M00, M01, M02, M03;
    public float M10, M11, M12, M13;
    public float M20, M21, M22, M23;
    public float M30, M31, M32, M33;

    public My_Matrix4x4(Vector4 column0, Vector4 column1, Vector4 column2, Vector4 column3)
    {
        M00 = column0[0];
        M10 = column0[1];
        M20 = column0[2];
        M30 = column0[3];

        M01 = column1[0];
        M11 = column1[1];
        M21 = column1[2];
        M31 = column1[3];

        M02 = column2[0];
        M12 = column2[1];
        M22 = column2[2];
        M32 = column2[3];

        M03 = column3[0];
        M13 = column3[1];
        M23 = column3[2];
        M33 = column3[3];
    }

    public My_Matrix4x4()
    {
        M00 = 0;
        M10 = 0;
        M20 = 0;
        M30 = 0;

        M01 = 0;
        M11 = 0;
        M21 = 0;
        M31 = 0;

        M02 = 0;
        M12 = 0;
        M22 = 0;
        M32 = 0;

        M03 = 0;
        M13 = 0;
        M23 = 0;
        M33 = 0;
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
                default:
                    throw new IndexOutOfRangeException("Out of range!");
            }
        }

        set
        {
            switch (index)
            {
                case 0: { M00 = value; break; }
                case 1: { M10 = value; break; }
                case 2: { M20 = value; break; }
                case 3: { M30 = value; break; }
                case 4: { M01 = value; break; }
                case 5: { M11 = value; break; }
                case 6: { M21 = value; break; }
                case 7: { M31 = value; break; }
                case 8: { M02 = value; break; }
                case 9: { M12 = value; break; }
                case 10: { M22 = value; break; }
                case 11: { M32 = value; break; }
                case 12: { M03 = value; break; }
                case 13: { M13 = value; break; }
                case 14: { M23 = value; break; }
                case 15: { M33 = value; break; }
                default:
                    throw new IndexOutOfRangeException("Out of range!");
            }
        }
    }

    public float this[int row, int column]
    {
        get { return this[row + column * 4]; }

        set { this[row + column * 4] = value; }
    }

    public static My_Matrix4x4 Zero
    {
        get
        {
            return new My_Matrix4x4();
        }
    }

    public static My_Matrix4x4 Identity
    {
        get
        {
            Vector4 column0 = new Vector4(1, 0, 0, 0);
            Vector4 column1 = new Vector4(0, 1, 0, 0);
            Vector4 column2 = new Vector4(0, 0, 1, 0);
            Vector4 column3 = new Vector4(0, 0, 0, 1);

            return new My_Matrix4x4(column0, column1, column2, column3);
        }
    }

    // la iunversa es la que al multiplicar una matriz por la inversa, se obtiene la matiz identidad
    public My_Matrix4x4 inverse
    {
        get
        {
            float newM00 = M12 * M23 * M31 - M13 * M22 * M31 + M13 * M21 * M32 - M11 * M23 * M32 - M12 * M21 * M33 +
                           M11 * M22 * M33;

            float newM01 = M03 * M22 * M31 - M02 * M23 * M31 - M03 * M21 * M32 + M01 * M23 * M32 + M02 * M21 * M33 -
                           M01 * M22 * M33;

            float newM02 = M02 * M13 * M31 - M03 * M12 * M31 + M03 * M11 * M32 - M01 * M13 * M32 - M02 * M11 * M33 +
                           M01 * M12 * M33;

            float newM03 = M03 * M12 * M21 - M02 * M13 * M21 - M03 * M11 * M22 + M01 * M13 * M22 + M02 * M11 * M23 -
                           M01 * M12 * M23;

            float newM10 = M13 * M22 * M30 - M12 * M23 * M30 - M13 * M20 * M32 + M10 * M23 * M32 + M12 * M20 * M33 -
                           M10 * M22 * M33;

            float newM11 = M02 * M23 * M30 - M03 * M22 * M30 + M03 * M20 * M32 - M00 * M23 * M32 - M02 * M20 * M33 +
                           M00 * M22 * M33;

            float newM12 = M03 * M12 * M30 - M02 * M13 * M30 - M03 * M10 * M32 + M00 * M13 * M32 + M02 * M10 * M33 -
                           M00 * M12 * M33;

            float newM13 = M02 * M13 * M20 - M03 * M12 * M20 + M03 * M10 * M22 - M00 * M13 * M22 - M02 * M10 * M23 +
                           M00 * M12 * M23;

            float newM20 = M11 * M23 * M30 - M13 * M21 * M30 + M13 * M20 * M31 - M10 * M23 * M31 - M11 * M20 * M33 +
                           M10 * M21 * M33;

            float newM21 = M03 * M21 * M30 - M01 * M23 * M30 - M03 * M20 * M31 + M00 * M23 * M31 + M01 * M20 * M33 -
                           M00 * M21 * M33;

            float newM22 = M01 * M13 * M30 - M03 * M11 * M30 + M03 * M10 * M31 - M00 * M13 * M31 - M01 * M10 * M33 +
                           M00 * M11 * M33;

            float newM23 = M03 * M11 * M20 - M01 * M13 * M20 - M03 * M10 * M21 + M00 * M13 * M21 + M01 * M10 * M23 -
                           M00 * M11 * M23;

            float newM30 = M12 * M21 * M30 - M11 * M22 * M30 - M12 * M20 * M31 + M10 * M22 * M31 + M11 * M20 * M32 -
                           M10 * M21 * M32;

            float newM31 = M01 * M22 * M30 - M02 * M21 * M30 + M02 * M20 * M31 - M00 * M22 * M31 - M01 * M20 * M32 +
                           M00 * M21 * M32;

            float newM32 = M02 * M11 * M30 - M01 * M12 * M30 - M02 * M10 * M31 + M00 * M12 * M31 + M01 * M10 * M32 -
                           M00 * M11 * M32;

            float newM33 = M01 * M12 * M20 - M02 * M11 * M20 + M02 * M10 * M21 - M00 * M12 * M21 - M01 * M10 * M22 +
                           M00 * M11 * M22;

            return new My_Matrix4x4(
                new Vector4(newM00, newM01, newM02, newM03),
                new Vector4(newM10, newM11, newM12, newM13),
                new Vector4(newM20, newM21, newM22, newM23),
                new Vector4(newM30, newM31, newM32, newM33)
            );
        }
    }

    public bool IsIdentity
    {
        get
        {
            for (int i = 0; i < 16; i++)
            {
                if (this[i] != Identity[i])
                {
                    return false;
                }
            }
            return true;
        }
    }

    public Vec3 LossyScale =>
        new(
            //escala ejex
            new Vec3(M00, M10, M20).magnitude,
            //y
            new Vec3(M01, M11, M21).magnitude,
            //z
            new Vec3(M02, M12, M22).magnitude
        );
    
    public My_Quaternion Rotation
    {
        get
        {
            My_Quaternion result;
            float factor;

            if (M22 < 0)
            {
                if (M00 > M11)
                {
                    factor = 1 + M00 - M11 - M22;

                    result = new My_Quaternion(factor, M10 + M01, M20 + M02, M12 - M21);
                }
                else
                {
                    factor = 1 - M00 + M11 - M22;

                    result = new My_Quaternion(M01 + M10, factor, M12 + M21, M20 - M02);
                }
            }
            else
            {
                if (M00 < -M11)
                {
                    factor = 1 - M00 - M11 + M22;

                    result = new My_Quaternion(M20 + M02, M12 + M21, factor, M01 - M10);
                }
                else
                {
                    factor = 1 + M00 + M11 + M22;

                    result = new My_Quaternion(M12 - M21, M20 - M02, M01 - M10, factor);
                }
            }

            result *= 0.5f / Mathf.Sqrt(factor);

            return result;
        }
    }

    public Vector4 GetColumn(int index)
    {
        switch (index)
        {
            case 0:
                return new Vector4(M00, M10, M20, M30);
            case 1:
                return new Vector4(M01, M11, M21, M31);
            case 2:
                return new Vector4(M02, M12, M22, M32);
            case 3:
                return new Vector4(M03, M13, M23, M33);
            default:
                throw new IndexOutOfRangeException("Out of range!");
        }
    }

    public Vector4 GetRow(int index)
    {
        switch (index)
        {
            case 0:
                return new Vector4(M00, M01, M02, M03);
            case 1:
                return new Vector4(M10, M11, M12, M13);
            case 2:
                return new Vector4(M20, M21, M22, M23);
            case 3:
                return new Vector4(M30, M31, M32, M33);
            default:
                throw new IndexOutOfRangeException("Out of range!");
        }
    }

    public Vec3 GetPosition()
    {
        return new Vec3(M03, M13, M23);
    }

    public bool Equals(My_Matrix4x4 other)
    {
        for (int i = 0; i < 16; i++)
        {
            if (!this[i].Equals(other[i]))
            {
                return false;
            }
        }
        return true;
    }

    public override bool Equals(object other)
    {
        if (!(other is My_Matrix4x4))
        {
            return false;
        }
        return Equals((My_Matrix4x4)other);
    }

 
}
