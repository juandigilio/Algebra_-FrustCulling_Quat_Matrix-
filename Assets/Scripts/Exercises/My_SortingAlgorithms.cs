using System;
using System.Collections.Generic;

public static class My_SortingAlgorithms
{
    private static void Swap<T>(List<T> array, int i, int j)
    {
        T temp = array[i];
        array[i] = array[j];
        array[j] = temp;
    }

    public static void BitonicSort<T>(List<T> array) where T : IComparable<T>
    {
        void BitonicMerge(int low, int count, bool ascending)
        {
            if (count > 1)
            {
                int mid = count / 2;
                for (int i = low; i < low + mid; i++)
                {
                    if ((array[i].CompareTo(array[i + mid]) > 0) == ascending)
                        Swap(array, i, i + mid);
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

    public static void SelectionSort<T>(List<T> array) where T : IComparable<T>
    {
        for (int i = 0; i < array.Count - 1; i++)
        {
            int minIndex = i;
            for (int j = i + 1; j < array.Count; j++)
                if (array[j].CompareTo(array[minIndex]) < 0)
                    minIndex = j;
            Swap(array, i, minIndex);
        }
    }

    public static void CocktailShakerSort<T>(List<T> array) where T : IComparable<T>
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
                }
            }
            for (int i = array.Count - 2; i >= 0; i--)
            {
                if (array[i].CompareTo(array[i + 1]) > 0)
                {
                    Swap(array, i, i + 1);
                    swapped = true;
                }
            }
        } while (swapped);
    }

    public static void QuickSort<T>(List<T> array) where T : IComparable<T>
    {
        void Sort(int low, int high)
        {
            if (low < high)
            {
                int pivot = Partition(low, high);
                Sort(low, pivot - 1);
                Sort(pivot + 1, high);
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

    public static void RadixSortLSD(List<int> array)
    {
        int GetDigit(int number, int place) => (number / place) % 10;

        int max = array[0];
        foreach (var num in array)
            max = Math.Max(max, num);

        for (int place = 1; max / place > 0; place *= 10)
        {
            var buckets = new List<int>[10];
            for (int i = 0; i < 10; i++) buckets[i] = new List<int>();

            foreach (var num in array)
                buckets[GetDigit(num, place)].Add(num);

            int index = 0;
            foreach (var bucket in buckets)
                foreach (var num in bucket)
                    array[index++] = num;
        }
    }

    public static void ShellSort<T>(List<T> array) where T : IComparable<T>
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
                }
                array[j] = temp;
            }
            gap /= 2;
        }
    }

    public static void BogoSort<T>(List<T> array) where T : IComparable<T>
    {
        bool IsSorted()
        {
            for (int i = 1; i < array.Count; i++)
                if (array[i - 1].CompareTo(array[i]) > 0)
                    return false;
            return true;
        }

        var random = new Random();
        while (!IsSorted())
        {
            for (int i = 0; i < array.Count; i++)
                Swap(array, i, random.Next(array.Count));
        }
    }

    public static void RadixSortMSD(List<int> array)
    {
        void Sort(List<int> array, int digit)
        {
            if (array.Count <= 1 || digit < 0) return;

            var buckets = new List<int>[10];
            for (int i = 0; i < 10; i++) buckets[i] = new List<int>();

            foreach (var num in array)
            {
                int bucketIndex = (num / (int)Math.Pow(10, digit)) % 10;
                buckets[bucketIndex].Add(num);
            }

            array.Clear();
            for (int i = 0; i < 10; i++)
            {
                Sort(buckets[i], digit - 1);
                array.AddRange(buckets[i]);
            }
        }

        int max = array[0];
        foreach (var num in array)
            max = Math.Max(max, num);

        int maxDigits = (int)Math.Log10(max) + 1;
        Sort(array, maxDigits - 1);
    }

    public static void IntroSort<T>(List<T> array) where T : IComparable<T>
    {
        void Sort(int low, int high, int depthLimit)
        {
            if (low >= high) return;

            if (depthLimit == 0)
            {
                HeapSort(array, low, high);
            }
            else
            {
                int pivot = Partition(low, high);
                Sort(low, pivot - 1, depthLimit - 1);
                Sort(pivot + 1, high, depthLimit - 1);
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

    public static void AdaptiveMergeSort<T>(List<T> array) where T : IComparable<T>
    {
        void Merge(List<T> left, List<T> right, List<T> result)
        {
            int i = 0, j = 0;
            while (i < left.Count && j < right.Count)
            {
                if (left[i].CompareTo(right[j]) <= 0)
                    result.Add(left[i++]);
                else
                    result.Add(right[j++]);
            }
            result.AddRange(left.GetRange(i, left.Count - i));
            result.AddRange(right.GetRange(j, right.Count - j));
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

        Sort(array, new List<T>());
    }

    public static void BubbleSort<T>(List<T> array) where T : IComparable<T>
    {
        for (int i = 0; i < array.Count - 1; i++)
        {
            for (int j = 0; j < array.Count - i - 1; j++)
            {
                if (array[j].CompareTo(array[j + 1]) > 0)
                {
                    Swap(array, j, j + 1);
                }
            }
        }
    }

    public static void GnomeSort<T>(List<T> array) where T : IComparable<T>
    {
        int index = 0;
        while (index < array.Count)
        {
            if (index == 0 || array[index].CompareTo(array[index - 1]) >= 0)
                index++;
            else
            {
                Swap(array, index, index - 1);
                index--;
            }
        }
    }

    public static void MergeSort<T>(List<T> array) where T : IComparable<T>
    {
        void Merge(List<T> left, List<T> right, List<T> result)
        {
            int i = 0, j = 0;
            while (i < left.Count && j < right.Count)
            {
                if (left[i].CompareTo(right[j]) <= 0)
                    result.Add(left[i++]);
                else
                    result.Add(right[j++]);
            }
            result.AddRange(left.GetRange(i, left.Count - i));
            result.AddRange(right.GetRange(j, right.Count - j));
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

        Sort(array, new List<T>());
    }

    private static void HeapSort<T>(List<T> array, int low, int high) where T : IComparable<T>
    {
        void Heapify(int start, int end, int root)
        {
            int largest = root;
            int left = 2 * (root - start) + 1 + start; // Ajustar índices al rango
            int right = 2 * (root - start) + 2 + start; // Ajustar índices al rango

            if (left <= end && array[left].CompareTo(array[largest]) > 0)
                largest = left;

            if (right <= end && array[right].CompareTo(array[largest]) > 0)
                largest = right;

            if (largest != root)
            {
                Swap(array, root, largest);
                Heapify(start, end, largest);
            }
        }

        // Construir el heap en el rango especificado
        for (int i = (low + (high - low + 1) / 2) - 1; i >= low; i--)
            Heapify(low, high, i);

        // Extraer elementos del heap
        for (int i = high; i > low; i--)
        {
            Swap(array, low, i);
            Heapify(low, i - 1, low);
        }
    }

    public static void InsertionSort<T>(List<T> array) where T : IComparable<T>
    {
        for (int i = 1; i < array.Count; i++)
        {
            T key = array[i];
            int j = i - 1;
            while (j >= 0 && array[j].CompareTo(key) > 0)
            {
                array[j + 1] = array[j];
                j--;
            }
            array[j + 1] = key;
        }
    }
}
