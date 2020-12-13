using System;
using System.IO; 
using System.Linq;
using System.Collections.Generic;

namespace Day13
{
    class Program
    {

        static void P1(int ts, int[] busses)
        {
            var first = int.MaxValue;
            var busId = -1; 
            for(int i=0;i<busses.Length;i++) 
            {
                var bus = 0;
                for (var j=0;j<ts+busses[i];j++) 
                {
                    bus += busses[i];
                    if (bus > ts) 
                    {
                        if (bus < first) 
                        {
                            first = bus;
                            busId = busses[i];
                        }
                        break;
                    }                    
                }
            }
            Console.WriteLine($"Day13/1 answer {(first-ts)*busId}");
        }
        static bool OffsetMatches(long ts, long[] buscount)
        {
            
            for(int i=0;i<buscount.Length;i++)
            {
                if(buscount[i]>=0)
                {
                    if (ts+i != buscount[i]) return false;
                }
            }
            return true; 
        }

        // pointless, but correct
        static void P2(int[] busses)
        {
            long ts = 0; 
            var buscount = busses.Select(b=>(long)b).ToArray();
            if(ts>0) for(int i=0;i<busses.Length;i++) 
            {
                if(buscount[i]>=0)
                    buscount[i]+=(long)Math.Floor((double)(ts/busses[i]))*busses[i]; 
            }
            var minbus = busses.Where(b=>b>0).Min();
            while (true)
            {
                for(int i=0;i<busses.Length;i++) 
                {
                    if(buscount[i]>=0 && buscount[i]<ts)
                        buscount[i]+=busses[i]; 
                }

                
                if(OffsetMatches(ts, buscount)) 
                {
                    Console.WriteLine($"Day13/2 answer {ts}");
                    return; 
                }
                ts++; 
            }    
        }

        static void P22(int[] busses)
        {

            var l = busses.Select((b,i)=>(i%b,b)).Where(b=>b.b!=-1);
            long ts = 0; 
            long inc = 1;
            foreach(var i in l) 
            {
                while (true)
                {
                    if (ts%i.b==(i.Item1>0?i.b-i.Item1:0))
                    {
                        break;
                    }
                    ts += inc; 
                }
                inc *= i.b;
            }

            Console.WriteLine($"Day13/2 answer {ts}");
        }
        static void Main(string[] args)
        {
            var input = File.ReadAllText("../1.txt").Trim().Split("\n").ToArray();
            var ts = int.Parse(input[0]);
            var busses = input[1].Split(',').Where(c=>c!="x").Select(c=>int.Parse(c)).ToArray(); 
            var busses2 = input[1].Split(',').Select(c=>c=="x"?-1:int.Parse(c)).ToArray();

            P1(ts, busses);
            //P2(busses2);
            P22(busses2);
        }
    }
}
