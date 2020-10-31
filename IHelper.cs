using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalSimulationVersion2
{
    public interface IArraySupport
    {
        public List<T> DeepCopy<T>(List<T> list);
        public T[] DeepCopy<T>(T[] array);
        public List<T> Add<T>(List<T> list, T value);
        public T[] Add<T>(T[] array, T value);
    }
}
