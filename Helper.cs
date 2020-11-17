using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimalSimulationVersion2
{
    public class Helper : IHelper
    {
        private static Random rnd;
        private static List<string> ids;
        private ulong idNumber = 0;
        /// <summary>
        /// Get a reference to the instance of Helper.
        /// </summary>
        public static IHelper Instance { get; }
        static Helper()
        {
            rnd = new Random();
            ids = new List<string>();
            Instance = new Helper();
        }
        private Helper() { }

        public T[] DeepCopy<T>(T[] array)
        {
            if (array == null)
                return null;
            T[] newArray = new T[array.Length];
            for (int i = 0; i < newArray.Length; i++)
                newArray[i] = array[i];
            return newArray;
        }

        public List<T> DeepCopy<T>(List<T> list)
        {
            if (list == null)
                return null;
            List<T> newList = new List<T>();
            for (int i = 0; i < list.Count; i++)
                newList.Add(list[i]);
            return newList;
        }
        public void Add<T>(ref T[] array, T value)
        {
            if (array != null)
            {
                List<T> list = array.ToList();
                list.Add(value);
                array = list.ToArray();
            }
        }

        public void Add<T>(List<T> list, T value) => list?.Add(value);
        
        public string GenerateID()
        {
            idNumber++;
            return idNumber.ToString();
        }

        public void Remove<T>(List<T> list, T value) => list?.Remove(value);

        public void Remove<T>(ref T[] array, T value)
        {
            if (array != null)
            {
                List<T> list = array.ToList();
                list.Remove(value);
                array = list.ToArray();
            }
        }

        public bool Contains<T>(List<T> list, T value) //https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.equalitycomparer-1.default?view=netcore-3.1
        {
            if (list == null)
                return false;
            foreach (T val in list)
                if (EqualityComparer<T>.Default.Equals(val,value))
                    return true;
            return false;
        }

        public bool Contains<T>(T[] array, T value)
        {
            if (array == null)
                return false;
            foreach (T val in array)
                if (EqualityComparer<T>.Default.Equals(val, value))
                    return true;
            return false;
        }
        public int GenerateRandomNumber(int minimum, int maximum)
        {
            return rnd.Next(minimum, maximum + 1);
        }

        public (T, T) DeepCopy<T>((T, T) value)
        {
            T value1 = value.Item1;
            T value2 = value.Item2;
            return (value1, value2);
        }

    }
}
