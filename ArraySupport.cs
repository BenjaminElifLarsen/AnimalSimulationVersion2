using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimalSimulationVersion2
{
    public class ArraySupport : IArraySupport
    {
        public static IArraySupport Instance { get; }
        static ArraySupport()
        {
            Instance = new ArraySupport();
        }
        private ArraySupport() { }


        public T[] DeepCopy<T>(T[] array)
        {
            T[] newArray = new T[array.Length];
            for (int i = 0; i < newArray.Length; i++)
                newArray[i] = array[i];
            return newArray;
        }

        public List<T> DeepCopy<T>(List<T> list)
        {
            List<T> newList = new List<T>();
            for (int i = 0; i < list.Count; i++)
                newList.Add(list[i]);
            return newList;
        }
        public T[] Add<T>(T[] array, T value)
        {
            List<T> list = array.ToList();
            list.Add(value);
            return list.ToArray();
        }

        public List<T> Add<T>(List<T> list, T value)
        {
            list.Add(value);
            return list;
        }
    }
}
