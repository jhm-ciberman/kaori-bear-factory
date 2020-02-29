using System.Collections.Generic;

public static class ListUtils
{
    // https://stackoverflow.com/a/1262619/2022985
    public static IList<T> Shuffle<T>(System.Random random, IList<T> list)  
    {  
        int n = list.Count;  
        while (n > 1) 
        {  
            n--;  
            int k = random.Next(n + 1);
            (list[k], list[n]) = (list[n], list[k]);
        }
        return list;
    }
}