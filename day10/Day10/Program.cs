using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Day10
{
    class Program
    {
        static void Main(string[] args)
        {
            var numbers = File
                .ReadAllLines("../test1.txt")
                .Select(l=>int.Parse(l))
                .OrderByDescending(l=>l)
                .ToList(); 
        
            var highest = (int)numbers.Max();
            numbers.Insert(0, highest+3);
            numbers.Add(0);

            var len = new Dictionary<int, long>(); 
            var possibles = new Dictionary<int, List<int>>(); 
            len[highest] = 1; 

            numbers.ForEach(n=>
            {
                possibles[n] = numbers.Where(n1=>n1>n && n1<=n+3).ToList();
                if (n!=highest) 
                {
                    len[n] = 0; 
                    for(var i=0;i<possibles[n].Count;i++)
                        len[n] += len[possibles[n][i]];
                }
            });

            Console.WriteLine($"Solution {len[0]}");

            numbers.Reverse();
            Console.WriteLine($"Result recursive: {RecursiveSolution(0, numbers)}");

        }

        static long RecursiveSolution(int i, List<int> numbers)
        {
            if (i==numbers.Count-1)
                return 1; 

            long result = 0; 
            for(int j = i+1; j<numbers.Count;j++)
            {
                if (numbers[j]-numbers[i]<=3)
                    result += RecursiveSolution(j, numbers);
            }
            return result;
        }
    }
}
