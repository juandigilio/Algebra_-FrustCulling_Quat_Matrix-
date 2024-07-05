using CustomMath;
using UnityEngine;


namespace System.Numerics
{
    public struct My_Quaternion : IEquatable<My_Quaternion>
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

        public static My_Quaternion Zero
        {
            get => default;
        }

        public static My_Quaternion Identity
        {
            get => new My_Quaternion(0.0f, 0.0f, 0.0f, 1.0f);
        }

        public readonly bool IsIdentity
        {
            get => this == Identity;
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

        public static My_Quaternion operator /(My_Quaternion value1, My_Quaternion value2)
        {
            My_Quaternion ans;

            float q1x = value1.X;
            float q1y = value1.Y;
            float q1z = value1.Z;
            float q1w = value1.W;

            //-------------------------------------
            // Inverse part.
            float ls = value2.X * value2.X + value2.Y * value2.Y +
                       value2.Z * value2.Z + value2.W * value2.W;
            float invNorm = 1.0f / ls;

            float q2x = -value2.X * invNorm;
            float q2y = -value2.Y * invNorm;
            float q2z = -value2.Z * invNorm;
            float q2w = value2.W * invNorm;

            //-------------------------------------
            // Multiply part.

            // cross(av, bv)
            float cx = q1y * q2z - q1z * q2y;
            float cy = q1z * q2x - q1x * q2z;
            float cz = q1x * q2y - q1y * q2x;

            float dot = q1x * q2x + q1y * q2y + q1z * q2z;

            ans.X = q1x * q2w + q2x * q1w + cx;
            ans.Y = q1y * q2w + q2y * q1w + cy;
            ans.Z = q1z * q2w + q2z * q1w + cz;
            ans.W = q1w * q2w - dot;

            return ans;
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
            My_Quaternion ans;

            float q1x = value1.X;
            float q1y = value1.Y;
            float q1z = value1.Z;
            float q1w = value1.W;

            float q2x = value2.X;
            float q2y = value2.Y;
            float q2z = value2.Z;
            float q2w = value2.W;

            // cross(av, bv)
            float cx = q1y * q2z - q1z * q2y;
            float cy = q1z * q2x - q1x * q2z;
            float cz = q1x * q2y - q1y * q2x;

            float dot = q1x * q2x + q1y * q2y + q1z * q2z;

            ans.X = q1x * q2w + q2x * q1w + cx;
            ans.Y = q1y * q2w + q2y * q1w + cy;
            ans.Z = q1z * q2w + q2z * q1w + cz;
            ans.W = q1w * q2w - dot;

            return ans;
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

        public static My_Quaternion Add(My_Quaternion value1, My_Quaternion value2)
        {
            return value1 + value2;
        }

        public static My_Quaternion Concatenate(My_Quaternion value1, My_Quaternion value2)
        {
            My_Quaternion ans;

            // Concatenate rotation is actually q2 * q1 instead of q1 * q2.
            // So that's why value2 goes q1 and value1 goes q2.
            float q1x = value2.X;
            float q1y = value2.Y;
            float q1z = value2.Z;
            float q1w = value2.W;

            float q2x = value1.X;
            float q2y = value1.Y;
            float q2z = value1.Z;
            float q2w = value1.W;

            // cross(av, bv)
            float cx = q1y * q2z - q1z * q2y;
            float cy = q1z * q2x - q1x * q2z;
            float cz = q1x * q2y - q1y * q2x;

            float dot = q1x * q2x + q1y * q2y + q1z * q2z;

            ans.X = q1x * q2w + q2x * q1w + cx;
            ans.Y = q1y * q2w + q2y * q1w + cy;
            ans.Z = q1z * q2w + q2z * q1w + cz;
            ans.W = q1w * q2w - dot;

            return ans;
        }

        public static My_Quaternion RotateTowards(My_Quaternion from, My_Quaternion to, float maxDegreesDelta)
        {
            float num = My_Quaternion.Dot(from, to);
            if (num > 1f)
            {
                return to;
            }
            else if (num < -1f)
            {
                return from;
            }
            float num2 = Mathf.Acos(num);
            float t = Mathf.Min(1f, maxDegreesDelta / num2);
            My_Quaternion result = My_Quaternion.SlerpUnclamped(from, to, t);
            result.Normalize();
            return result;
        }

        public static My_Quaternion LerpUnclamped(My_Quaternion a, My_Quaternion b, float t)
        {
            return new My_Quaternion(
                Mathf.LerpUnclamped(a.X, b.X, t),
                Mathf.LerpUnclamped(a.Y, b.Y, t),
                Mathf.LerpUnclamped(a.Z, b.Z, t),
                Mathf.LerpUnclamped(a.W, b.W, t)
            );
        }

        public static My_Quaternion SlerpUnclamped(My_Quaternion a, My_Quaternion b, float t)
        {
            // Si los cuaterniones son iguales, retornar uno de ellos
            if (My_Quaternion.Dot(a, b) > 0.9995f)
            {
                return My_Quaternion.LerpUnclamped(a, b, t);
            }

            float cosHalfTheta = a.W * b.W + a.X * b.X + a.Y * b.Y + a.Z * b.Z;

            if (Mathf.Abs(cosHalfTheta) >= 1.0f)
            {
                return a;
            }

            float halfTheta = Mathf.Acos(cosHalfTheta);
            float sinHalfTheta = Mathf.Sqrt(1.0f - cosHalfTheta * cosHalfTheta);

            float ratioA = Mathf.Sin((1.0f - t) * halfTheta) / sinHalfTheta;
            float ratioB = Mathf.Sin(t * halfTheta) / sinHalfTheta;

            return new My_Quaternion(
                a.X * ratioA + b.X * ratioB,
                a.Y * ratioA + b.Y * ratioB,
                a.Z * ratioA + b.Z * ratioB,
                a.W * ratioA + b.W * ratioB
            );
        }

        public static My_Quaternion Conjugate(My_Quaternion value)
        {
            return new My_Quaternion(-value.X, -value.Y, -value.Z, value.W);
        }

        public static My_Quaternion CreateFromAxisAngle(Vec3 axis, float angle)
        {
            My_Quaternion ans;

            float halfAngle = angle * 0.5f;
            float s = MathF.Sin(halfAngle);
            float c = MathF.Cos(halfAngle);

            ans.X = axis.x * s;
            ans.Y = axis.y * s;
            ans.Z = axis.z * s;
            ans.W = c;

            return ans;
        }

        public static My_Quaternion CreateFromRotationMatrix(Matrix4x4 matrix)
        {
            float trace = matrix.M11 + matrix.M22 + matrix.M33;

            My_Quaternion q = default;

            if (trace > 0.0f)
            {
                float s = MathF.Sqrt(trace + 1.0f);
                q.W = s * 0.5f;
                s = 0.5f / s;
                q.X = (matrix.M23 - matrix.M32) * s;
                q.Y = (matrix.M31 - matrix.M13) * s;
                q.Z = (matrix.M12 - matrix.M21) * s;
            }
            else
            {
                if (matrix.M11 >= matrix.M22 && matrix.M11 >= matrix.M33)
                {
                    float s = MathF.Sqrt(1.0f + matrix.M11 - matrix.M22 - matrix.M33);
                    float invS = 0.5f / s;
                    q.X = 0.5f * s;
                    q.Y = (matrix.M12 + matrix.M21) * invS;
                    q.Z = (matrix.M13 + matrix.M31) * invS;
                    q.W = (matrix.M23 - matrix.M32) * invS;
                }
                else if (matrix.M22 > matrix.M33)
                {
                    float s = MathF.Sqrt(1.0f + matrix.M22 - matrix.M11 - matrix.M33);
                    float invS = 0.5f / s;
                    q.X = (matrix.M21 + matrix.M12) * invS;
                    q.Y = 0.5f * s;
                    q.Z = (matrix.M32 + matrix.M23) * invS;
                    q.W = (matrix.M31 - matrix.M13) * invS;
                }
                else
                {
                    float s = MathF.Sqrt(1.0f + matrix.M33 - matrix.M11 - matrix.M22);
                    float invS = 0.5f / s;
                    q.X = (matrix.M31 + matrix.M13) * invS;
                    q.Y = (matrix.M32 + matrix.M23) * invS;
                    q.Z = 0.5f * s;
                    q.W = (matrix.M12 - matrix.M21) * invS;
                }
            }

            return q;
        }

        public static My_Quaternion CreateFromYawPitchRoll(float yaw, float pitch, float roll)
        {
            //  Roll first, about axis the object is facing, then
            //  pitch upward, then yaw to face into the new heading
            float sr, cr, sp, cp, sy, cy;

            float halfRoll = roll * 0.5f;
            sr = MathF.Sin(halfRoll);
            cr = MathF.Cos(halfRoll);

            float halfPitch = pitch * 0.5f;
            sp = MathF.Sin(halfPitch);
            cp = MathF.Cos(halfPitch);

            float halfYaw = yaw * 0.5f;
            sy = MathF.Sin(halfYaw);
            cy = MathF.Cos(halfYaw);

            My_Quaternion result;

            result.X = cy * sp * cr + sy * cp * sr;
            result.Y = sy * cp * cr - cy * sp * sr;
            result.Z = cy * cp * sr - sy * sp * cr;
            result.W = cy * cp * cr + sy * sp * sr;

            return result;
        }

        public static My_Quaternion Divide(My_Quaternion value1, My_Quaternion value2)
        {
            return value1 / value2;
        }

        internal static My_Quaternion Divide(My_Quaternion left, float divisor)
        {
            return new My_Quaternion(
                left.X / divisor,
                left.Y / divisor,
                left.Z / divisor,
                left.W / divisor
            );
        }

        public static float Dot(My_Quaternion quaternion1, My_Quaternion quaternion2)
        {
            return (quaternion1.X * quaternion2.X)
                 + (quaternion1.Y * quaternion2.Y)
                 + (quaternion1.Z * quaternion2.Z)
                 + (quaternion1.W * quaternion2.W);
        }

        public static My_Quaternion Inverse(My_Quaternion value)
        {
            return Divide(Conjugate(value), value.LengthSquared());
        }

        public static My_Quaternion Lerp(My_Quaternion quaternion1, My_Quaternion quaternion2, float amount)
        {
            float t = amount;
            float t1 = 1.0f - t;

            My_Quaternion r = default;

            float dot = quaternion1.X * quaternion2.X + quaternion1.Y * quaternion2.Y +
                        quaternion1.Z * quaternion2.Z + quaternion1.W * quaternion2.W;

            if (dot >= 0.0f)
            {
                r.X = t1 * quaternion1.X + t * quaternion2.X;
                r.Y = t1 * quaternion1.Y + t * quaternion2.Y;
                r.Z = t1 * quaternion1.Z + t * quaternion2.Z;
                r.W = t1 * quaternion1.W + t * quaternion2.W;
            }
            else
            {
                r.X = t1 * quaternion1.X - t * quaternion2.X;
                r.Y = t1 * quaternion1.Y - t * quaternion2.Y;
                r.Z = t1 * quaternion1.Z - t * quaternion2.Z;
                r.W = t1 * quaternion1.W - t * quaternion2.W;
            }

            //Normalize
            float ls = r.X * r.X + r.Y * r.Y + r.Z * r.Z + r.W * r.W;
            float invNorm = 1.0f / MathF.Sqrt(ls);

            r.X *= invNorm;
            r.Y *= invNorm;
            r.Z *= invNorm;
            r.W *= invNorm;

            return r;
        }

        public static My_Quaternion Multiply(My_Quaternion value1, My_Quaternion value2)
        {
            return value1 * value2;
        }

        internal static My_Quaternion Multiply(My_Quaternion value1, Vector4 value2)
        {
            return new My_Quaternion(
                value1.X * value2.X,
                value1.Y * value2.Y,
                value1.Z * value2.Z,
                value1.W * value2.W
            );
        }

        public static My_Quaternion Multiply(My_Quaternion value1, float value2)
        {
            return value1 * value2;
        }

        public static My_Quaternion Negate(My_Quaternion value)
        {
            return -value;
        }

        public My_Quaternion Normalize()
        {
            return Divide(this, Length());
        }

        public static My_Quaternion Euler(float x, float y, float z)
        {
            float halfToRad = Mathf.Deg2Rad * 0.5f;
            float sx = Mathf.Sin(x * halfToRad);
            float cx = Mathf.Cos(x * halfToRad);
            float sy = Mathf.Sin(y * halfToRad);
            float cy = Mathf.Cos(y * halfToRad);
            float sz = Mathf.Sin(z * halfToRad);
            float cz = Mathf.Cos(z * halfToRad);

            My_Quaternion result;
            result.X = sx * cy * cz + cx * sy * sz;
            result.Y = cx * sy * cz - sx * cy * sz;
            result.Z = cx * cy * sz - sx * sy * cz;
            result.W = cx * cy * cz + sx * sy * sz;

            return result;
        }

        public static My_Quaternion AngleAxis(float angle, Vec3 newAxis)
        {
            newAxis.Normalize();
            float halfAngle = angle * Mathf.Deg2Rad * 0.5f;
            float s = Mathf.Sin(halfAngle);

            return new My_Quaternion(
                newAxis.x * s,
                newAxis.y * s,
                newAxis.z * s,
                Mathf.Cos(halfAngle)
            );
        }

        public static My_Quaternion Slerp(My_Quaternion quaternion1, My_Quaternion quaternion2, float amount)
        {
            float t = amount;

            float cosOmega = quaternion1.X * quaternion2.X + quaternion1.Y * quaternion2.Y +
                             quaternion1.Z * quaternion2.Z + quaternion1.W * quaternion2.W;

            bool flip = false;

            if (cosOmega < 0.0f)
            {
                flip = true;
                cosOmega = -cosOmega;
            }

            float s1, s2;

            if (cosOmega > (1.0f - SlerpEpsilon))
            {
                // Too close, do straight linear interpolation.
                s1 = 1.0f - t;
                s2 = (flip) ? -t : t;
            }
            else
            {
                float omega = MathF.Acos(cosOmega);
                float invSinOmega = 1 / MathF.Sin(omega);

                s1 = MathF.Sin((1.0f - t) * omega) * invSinOmega;
                s2 = (flip)
                    ? -MathF.Sin(t * omega) * invSinOmega
                    : MathF.Sin(t * omega) * invSinOmega;
            }

            My_Quaternion ans;

            ans.X = s1 * quaternion1.X + s2 * quaternion2.X;
            ans.Y = s1 * quaternion1.Y + s2 * quaternion2.Y;
            ans.Z = s1 * quaternion1.Z + s2 * quaternion2.Z;
            ans.W = s1 * quaternion1.W + s2 * quaternion2.W;

            return ans;
        }

        public static My_Quaternion Subtract(My_Quaternion value1, My_Quaternion value2)
        {
            return value1 - value2;
        }

        public override readonly bool Equals( object obj)
        {
            return (obj is My_Quaternion other) && Equals(other);
        }

        public readonly bool Equals(My_Quaternion other)
        {
            return X == other.X && Y == other.Y && Z == other.Z && W == other.W;
        }

        public override readonly int GetHashCode()
        {
            return HashCode.Combine(X, Y, Z, W);
        }

        public readonly float Length()
        {
            float lengthSquared = LengthSquared();
            return MathF.Sqrt(lengthSquared);
        }

        public readonly float LengthSquared()
        {
            return Dot(this, this);
        }

        public override readonly string ToString() =>
            $"{{X:{X} Y:{Y} Z:{Z} W:{W}}}";
    }
}