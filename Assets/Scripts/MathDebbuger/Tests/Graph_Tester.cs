using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public enum Methods
{
    All,
    Any,
    Contains,
    Distinct,
    ElementAt,
    Except,
    First,
    Last,
    Intersect,
    Count,
    SequenceEqual,
    Single,
    SkipWhile,
    Union,
    Where
}

public class Graph_Tester : MonoBehaviour
{
    [SerializeField] private List<int> source1 = new List<int>();
    [SerializeField] private List<int> source2 = new List<int>();
    [SerializeField] private Methods method;
    [SerializeField] private int tester;
    [SerializeField] private bool refresh;

    private Methods lastMethod;


    void Start()
    {
        TestMethod(method);
        refresh = false;
    }

    void Update()
    {
        if (lastMethod != method || refresh == true)
        {
            refresh = false;
            lastMethod = method;
            TestMethod(method);
        }
    }

    private void TestMethod(Methods newMethod)
    {
        switch (newMethod)
        {
            case Methods.All:
                TestAll();
                break;
            case Methods.Any:
                TestAny();
                break;
            case Methods.Contains:
                TestContains();
                break;
            case Methods.Distinct:
                TestDistinct();
                break;
            case Methods.ElementAt:
                TestElementAt();
                break;
            case Methods.Except:
                TestExcept();
                break;
            case Methods.First:
                TestFirst();
                break;
            case Methods.Last:
                TestLast();
                break;
            case Methods.Intersect:
                TestIntersect();
                break;
            case Methods.Count:
                TestCount();
                break;
            case Methods.SequenceEqual:
                TestSequenceEqual();
                break;
            case Methods.Single:
                TestSingle();
                break;
            case Methods.SkipWhile:
                TestSkipWhile();
                break;
            case Methods.Union:
                TestUnion();
                break;
            case Methods.Where:
                TestWhere();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void TestAll()
    {
        Debug.Log($"All are {tester.ToString()}: {GraphMethods.All(source1, i => i == tester)}");
    }

    private void TestAny()
    {
        Debug.Log($"Any is {tester.ToString()}: {GraphMethods.Any(source1, i => i == tester)}");
    }

    private void TestContains()
    {
        Debug.Log($"Contains {tester.ToString()}: {GraphMethods.Contains(source1, tester)}");
    }

    private void TestDistinct()
    {
        var distinctInts = GraphMethods.ToList(GraphMethods.Distinct(source1));

        var logText = "Distincts: ";

        foreach (var element in distinctInts)
        {
            logText += $"{element}, ";
        }

        Debug.Log(logText);
    }

    private void TestElementAt()
    {
        Debug.Log($"Element at: {tester}: {GraphMethods.ElementAt(source1, tester)}");
    }

    private void TestExcept()
    {
        var exceptsList = GraphMethods.ToList(GraphMethods.Except(source1, source2));
        string logText = "Excepts: ";

        foreach (var element in exceptsList)
        {
            logText += $"{element}, ";
        }

        Debug.Log(logText);
    }

    private void TestFirst()
    {
        Debug.Log($"First equal value: {GraphMethods.First(source1, i => i == tester)}");
    }

    private void TestLast()
    {
        Debug.Log($"Last equal value: {GraphMethods.Last(source1, i => i == tester)}");
    }

    private void TestIntersect()
    {
        var intersectList = GraphMethods.ToList(GraphMethods.Intersect(source1, source2));
        string logText = "Intersects: ";

        foreach (var element in intersectList)
        {
            logText += $"{element}, ";
        }

        Debug.Log(logText);
    }

    private void TestCount()
    {
        Debug.Log($"Count of {tester}: {GraphMethods.Count(source1, i => i == tester)}");
    }

    private void TestSequenceEqual()
    {
        Debug.Log($"Are Equal: {GraphMethods.SequenceEqual(source1, source2, EqualityComparer<int>.Default)}");
    }

    private void TestSingle()
    {
        Debug.Log($"Are only one {tester}: {GraphMethods.Single(source1, i => i == tester)}");
    }

    private void TestSkipWhile()
    {
        var resultList = GraphMethods.ToList(GraphMethods.SkipWhile(source1, i => i == tester));

        string logText = "SkipWhile result: ";

        foreach (var element in resultList)
        {
            logText += $"{element}, ";
        }

        Debug.Log(logText);
    }

    private void TestUnion()
    {
        var resultList = GraphMethods.ToList(GraphMethods.Union(source1, source2));
        string logText = "Union result: ";

        foreach (var element in resultList)
        {
            logText += $"{element}, ";
        }

        Debug.Log(logText);
    }

    private void TestWhere()
    {
        var resultList = GraphMethods.ToList(GraphMethods.Where(source1, i => i == tester));

        string logText = "Where result: ";

        foreach (var element in resultList)
        {
            logText += $"{element}, ";
        }

        Debug.Log(logText);
    }
}
