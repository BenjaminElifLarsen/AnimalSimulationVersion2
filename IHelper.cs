using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalSimulationVersion2
{
    public interface IHelper
    {
        public List<T> DeepCopy<T>(List<T> list);
        public T[] DeepCopy<T>(T[] array);
        public void Add<T>(List<T> list, T value);
        public void Add<T>(T[] array, T value);
        public void Remove<T>(List<T> list, T value);
        public void Remove<T>(T[] array, T value);
        public bool Contains<T>(List<T> list, T value);
        public bool Contains<T>(T[] array, T value);
        public string GenerateID(); 
    }
}
