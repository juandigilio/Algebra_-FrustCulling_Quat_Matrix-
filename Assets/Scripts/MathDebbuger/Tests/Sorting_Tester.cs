using UnityEngine;
using System.Collections.Generic;


public enum Algorithms
{
    BitonicSort = 1,
    SelectionSort = 2,
    CocktailShakerSort = 3,
    QuickSort = 4,
    RadixSortLSD = 5,
    ShellSort = 6,
    BogoSort = 7,
    RadixSortMSD = 8,
    IntroSort = 9,
    AdaptiveMergeSort = 10,
    BubbleSort = 11,
    GnomeSort = 12,
    MergeSort = 13,
    HeapSort = 14,
    InsertionSort = 15
}


//BitonicSort
//RadixLSD
//AdaptiveMerge
//Merge
//Heap
//Intro


//RadixMSD



public class Sorting_Tester : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int objectsQnty;
    [SerializeField] private Algorithms algorithms;
    [SerializeField] public float delay = 10f;

    private List<GameObject> objects = new List<GameObject>();
    private List<float> heights = new List<float>();

    private float screenSize;
    private Algorithms currentAlgorithm;

    void Start()
    {
        screenSize = Camera.main.orthographicSize * 2 * Camera.main.aspect;
        float width = screenSize / objectsQnty;

        for (int i = 0; i < objectsQnty; i++)
        {
            GenerateInstance(width);
        }

        prefab.SetActive(false);

        AlignObjectsInRow();

        currentAlgorithm = algorithms;
        SetFrameRate();
    }

    void Update()
    {
        if (currentAlgorithm != algorithms)
        {
            currentAlgorithm = algorithms;

            ResetHeights();

            SetFrameRate();
        }

        switch (algorithms)
        {
            case Algorithms.BitonicSort:
                {
                    //StartCoroutine(My_SortingAlgorithms<float>.BitonicSort(heights, delay));
                    My_SortingAlgorithms<float>.BitonicSort(heights, delay);
                    break;
                }
            case Algorithms.SelectionSort:
                {
                    StartCoroutine(My_SortingAlgorithms<float>.SelectionSort(heights, delay));
                    break;
                }
            case Algorithms.BubbleSort:
                {
                    StartCoroutine(My_SortingAlgorithms<float>.BubbleSort(heights, delay));
                    break;
                }
            case Algorithms.CocktailShakerSort:
                {
                    StartCoroutine(My_SortingAlgorithms<float>.CocktailShakerSort(heights, delay));
                    break;
                }
            case Algorithms.QuickSort:
                {
                    StartCoroutine(My_SortingAlgorithms<float>.QuickSort(heights, delay));
                    break;
                }
            case Algorithms.RadixSortLSD:
                {
                    //StartCoroutine(My_SortingAlgorithms<float>.RadixSortLSD(heights, delay, x => x));
                    My_SortingAlgorithms<float>.RadixSortLSD(heights, delay, x => x);
                    break;
                }
            case Algorithms.ShellSort:
                {
                    StartCoroutine(My_SortingAlgorithms<float>.ShellSort(heights, delay));
                    break;
                }
            case Algorithms.BogoSort:
                {
                    StartCoroutine(My_SortingAlgorithms<float>.BogoSort(heights, delay));
                    break;
                }
            case Algorithms.RadixSortMSD:
                {
                    //StartCoroutine(My_SortingAlgorithms<float>.RadixSortMSD(heights, delay, x => (int)(x * 10)));
                    My_SortingAlgorithms<float>.RadixSortMSD(heights, delay, x => (int)(x * 10));
                    break;
                }
            case Algorithms.IntroSort:
                {
                    //StartCoroutine(My_SortingAlgorithms<float>.IntroSort(heights, delay));
                    My_SortingAlgorithms<float>.IntroSort(heights, delay);
                    break;
                }
            case Algorithms.AdaptiveMergeSort:
                {
                    My_SortingAlgorithms<float>.AdaptiveMergeSort(heights, delay);
                    break;
                }
            case Algorithms.GnomeSort:
                {
                    StartCoroutine(My_SortingAlgorithms<float>.GnomeSort(heights, delay));
                    break;
                }
            case Algorithms.MergeSort:
                {
                    //StartCoroutine(My_SortingAlgorithms<float>.MergeSort(heights, delay));
                    My_SortingAlgorithms<float>.MergeSort(heights, delay);
                    break;
                }
            case Algorithms.HeapSort:
                {
                    //StartCoroutine(My_SortingAlgorithms<float>.HeapSort(heights, delay));
                    My_SortingAlgorithms<float>.HeapSort(heights, delay);
                    break;
                }
            case Algorithms.InsertionSort:
                {
                    StartCoroutine(My_SortingAlgorithms<float>.InsertionSort(heights, delay));
                    break;
                }
        }

        SetTransformsScale();
    }

    void SetTransformsScale()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].transform.localScale = new Vector3(objects[i].transform.localScale.x, heights[i], 1f);
            objects[i].transform.position = new Vector3(objects[i].transform.position.x, (-4f + objects[i].transform.localScale.y / 2), 0);

            float redValue = Mathf.Clamp01(heights[i] / 10f);
            objects[i].GetComponent<Renderer>().material.color = new Color(1f, 1f - redValue, 1f - redValue);
        }
    }

    public void GenerateInstance(float width)
    {
        GameObject temporalInstance = Instantiate(prefab, Vector3.zero, Quaternion.identity);

        temporalInstance.transform.localScale = new Vector3(width, 1f, 1f);

        objects.Add(temporalInstance);
        heights.Add(Random.Range(1f, 10f));
    }

    void AlignObjectsInRow()
    {
        float currentX = -screenSize / 2f;
        float step = objects[0].transform.localScale.x;

        foreach (GameObject obj in objects)
        {
            obj.transform.position = new Vector3(currentX + step, (-10f + obj.transform.localScale.y * 2), 0);
            currentX += step;
        }
    }

    void ResetHeights()
    {
        StopAllCoroutines();
        
        heights.Clear();

        for (int i = 0; i < objects.Count; i++)
        {
            heights.Add(Random.Range(1f, 10f));
        }

        SetTransformsScale();
    }

    void SetFrameRate()
    {
        if (algorithms == Algorithms.BitonicSort)
        {
            Application.targetFrameRate = 90;
        }
        else
        {
            Application.targetFrameRate = 60;
        }
    }
}