
using System;
using UdonSharp;

namespace valenvrc.Common
{
    public class ArrayHelper : UdonSharpBehaviour
    {
        public static T[] AddToList<T>(T[] list, T item, out string result)
        {
            //Check for duplicates
            if (Array.IndexOf(list, item) != -1){
                result = "Item already exists";
                return list;
            }

            // Check for an empty slot
            for (int i = 0; i < list.Length; i++){
                if (object.Equals(list[i], default(T))){
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

        public static T[] RemoveFromList<T>(T[] list, T item, out string result){
            //Find the item
            int index = Array.IndexOf(list, item);
            if (index == -1){
                result = "Item not found";
                return list;
            }

            //Remove the item by setting it to default
            list[index] = default(T);
            result = "Item removed";
            return list;
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