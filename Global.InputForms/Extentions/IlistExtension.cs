using System;
using System.Collections;

namespace Global.InputForms.Extentions
{
    public static class IlistExtension
    {
        public static bool ContainsDuplicates(this IList list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                for (int y = i + 1; y < list.Count; y++)
                {
                    if (list[i] == list[y])
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
