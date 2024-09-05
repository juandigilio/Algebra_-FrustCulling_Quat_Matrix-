using CustomMath;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using static Unity.VisualScripting.Metadata;


public class My_Transform
{
    //private My_Matrix4x4 matrix = new My_Matrix4x4();
    public List<My_Transform> childrens { get; set; }

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


    public void DetachChildrens()
    {
        foreach (My_Transform child in childrens)
        {
            child.SetParent(null);
        }

        childCount = 0;
        childrens.Clear();

        hasChanged = true;
    }

    public void DetachChildren(My_Transform children)
    {
        childrens.Remove(children);
        childCount--;
    }

    public int GetChildCount()
    {
        return childCount;
    }

    public void SetParent(My_Transform parent)
    {
        if (this.parent != null)
        {
            this.parent.DetachChildren(this);
            this.parent = null;
        }

        this.parent = parent;

        parent.AddChildren(this);

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

    public void AddChildren(My_Transform children)
    {
        childrens.Add(children);
        children.parent = this;
        childCount++;
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

    public void Rotate(Vec3 eulers, Space relativeTo)
    {
        My_Quaternion eulerRot = My_Quaternion.Euler(eulers.x, eulers.y, eulers.z);

        if (relativeTo == Space.Self)
        {
            localRotation = localRotation * eulerRot;
        }
        else
        {
            rotation = rotation * (My_Quaternion.Inverse(rotation) * eulerRot * rotation);
        }
    }

    public void Rotate(Vec3 eulers)
    {
        Rotate(eulers, Space.Self);
    }

    public void Rotate(float xAngle, float yAngle, float zAngle)
    {
        Rotate(new Vec3(xAngle, yAngle, zAngle));
    }

    public void Rotate(float xAngle, float yAngle, float zAngle, Space relativeTo)
    {
        Rotate(new Vec3(xAngle, yAngle, zAngle), relativeTo);
    }

    public void Rotate(Vec3 axis, float angle, Space relativeTo)
    {
        My_Quaternion axisRotation = My_Quaternion.AngleAxis(angle, axis);

        if (relativeTo == Space.Self)
        {
            localRotation = localRotation * axisRotation;
        }
        else
        {
            rotation = rotation * (My_Quaternion.Inverse(rotation) * axisRotation * rotation);
        }
    }

    public void Rotate(Vec3 axis, float angle)
    {
        Rotate(axis, angle, Space.Self);
    }

    public void RotateAround(Vec3 point, Vec3 axis, float angle)
    {
        My_Quaternion newRotation = My_Quaternion.AngleAxis(angle, axis);

        Vec3 localPoint = point - position;

        Vec3 rotatedPoint = newRotation * localPoint;

        position = rotatedPoint + position;

        newRotation = newRotation * this.rotation;

        this.rotation = newRotation;
    }

    public void RotateAround(Vec3 axis, float angle)
    {
        RotateAround(Vec3.Zero, axis, angle);
    }

    public void SetLocalPositionAndRotation(Vec3 localPosition, My_Quaternion rotation)
    {
        this.localPosition = localPosition;
        this.localRotation = rotation;

        if (parent != null)
        {
            position = parent.position + parent.rotation * localPosition;
            this.rotation = parent.rotation * localRotation;
        }
        else
        {
            position = localPosition;
            this.rotation = localRotation;
        }

        hasChanged = true;
    }

    public void SetPositionAndRotation(Vec3 position, My_Quaternion rotation)
    {
        this.position = position;
        this.rotation = rotation;

        if (parent != null)
        {
            localPosition = My_Quaternion.Inverse(parent.rotation) * (position - parent.position);
            localRotation = My_Quaternion.Inverse(parent.rotation) * rotation;
        }
        else
        {
            localPosition = position;
            localRotation = rotation;
        }

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

    public void Translate(Vec3 translation)
    {
        Translate(translation, Space.Self);
    }

    public void Translate(float x, float y, float z, Space relativeTo)
    {
        Translate(new Vec3(x, y, z), relativeTo);
    }

    public void Translate(float x, float y, float z)
    {
        Translate(new Vec3(x, y, z));
    }

    public void Translate(Vec3 translation, My_Transform relativeTo)
    {
        if (relativeTo != null)
        {
            position += relativeTo.TransformDirection(translation);
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
