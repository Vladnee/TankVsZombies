using System.Collections.Generic;

static class ListExtensions
{
    public static T PopAt<T>(this List<T> list, int index)
    {
        T element = list[index];
        list.RemoveAt(index);
        return element;
    }
}
