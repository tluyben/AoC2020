using System;
using System.IO; 
using System.Linq;
using System.Collections.Generic;

namespace Day12
{
    class Program
    {
        static char Turn(char current, int degrees)
        {
            var dirs = "NESW";
            var idx = dirs.IndexOf(current);

            idx = (idx+(degrees/90))%4; 
            if (idx<0) idx = 4+idx; 
            return dirs[idx];
        }

        static void P1(string[] input) 
        {
            var start = 'E'; 

            var east = 0; 
            var north = 0; 

            foreach(var i in input)
            {
                var c = i[0];
                var l = int.Parse(i.Substring(1));

                if (c == 'F') c = start; 

                switch(c)
                {
                    case 'L': 
                        start = Turn(start, -1*l);
                        break;
                    case 'R':
                        start = Turn(start, l);
                        break;
                    case 'E':
                        east += l;
                        break;
                    case 'W':
                        east -= l; 
                        break;
                    case 'N':
                        north += l;
                        break;
                    case 'S':
                        north -= l;
                        break;
                }
            }
            Console.WriteLine($"Day 12/1 answer: {Math.Abs(east)} {Math.Abs(north)} {Math.Abs(east)+Math.Abs(north)}");


        }

        static void P2(string[] input) 
        { 
            var wps = new List<int> {1,10,0,0};

            var east = 0;
            var north = 0; 
            
            foreach(var i in input)
            {
                var c = i[0];
                var l = int.Parse(i.Substring(1));

               
                switch(c)
                {
                    case 'L': 
                        for(var j=0;j<l/90;j++) {
                            wps.Add(wps.First());
                            wps.RemoveAt(0); 

                        }
                        break;
                    case 'R':

                        for(var j=0;j<l/90;j++) {
                            wps.Insert(0, wps.Last());
                            wps.RemoveAt(wps.Count-1); 

                        }
                        break;
                    case 'E':
                        wps[1] += l;
                        break;
                    case 'F':
                        east += l*(wps[1]-wps[3]);
                        north += l*(wps[0]-wps[2]);
                        break;
                    case 'W':
                        wps[3] += l; 
                        break;
                    case 'N':
                        wps[0] += l;
                        break;
                    case 'S':
                        wps[2] += l;
                        break;
                }
            }
            Console.WriteLine($"Day 12/2 answer: {Math.Abs(east)} {Math.Abs(north)} {Math.Abs(east)+Math.Abs(north)}");



        }

        static void Main(string[] args)
        {
            var input = File.ReadAllText("../1.txt").Trim().Split("\n").ToArray();
           
            P1(input);
            P2(input);
        }
    }
}
