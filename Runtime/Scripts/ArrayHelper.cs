
using System;
using UdonSharp;

namespace valenvrc.Common
{
    public class ArrayHelper : UdonSharpBehaviour
    {
        public static string[] AddToList(string[] list, string item, out string result)
        {
            int emptyIndex = -1;

            for (int i = 0; i < list.Length; i++)
            {
                if (list[i] == item)
                {
                    result = "Item already exists";
                    return list;
                }

                if (emptyIndex == -1 && string.IsNullOrEmpty(list[i]))
                    emptyIndex = i;
            }

            if (emptyIndex != -1)
            {
                list[emptyIndex] = item;
                result = "Item added to empty slot";
                return list;
            }

            // No empty slot, need to expand
            var newList = new string[list.Length + 1];
            Array.Copy(list, newList, list.Length);
            newList[list.Length] = item;
            result = "Item added to new slot";
            return newList;
        }

        public static string[] RemoveFromList(string[] list, string item, out string result)
        {
            result = "Item not found";
            for (int i = 0; i < list.Length; i++)
            {
                if (!string.IsNullOrEmpty(list[i]) && list[i] == item)
                {
                    list[i] = string.Empty;
                    result = "Item removed";
                    break;
                }
            }
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