using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class My_SortingAlgorithms<T> where T : IComparable<T>
{
    private static void Swap(List<T> array, int i, int j)
    {
        T temp = array[i];
        array[i] = array[j];
        array[j] = temp;
    }

    public static void BitonicSort(List<T> array, float delay)
    {
        IEnumerator BitonicMerge(int low, int count, bool ascending)
        {
            if (count > 1)
            {
                int mid = count / 2;

                for (int i = low; i < low + mid; i++)
                {
                    if ((array[i].CompareTo(array[i + mid]) > 0) == ascending)
                    {
                        Swap(array, i, i + mid);
                    }

                    yield return new WaitForSeconds(delay);
                }

                BitonicMerge(low, mid, ascending);
                BitonicMerge(low + mid, mid, ascending);
            }
        }

        void RecursiveSort(int low, int count, bool ascending)
        {
            if (count > 1)
            {
                int mid = count / 2;
                RecursiveSort(low, mid, true);
                RecursiveSort(low + mid, mid, false);
                BitonicMerge(low, count, ascending);
            }
        }

        RecursiveSort(0, array.Count, true);
    }

    public static IEnumerator SelectionSort(List<T> array, float delay)
    {
        for (int i = 0; i < array.Count - 1; i++)
        {
            int minIndex = i;

            for (int j = i + 1; j < array.Count; j++)
            {
                if (array[j].CompareTo(array[minIndex]) < 0)
                {
                    minIndex = j;
                }
            }
                
            Swap(array, i, minIndex);

            yield return new WaitForSeconds(delay);
        }
    }

    public static IEnumerator CocktailShakerSort(List<T> array, float delay)
    {
        bool swapped;
        do
        {
            swapped = false;

            for (int i = 0; i < array.Count - 1; i++)
            {
                if (array[i].CompareTo(array[i + 1]) > 0)
                {
                    Swap(array, i, i + 1);
                    swapped = true;
                    yield return new WaitForSeconds(delay);
                }
            }
            for (int i = array.Count - 2; i >= 0; i--)
            {
                if (array[i].CompareTo(array[i + 1]) > 0)
                {
                    Swap(array, i, i + 1);
                    swapped = true;
                    yield return new WaitForSeconds(delay);
                }
            }

        } while (swapped);
    }

    public static void QuickSort(List<T> array, float delay)
    {
        IEnumerator Sort(int low, int high)
        {
            if (low < high)
            {
                int pivot = Partition(low, high);
                Sort(low, pivot - 1);
                Sort(pivot + 1, high);

                yield return new WaitForSeconds(delay);
            }
        }

        int Partition(int low, int high)
        {
            T pivot = array[high];
            int i = low;

            for (int j = low; j < high; j++)
            {
                if (array[j].CompareTo(pivot) < 0)
                {
                    Swap(array, i, j);
                    i++;
                }
            }

            Swap(array, i, high);
            return i;
        }

        Sort(0, array.Count - 1);
    }

    public static IEnumerator RadixSortLSD(List<T> array, float delay, Func<T, float> getKey, int precision = 1000)
    {
        int GetDigit(int number, int place) => (number / place) % 10;

        List<int> scaledArray = new List<int>();

        foreach (var item in array)
        {
            scaledArray.Add((int)(getKey(item) * precision));
        }

        int maxKey = scaledArray[0];
        foreach (var key in scaledArray)
        {
            maxKey = Math.Max(maxKey, key);
        }

        for (int place = 1; maxKey / place > 0; place *= 10)
        {
            var buckets = new List<T>[10];
            for (int i = 0; i < 10; i++)
            {
                buckets[i] = new List<T>();
            }

            for (int i = 0; i < array.Count; i++)
            {
                int digit = GetDigit(scaledArray[i], place);
                buckets[digit].Add(array[i]);
            }

            int index = 0;
            foreach (var bucket in buckets)
            {
                foreach (var item in bucket)
                {
                    array[index] = item;
                    scaledArray[index++] = (int)(getKey(item) * precision);
                }
            }

            yield return new WaitForSeconds(delay);
        }
    }

    public static IEnumerator ShellSort(List<T> array, float delay)
    {
        int gap = array.Count / 2;

        while (gap > 0)
        {
            for (int i = gap; i < array.Count; i++)
            {
                T temp = array[i];
                int j = i;

                while (j >= gap && array[j - gap].CompareTo(temp) > 0)
                {
                    array[j] = array[j - gap];
                    j -= gap;
                    yield return new WaitForSeconds(delay);
                }

                array[j] = temp;
            }

            gap /= 2;
        }
    }

    public static IEnumerator BogoSort(List<T> array, float delay)
    {
        bool IsSorted()
        {
            for (int i = 1; i < array.Count; i++)
            {
                if (array[i - 1].CompareTo(array[i]) > 0)
                {
                    return false;
                }
            }
            return true;
        }

        var random = new System.Random();

        while (!IsSorted())
        {
            for (int i = 0; i < array.Count; i++)
            {
                Swap(array, i, random.Next(array.Count));

                yield return new WaitForSeconds(delay);
            }
        }
    }

    public static IEnumerator RadixSortMSD(List<T> array, float delay, System.Func<T, int> keySelector)
    {
        IEnumerator Sort(List<T> array, int digit)
        {
            if (array.Count <= 1 || digit < 0)
            {
                yield return new WaitForSeconds(delay);
                yield break;
            }

            var buckets = new List<T>[10];

            for (int i = 0; i < 10; i++)
            {
                buckets[i] = new List<T>();
            }

            foreach (var item in array)
            {
                int bucketIndex = (keySelector(item) / (int)Math.Pow(10, digit)) % 10;
                buckets[bucketIndex].Add(item);
            }

            array.Clear();

            for (int i = 0; i < 10; i++)
            {
                yield return Sort(buckets[i], digit - 1);
                array.AddRange(buckets[i]);
                yield return new WaitForSeconds(delay);
            }
        }

        int max = keySelector(array[0]);
        foreach (var item in array)
        {
            max = Math.Max(max, keySelector(item));
        }

        int maxDigits = (int)Math.Log10(max) + 1;

        yield return Sort(array, maxDigits - 1);
    }

    public static void IntroSort(List<T> array, float delay)
    {
        IEnumerator Sort(int low, int high, int depthLimit)
        {
            if (low >= high)
            {
                yield return new WaitForSeconds(delay);
            }

            if (depthLimit == 0)
            {
                HeapSort(array, delay);
            }
            else
            {
                int pivot = Partition(low, high);
                Sort(low, pivot - 1, depthLimit - 1);
                yield return new WaitForSeconds(delay);
                Sort(pivot + 1, high, depthLimit - 1);
                yield return new WaitForSeconds(delay);
            }
        }

        int Partition(int low, int high)
        {
            T pivot = array[high];
            int i = low;

            for (int j = low; j < high; j++)
            {
                if (array[j].CompareTo(pivot) < 0)
                {
                    Swap(array, i, j);
                    i++;
                }
            }

            Swap(array, i, high);

            return i;
        }

        int depthLimit = (int)(2 * Math.Log(array.Count));

        Sort(0, array.Count - 1, depthLimit);
    }

    public static void AdaptiveMergeSort(List<T> array, float delay)
    {
        IEnumerator Merge(List<T> left, List<T> right, List<T> array)
        {
            int i = 0, j = 0;

            while (i < left.Count && j < right.Count)
            {
                if (left[i].CompareTo(right[j]) <= 0)
                {
                    array.Add(left[i++]);
                }
                else
                {
                    array.Add(right[j++]);
                }

                yield return new WaitForSeconds(delay);
            }

            array.AddRange(left.GetRange(i, left.Count - i));
            array.AddRange(right.GetRange(j, right.Count - j));
        }

        void Sort(List<T> arrayCopy, List<T> array)
        {
            if (arrayCopy.Count <= 1)
            {
                return;
            }

            int mid = arrayCopy.Count / 2;
            var left = arrayCopy.GetRange(0, mid);
            var right = arrayCopy.GetRange(mid, arrayCopy.Count - mid);

            Sort(left, array);
            Sort(right, array);

            array.Clear();

            Merge(left, right, array);

            //arrayCopy.Clear();
            //arrayCopy.AddRange(array);
        }

        List<T> arrayCopy = new List<T>(array);

        Sort(arrayCopy, array);
    }

    public static IEnumerator BubbleSort(List<T> array, float delay)
    {
        for (int i = 0; i < array.Count - 1; i++)
        {
            for (int j = 0; j < array.Count - i - 1; j++)
            {
                if (array[j].CompareTo(array[j + 1]) > 0)
                {
                    Swap(array, j, j + 1);
                }

                yield return new WaitForSeconds(delay);
            }
        }
    }

    public static IEnumerator GnomeSort(List<T> array, float delay)
    {
        int index = 0;

        while (index < array.Count)
        {
            if (index == 0 || array[index].CompareTo(array[index - 1]) >= 0)
            {
                index++;
            }
            else
            {
                Swap(array, index, index - 1);
                index--;
            }

            yield return new WaitForSeconds(delay);
        }
    }

    public static void MergeSort(List<T> array, float delay)
    {
        IEnumerator Merge(List<T> left, List<T> right, List<T> array)
        {
            int i = 0, j = 0;
            while (i < left.Count && j < right.Count)
            {
                if (left[i].CompareTo(right[j]) <= 0)
                {
                    array.Add(left[i++]);
                }
                else
                {
                    array.Add(right[j++]);
                }

                yield return new WaitForSeconds(delay);
            }
            array.AddRange(left.GetRange(i, left.Count - i));
            array.AddRange(right.GetRange(j, right.Count - j));
        }

        void Sort(List<T> array, List<T> temp)
        {
            if (array.Count <= 1) return;

            int mid = array.Count / 2;
            var left = array.GetRange(0, mid);
            var right = array.GetRange(mid, array.Count - mid);

            Sort(left, temp);
            Sort(right, temp);

            temp.Clear();
            Merge(left, right, temp);
            array.Clear();
            array.AddRange(temp);
        }

        List<T> arrayCopy = new List<T>(array);
        Sort(arrayCopy, array);
    }

    public static IEnumerator HeapSort(List<T> array, float delay)
    {
        IEnumerator Heapify(int start, int end, int root)
        {
            int largest = root;
            int left = 2 * root + 1;
            int right = 2 * root + 2;

            if (left < end && array[left].CompareTo(array[largest]) > 0)
            {
                largest = left;
            }

            if (right < end && array[right].CompareTo(array[largest]) > 0)
            {
                largest = right;
            }

            if (largest != root)
            {
                Swap(array, root, largest);
                yield return new WaitForSeconds(delay);
                yield return Heapify(start, end, largest);
            }
        }

        int n = array.Count;

        for (int i = (n / 2) - 1; i >= 0; i--)
        {
            yield return Heapify(0, n, i);
        }

        for (int i = n - 1; i > 0; i--)
        {
            Swap(array, 0, i);
            yield return new WaitForSeconds(delay);
            yield return Heapify(0, i, 0);
        }
    }

    public static IEnumerator InsertionSort(List<T> array, float delay)
    {
        for (int i = 1; i < array.Count; i++)
        {
            T key = array[i];
            int j = i - 1;

            while (j >= 0 && array[j].CompareTo(key) > 0)
            {
                array[j + 1] = array[j];
                j--;

                yield return new WaitForSeconds(delay);
            }

            array[j + 1] = key;
        }
    }
}