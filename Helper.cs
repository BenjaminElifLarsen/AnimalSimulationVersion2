using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimalSimulationVersion2
{
    public class Helper : IHelper
    {
        public static IHelper Instance { get; }
        static Helper()
        {
            Instance = new Helper();
        }
        private Helper() { }


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
        public void Add<T>(T[] array, T value)
        {
            List<T> list = array.ToList();
            list.Add(value);
            array = list.ToArray();
        }

        public void Add<T>(List<T> list, T value)
        {
            list.Add(value);
        }

        public string GenerateID()
        {
            throw new NotImplementedException();
        }

        public void Remove<T>(List<T> list, T value)
        {
            throw new NotImplementedException();
        }

        public void Remove<T>(T[] array, T value)
        {
            throw new NotImplementedException();
        }

        public bool Contains<T>(List<T> list, T value)
        {
            throw new NotImplementedException();
        }

        public bool Contains<T>(T[] array, T value)
        {
            throw new NotImplementedException();
        }
    }
}
