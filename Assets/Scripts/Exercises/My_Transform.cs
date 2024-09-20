using CustomMath;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class My_Transform
{
    public Vec3 position { get; set; }
    public Vec3 localPosition { get; set; }
    public My_Quaternion rotation { get; set; }
    public My_Quaternion localRotation { get; set; }
    public Vec3 eulerAngles { get; set; }
    public Vec3 localEulerAngles { get; set; }
    public Vec3 right { get; set; }
    public Vec3 up { get; set; }
    public Vec3 forward { get; set; }
    public My_Matrix4x4 worldToLocalMatrix { get; set; }
    public My_Matrix4x4 localToWorldMatrix { get; set; }
    public Vec3 lossyScale { get; set; }
    public Vec3 localScale { get; set; }
    public bool hasChanged { get; set; }
    public My_Transform parent { get; set; }
    public List<My_Transform> childrens { get; set; }
    public int childCount { get; set; }

    //To check list
    //LookAt

    public My_Transform()
    {
        rotation = My_Quaternion.Identity;
        localRotation = My_Quaternion.Identity;
        worldToLocalMatrix = My_Matrix4x4.Identity;

        childrens = new List<My_Transform>();
    }

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

    public void SetParent(My_Transform newParent)
    {
        if (this.parent != null)
        {
            this.parent.DetachChildren(this);
            this.parent = null;
        }

        Vec3 worldPosition = this.position;
        My_Quaternion worldRotation = this.rotation;
        Vec3 worldScale = this.lossyScale;

        this.parent = newParent;

        if (newParent != null)
        {
            newParent.AddChildren(this);

            this.localPosition = newParent.InverseTransformPoint(worldPosition);
            this.localRotation = My_Quaternion.Inverse(newParent.rotation) * worldRotation;
            this.localScale = Vec3.Divide(worldScale, newParent.lossyScale);
        }

        hasChanged = true;
        UpdateMatrix();
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

    public void LookAt(Vec3 targetPosition)
    {
        Vec3 direction = (targetPosition - position).normalized;

        My_Quaternion newRotation = My_Quaternion.LookRotation(direction, Vec3.Up);

        rotation = newRotation;

        if (parent != null)
        {
            localRotation = My_Quaternion.Inverse(parent.rotation) * rotation;
        }
        else
        {
            localRotation = rotation;
        }

        hasChanged = true;

        UpdateMatrix();
    }

    public void LookAt(Vec3 targetPosition, Vec3 worldUp)
    {
        Vec3 direction = (targetPosition - position).normalized;

        Vec3 up = worldUp.normalized;

        My_Quaternion newRotation = My_Quaternion.LookRotation(direction, up);

        rotation = newRotation;

        if (parent != null)
        {
            localRotation = My_Quaternion.Inverse(parent.rotation) * rotation;
        }
        else
        {
            localRotation = rotation;
        }

        hasChanged = true;

        UpdateMatrix();
    }

    public void LookAt(My_Transform target)
    {
        LookAt(target.position);
    }

    public void LookAt(Transform target)
    {
        LookAt(target.position);
    }

    public void LookAt(My_Transform target, Vec3 worldUp)
    {
        LookAt(target.position, worldUp);
    }

    public void LookAt(Transform target, Vec3 worldUp)
    {
        LookAt(target.position, worldUp);
    }

    public void Rotate(Vec3 eulers, Space relativeTo)
    {
        My_Quaternion eulerRot = My_Quaternion.Euler(eulers.x, eulers.y, eulers.z);


        if (relativeTo == Space.Self)
        {
            localRotation *= eulerRot;

            if (parent != null)
            {
                rotation = parent.rotation * localRotation;
            }
            else
            {
                rotation = localRotation;
            }
        }
        else
        {
            rotation = rotation * (My_Quaternion.Inverse(rotation) * eulerRot * rotation);

            if (parent != null)
            {
                localRotation = My_Quaternion.Inverse(parent.rotation) * rotation;
            }
            else
            {
                localRotation = rotation;
            }
        }

        UpdateMatrix();
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
        axis = axis.normalized;

        My_Quaternion newRotation = My_Quaternion.AngleAxis(angle, axis);

        Vec3 localPoint = position - point;

        Vec3 rotatedPoint = newRotation * localPoint;

        position = rotatedPoint + point;

        this.rotation = newRotation * this.rotation;

        UpdateMatrix();
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

    public Vec3 InverseTransformDirection(Vec3 direction)
    {
        return My_Quaternion.Inverse(rotation) * direction;
    }

    public Vec3 InverseTransformDirection(float x, float y, float z)
    {
        return InverseTransformDirection(new Vec3(x, y, z));
    }

    public Vec3 InverseTransformPoint(Vec3 position)
    {
        Vec3 transformedPoint = position - localPosition;
        transformedPoint = My_Quaternion.Inverse(rotation) * transformedPoint;
        transformedPoint = Vec3.Scale(transformedPoint, Vec3.Inverse(localScale));

        return transformedPoint;
    }

    public Vec3 InverseTransformPoint(float x, float y, float z)
    {
        return InverseTransformPoint(x, y, z);
    }

    public Vec3 InverseTransformVector(Vec3 vector)
    {
        Vec3 transformedVector = My_Quaternion.Inverse(rotation) * vector;
        transformedVector = Vec3.Scale(transformedVector, Vec3.Inverse(localScale));

        return transformedVector;
    }

    public Vec3 InverseTransformVector(float x, float y, float z)
    {
        return InverseTransformVector(x, y, z);
    }

    private void UpdateLocalToWorldMatrix()
    {
        localToWorldMatrix = My_Matrix4x4.TRS(localPosition, localRotation, localScale);

        localEulerAngles = localRotation.EulerAngles;

        hasChanged = true;
    }

    private void UpdateWorldToLocalMatrix()
    {
        if (parent != null)
        {
            position = parent.position + (parent.rotation * Vec3.Scale(localPosition, parent.lossyScale));

            rotation = parent.rotation * localRotation;
        }
        else
        {
            worldToLocalMatrix = localToWorldMatrix.inverse;
        }

        eulerAngles = rotation.EulerAngles;

        hasChanged = true;

        UpdateChildrens();
    }

    private void UpdateMatrix()
    {
        UpdateLocalToWorldMatrix();
        UpdateWorldToLocalMatrix();
    }

    private void UpdateChildrens()
    {
        foreach (My_Transform child in childrens)
        {
            if (child != null)
            {
                child.UpdateWorldToLocalMatrix();
            }
        }
    }
}
