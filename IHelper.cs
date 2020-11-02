using System;
using System.Collections.Generic;
using System.Text;

namespace AnimalSimulationVersion2
{
    public interface IHelper
    {
        /// <summary>
        /// Returns a deep copy of <paramref name="list"/>.
        /// </summary>
        /// <typeparam name="T">The type of the list.</typeparam>
        /// <param name="list">The list to deep copy.</param>
        /// <returns>Returns a deep copy of <paramref name="list"/>.</returns>
        public List<T> DeepCopy<T>(List<T> list);
        /// <summary>
        /// Returns a deep copy of <paramref name="array"/>.
        /// </summary>
        /// <typeparam name="T">The type of the array.</typeparam>
        /// <param name="array">The array to copy.</param>
        /// <returns>Returns a deep copy of <paramref name="array"/>.</returns>
        public T[] DeepCopy<T>(T[] array);
        /// <summary>
        /// Adds <paramref name="value"/> to <paramref name="list"/>
        /// </summary>
        /// <typeparam name="T">The type of the list.</typeparam>
        /// <param name="list">The list to add too.</param>
        /// <param name="value">The value to add.</param>
        public void Add<T>(List<T> list, T value);
        /// <summary>
        /// Adds <paramref name="value"/> to <paramref name="array"/>.
        /// </summary>
        /// <typeparam name="T">The type of the array.</typeparam>
        /// <param name="array">The array to add too.</param>
        /// <param name="value">The value to add.</param>
        public void Add<T>(T[] array, T value);
        /// <summary>
        /// Removes <paramref name="value"/> from <paramref name="list"/>.
        /// </summary>
        /// <typeparam name="T">The type of the list.</typeparam>
        /// <param name="list">The list to remove from.</param>
        /// <param name="value">The value to remove.</param>
        public void Remove<T>(List<T> list, T value); //helper implements this method such that it is the first instance, so it needs special XML. Also look into LINQ
        /// <summary>
        /// Removes <paramref name="value"/> from <paramref name="array"/>.
        /// </summary>
        /// <typeparam name="T">The type of the array.</typeparam>
        /// <param name="array">The array to remove from.</param>
        /// <param name="value">The value to remove.</param>
        public void Remove<T>(T[] array, T value);
        /// <summary>
        /// Checks if <paramref name="value"/> is present in <paramref name="list"/> and returns true if it does.
        /// </summary>
        /// <typeparam name="T">The type of the list.</typeparam>
        /// <param name="list">The list to check.</param>
        /// <param name="value">The value to check for.</param>
        /// <returns>Returns true if <paramref name="value"/> is present in <paramref name="list"/> else false.</returns>
        public bool Contains<T>(List<T> list, T value);
        /// <summary>
        /// Checks if <paramref name="value"/> is present in <paramref name="array"/> and returns true if it does.
        /// </summary>
        /// <typeparam name="T">The type of the array.</typeparam>
        /// <param name="array">The array to check.</param>
        /// <param name="value">The value to check for.</param>
        /// <returns>Returns true if <paramref name="value"/> is present in <paramref name="array"/> else false.</returns>
        public bool Contains<T>(T[] array, T value);
        /// <summary>
        /// Generates an ID.
        /// </summary>
        /// <returns>Returns the generated ID.</returns>
        public string GenerateID(); 
    }
}
