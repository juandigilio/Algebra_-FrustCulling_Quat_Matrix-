using CustomMath;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using static Unity.VisualScripting.Metadata;


public class My_Transform
{
    //private My_Matrix4x4 matrix = new My_Matrix4x4();
    private List<My_Transform> childrens { get; set; }

    public Vec3 localPosition { get; set; }
    public Vec3 eulerAngles { get; set; }
    public Vec3 localEulerAngles { get; set; }
    public Vec3 right { get; set; }
    public Vec3 up { get; set; }
    public Vec3 forward { get; set; }
    public My_Quaternion rotation { get; set; }
    public Vec3 position { get; set; }
    public My_Quaternion localRotation { get; set; }
    public My_Transform parent { get; set; }
    public My_Matrix4x4 worldToLocalMatrix { get; set; }
    public My_Matrix4x4 localToWorldMatrix { get; set; }
    public My_Transform root { get; set; }
    public Vec3 lossyScale { get; set; }
    public bool hasChanged { get; set; }
    public Vec3 localScale { get; set; }
    public int childCount { get; set; }


    public void DetachChildren()
    {
        foreach (My_Transform child in childrens)
        {
            child.SetParent(null);
        }

        childCount = 0;

        hasChanged = true;
    }

    //public My_Transform GetChild(int index)
    //{
    //    throw new System.NotImplementedException();
    //}

    public int GetChildCount()
    {
        return childCount;
    }

    //direction from global to local
    public Vec3 InverseTransformDirection(Vec3 direction)
    {
        throw new System.NotImplementedException();
    }

    public Vec3 InverseTransformDirection(float x, float y, float z)
    {
        throw new System.NotImplementedException();
    }

    public Vec3 InverseTransformPoint(Vec3 position)
    {
        throw new System.NotImplementedException();
    }

    public Vec3 InverseTransformPoint(float x, float y, float z)
    {
        throw new System.NotImplementedException();
    }

    public Vec3 InverseTransformVector(Vec3 vector)
    {
        throw new System.NotImplementedException();
    }

    public Vec3 InverseTransformVector(float x, float y, float z)
    {
        throw new System.NotImplementedException();
    }

    public void LookAt(Transform target)
    {
        throw new System.NotImplementedException();
    }

    public void LookAt(Transform target, Vec3 worldUp)
    {
        throw new System.NotImplementedException();
    }

    public void LookAt(Vec3 worldPosition)
    {
        throw new System.NotImplementedException();
    }

    public void LookAt(Vec3 worldPosition, Vec3 worldUp)
    {
        throw new System.NotImplementedException();
    }

    public void Rotate(Vec3 eulers)
    {
        throw new System.NotImplementedException();
    }

    public void Rotate(Vec3 eulers, Space relativeTo)
    {
        throw new System.NotImplementedException();
    }

    public void Rotate(float xAngle, float yAngle, float zAngle)
    {
        throw new System.NotImplementedException();
    }

    public void Rotate(float xAngle, float yAngle, float zAngle, Space relativeTo)
    {
        throw new System.NotImplementedException();
    }

    public void Rotate(Vec3 axis, float angle)
    {
        throw new System.NotImplementedException();
    }

    public void Rotate(Vec3 axis, float angle, Space relativeTo)
    {
        throw new System.NotImplementedException();
    }

    public void RotateAround(Vec3 point, Vec3 axis, float angle)
    {
        throw new System.NotImplementedException();
    }

    public void RotateAround(Vec3 axis, float angle)
    {
        throw new System.NotImplementedException();
    }

    public void SetLocalPositionAndRotation(Vec3 localPosition, My_Quaternion rotation)
    {
        throw new System.NotImplementedException();
    }

    public void SetParent(My_Transform parent)
    {
        this.parent = parent;
        parent.childrens.Add(this);
        parent.childCount++;

        hasChanged = true;
    }

    public void SetParent(My_Transform parent, bool worldPositionStays)
    {
        if (worldPositionStays)
        {
            Vec3 worldPosition = localToWorldMatrix.GetPosition();
            My_Quaternion worldRotation = rotation;

            SetParent(parent);

            if (parent != null)
            {
                position = parent.InverseTransformPoint(worldPosition);
            }
            else
            {
                position = worldPosition;
            }

            rotation = worldRotation;
        }
        else
        {
            SetParent(parent);
        }
    }

    public void SetPositionAndRotation(Vec3 position, My_Quaternion rotation)
    {
        this.position = position;
        this.rotation = rotation;

        hasChanged = true;
    }

    //from local to world
    public Vec3 TransformDirection(Vec3 direction)
    {
        return rotation * direction;
    }

    public Vec3 TransformDirection(float x, float y, float z)
    {
        return TransformDirection(new Vec3(x, y, z));
    }

    public Vec3 TransformPoint(Vec3 position)
    {
        Vec3 transformedPoint = rotation * Vec3.Scale(localPosition, localScale);

        transformedPoint += position;

        return transformedPoint;
    }

    public Vec3 TransformPoint(float x, float y, float z)
    {
        return TransformPoint(new(x, y, z));
    }

    public Vec3 TransformVector(Vec3 vector)
    {
        Vec3 transformedVector = rotation * vector;

        transformedVector = Vec3.Scale(transformedVector, localScale);

        return transformedVector;
    }

    public Vec3 TransformVector(float x, float y, float z)
    {
        return TransformVector(new Vec3(x, y, z));
    }

    public void Translate(Vec3 translation)
    {
        position += translation;

        hasChanged = true;
    }

    public void Translate(float x, float y, float z)
    {
        Translate(new Vec3(x, y, z));
    }

    public void Translate(Vec3 translation, Space relativeTo)
    {
        if (relativeTo == Space.World)
        {
            position += translation;
        }
        else
        {
            position += rotation * translation;
        }

        hasChanged = true;
    }

    public void Translate(float x, float y, float z, Space relativeTo)
    {
        Translate(new Vec3(x, y, z), relativeTo);
    }

    public void Translate(Vec3 translation, My_Transform relativeTo)
    {
        if (relativeTo != null)
        {
            position += relativeTo.rotation * translation;
        }
        else
        {
            position += translation;
        }

        hasChanged = true;
    }

    public void Translate(float x, float y, float z, My_Transform relativeTo)
    {
        Translate(new Vec3(x, y, z), relativeTo);
    }
}
