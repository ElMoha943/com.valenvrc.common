
using System;
using UdonSharp;

namespace valenvrc.Common
{
    public class ArrayHelper : UdonSharpBehaviour
    {
        /// <summary>
        /// Adds an item to an array, expanding the array if necessary.
        /// </summary>
        /// /// <typeparam name="T">Type of the items in the array</typeparam>
        /// <param name="list">The original array</param>
        /// <param name="item">The item to add</param>
        /// <param name="result">Output message indicating the result of the operation</param>
        /// <returns>A new array with the item added, or the original array if the item already exists</returns>
        /// <remarks>
        /// If the item already exists in the array, it will not be added again.
        /// </remarks>
        public static T[] AddToList<T>(T[] list, T item, out string result)
        {
            //Check for duplicates
            if (Array.IndexOf(list, item) != -1)
            {
                result = "Item already exists";
                return list;
            }

            // Check for an empty slot
            for (int i = 0; i < list.Length; i++)
            {
                if (object.Equals(list[i], default(T)))
                {
                    list[i] = item;
                    result = "Item added to existing slot";
                    return list;
                }
            }

            // No empty slot, need to expand
            var newList = new T[list.Length + 1];
            Array.Copy(list, newList, list.Length);
            newList[list.Length] = item;
            result = "Item added to new slot";
            return newList;
        }

        /// <summary>
        /// Checks if an item exists in an array.
        /// </summary>
        /// <param name="array"></param>
        /// <param name="item"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool Contains<T>(T[] array, T item)
        {
            return Array.IndexOf(array, item) != -1;
        }

        /// <summary>
        /// Removes an item from an array, optionally shrinking the array.
        /// </summary>
        /// <typeparam name="T">Type of the items in the array</typeparam>
        /// <param name="list">The original array</param>
        /// <param name="item">The item to remove</param>
        /// <param name="result">Output message indicating the result of the operation</param>
        /// <param name="shrink">If true, the array will be shrunk after removing the item</param>
        /// <returns>A new array with the item removed, or the original array if the item was not found</returns>
        public static T[] RemoveFromList<T>(T[] list, T item, out string result, bool shrink = false)
        {
            //Find the item
            int index = Array.IndexOf(list, item);
            if (index == -1)
            {
                result = "Item not found";
                return list;
            }

            //If shrink is true, we will remove the item and shrink the array
            if (shrink)
            {
                var newList = new T[list.Length - 1];
                Array.Copy(list, 0, newList, 0, index);
                Array.Copy(list, index + 1, newList, index, list.Length - index - 1);
                result = "Item removed and list shrunk";
                return newList;
            }
            else
            {
                //Remove the item by setting it to default
                list[index] = default(T);
                result = "Item removed";
                return list;
            }
        }

        public static string[] SanitizeStringArray(string[] input, bool trimWhitespace = true)
        {
            if (input == null) return new string[0];
            string[] result = new string[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                string val = input[i];
                if (string.IsNullOrEmpty(val))
                {
                    result[i] = "";
                }
                else
                {
                    result[i] = trimWhitespace ? val.Trim() : val;
                }
            }
            return result;
        }
    }
}