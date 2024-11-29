using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEngine;


public static class My_SortingAlgorithms<T> where T : IComparable<T>
{
    private static void Swap(List<T> array, int i, int j)
    {
        T temp = array[i];
        array[i] = array[j];
        array[j] = temp;
    }

    // BogoSort
    //0(n!)

    // BubbleSort
    //0(n^2)

    // CocktailShakerSort
    //0(n^2)

    // GnomeSort
    //0(n^2)

    // InsertionSort
    //0(n^2)

    // SelectionSort
    //0(n^2)

    // ShellSort
    //0(n^2)

    // BitonicSort
    //0(n log^2 n)

    // QuickSort
    //0(n log n)

    // MergeSort
    //0(n log n)

    // HeapSort
    //0(n log n)

    // RadixSortLSD
    //0(n)

    // RadixSortMSD
    //0(n)

    // IntroSort
    //0(n log n)

    // AdaptiveMergeSort
    //0(n log n)


    //Separo en secuencias bitonicas
    public static void BitonicSort(List<T> array, float delay)
    {
        void BitonicMerge(int low, int count, bool ascending)
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

                //ascendente
                RecursiveSort(low, mid, true);
                //descendente
                RecursiveSort(low + mid, mid, false);

                BitonicMerge(low, count, ascending);
            }
        }

        RecursiveSort(0, array.Count, true);
    }

    public static IEnumerator SelectionSort(List<T> array, float delay)
    {
        //find the minimumin element in the array and swap it with (i)
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

    public static IEnumerator QuickSort(List<T> array, float delay)
    {
        IEnumerator Sort(int low, int high)
        {
            yield return new WaitForSeconds(delay);

            if (low < high)
            {
                int pivot = Partition(low, high);
                yield return Sort(low, pivot - 1);
                yield return Sort(pivot + 1, high);

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

        yield return Sort(0, array.Count - 1);
    }

    public static void RadixSortLSD(List<T> array, float delay, Func<T, float> getKey, int precision = 1000)
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
            for (int i = 0; i < buckets.Length; i++)
            {
                foreach (var item in buckets[i])
                {
                    array[index] = item;
                    scaledArray[index++] = (int)(getKey(item) * precision);
                }
            }
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

    public static void RadixSortMSD(List<T> array, float delay)
    {
        void RecursiveRadixMSD(List<T> list, List<uint> ints, int from, int to, int digit)
        {
            if (to <= from)
            {
                return;
            }

            List<int>[] buckets = new List<int>[10];

            for (int i = 0; i < buckets.Length; i++)
            {
                buckets[i] = new List<int>();
            }

            for (int j = from; j <= to; j++)
            {
                int digitResult = (int)(ints[j] / Mathf.Pow(10, digit - 1) % 10);
                buckets[digitResult].Add(j);
            }

            List<T> auxList = new List<T>(list);
            List<uint> auxNumbers = new List<uint>(ints);

            int iterator = from;
            for (int bucketIndex = 0; bucketIndex < buckets.Length; bucketIndex++)
            {
                for (int i = 0; i < buckets[bucketIndex].Count; i++, iterator++)
                {
                    auxList[iterator] = list[buckets[bucketIndex][i]];
                    auxNumbers[iterator] = ints[buckets[bucketIndex][i]];
                }
            }

            for (int i = from; i <= to; i++)
            {
                list[i] = auxList[i];
                ints[i] = auxNumbers[i];
            }

            int prevPos = from;

            foreach (var element in buckets)
            {
                if (element.Count < 1)
                    continue;

                RecursiveRadixMSD(list, ints, prevPos, prevPos + element.Count - 1, digit - 1);
                prevPos += element.Count;
            }
        }

        int GetNumberOfDigits(int number)
        {
            if (number < 10)
                return 1;

            return 1 + GetNumberOfDigits(number / 10);
        }

        uint GetIntFromBitArray(List<uint> bits)
        {
            uint result = 0;
            for (int i = bits.Count - 1; i >= 0; i--)
            {
                result *= 2;
                result += bits[i];
            }

            return result;
        }

        List<uint> GetIntsFromT(List<T> list)
        {
            List<BitArray> bitArrays = new List<BitArray>();

            List<uint> ints = new List<uint>();
            for (int i = 0; i < list.Count; i++)
            {
                BinaryFormatter bf = new BinaryFormatter();
                MemoryStream ms = new MemoryStream();
                bf.Serialize(ms, list[i]);
                bitArrays.Add(new BitArray(ms.ToArray()));
            }

            for (int i = 0; i < list.Count; i++)
            {
                List<uint> bits = new List<uint>();
                for (int j = bitArrays[i].Count - 40; j < bitArrays[i].Count - 8; j++)
                {
                    bits.Add(Convert.ToUInt32(bitArrays[i][j]));
                }

                ints.Add(GetIntFromBitArray(bits));
            }

            return ints;
        }

        List<uint> ints = GetIntsFromT(array);

        uint biggestNum = ints[0];

        for (int i = 1; i < ints.Count; i++)
        {
            if (ints[i] > biggestNum)
                biggestNum = ints[i];
        }

        int maxDigits = GetNumberOfDigits(Convert.ToInt32(biggestNum));

        RecursiveRadixMSD(array, ints, 0, array.Count - 1, maxDigits);
    }

    public static void IntroSort(List<T> array, float delay)
    {
        void Sort(int low, int high, int depthLimit)
        {
            if (low >= high)
            {
                return;
            }

            if (depthLimit == 0)
            {
                HeapSort(array, delay);
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

    public static void AdaptiveMergeSort(List<T> array, float delay)
    {
        // Tamaño del segmento donde cambia a ordenamiento por inserción
        const int INSERTION_SORT_THRESHOLD = 16;

        void Merge(List<T> array, int left, int mid, int right)
        {
            int n1 = mid - left + 1;
            int n2 = right - mid;

            var leftArray = new T[n1];
            var rightArray = new T[n2];

            for (int i = 0; i < n1; i++) leftArray[i] = array[left + i];
            for (int j = 0; j < n2; j++) rightArray[j] = array[mid + 1 + j];

            int iL = 0, iR = 0, k = left;

            while (iL < n1 && iR < n2)
            {
                if (leftArray[iL].CompareTo(rightArray[iR]) <= 0)
                {
                    array[k++] = leftArray[iL++];
                }
                else
                {
                    array[k++] = rightArray[iR++];
                }
            }

            while (iL < n1) array[k++] = leftArray[iL++];
            while (iR < n2) array[k++] = rightArray[iR++];
        }

        void InsertionSort(List<T> array, int left, int right)
        {
            for (int i = left + 1; i <= right; i++)
            {
                var key = array[i];
                int j = i - 1;

                while (j >= left && array[j].CompareTo(key) > 0)
                {
                    array[j + 1] = array[j];
                    j--;
                }

                array[j + 1] = key;
            }
        }

        void Sort(List<T> array, int left, int right)
        {
            if (left >= right) return;

            if (right - left <= INSERTION_SORT_THRESHOLD)
            {
                InsertionSort(array, left, right);
                return;
            }

            int mid = left + (right - left) / 2;

            Sort(array, left, mid);
            Sort(array, mid + 1, right);

            Merge(array, left, mid, right);
        }

        Sort(array, 0, array.Count - 1);
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
        void Merge(List<T> left, List<T> right, List<T> array)
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

                //yield return new WaitForSeconds(delay);
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

    public static void HeapSort(List<T> array, float delay)
    {
        void Heapify(int end, int root)
        {
            int largest = root; // Inicialmente, la raíz es el nodo más grande
            int left = 2 * root + 1; // Hijo izquierdo
            int right = 2 * root + 2; // Hijo derecho

            // Verificar si el hijo izquierdo es más grande que la raíz
            if (left < end && array[left].CompareTo(array[largest]) > 0)
            {
                largest = left;
            }

            // Verificar si el hijo derecho es más grande que el más grande actual
            if (right < end && array[right].CompareTo(array[largest]) > 0)
            {
                largest = right;
            }

            // Si el más grande no es la raíz
            if (largest != root)
            {
                Swap(array, root, largest);

                // Aplicar recursivamente el heapify en el subárbol afectado
                Heapify(end, largest);
            }
        }

        int n = array.Count;

        // Construir el heap (reordenar el array)
        for (int i = n / 2 - 1; i >= 0; i--)
        {
            Heapify(n, i);
        }

        // Extraer los elementos del heap uno por uno
        for (int i = n - 1; i > 0; i--)
        {
            // Mover la raíz actual al final
            Swap(array, 0, i);

            // Llamar a Heapify en el heap reducido
            Heapify(i, 0);

            // Pausar para observar el cambio
            //yield return new WaitForSeconds(delay);
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