using System;
using System.IO; 
using System.Linq;
using System.Collections.Generic;

namespace Day11
{
    class Program
    {
        static int[][] around = new int[][] {
                            new int[2] {-1,-1}, new int[2] {-1,0},                 
                            new int[2] {-1,1},  new int[2] {0,1}, 
                            new int[2] {1,1},   new int[2] {1,0},
                            new int[2] {1,-1},  new int[2] {0,-1}};   
        static bool EmptyRule(char[][] map, int x, int y)
        {            
                
            foreach (var a in around) 
            {
                if (y+a[0]>=0 
                    && y+a[0]<map.Length 
                    && x+a[1]>=0 
                    && x+a[1]<map[y+a[0]].Length
                    && map[y+a[0]][x+a[1]] == '#') return false;
            }     
            return true; 
        }

        static bool EmptyLOSRule(char[][] map, int x, int y)
        {
            foreach (var a in around) 
            {
                int _y = y; 
                int _x = x; 

                while (_y+a[0]>=0 
                    && _y+a[0]<map.Length 
                    && _x+a[1]>=0 
                    && _x+a[1]<map[_y+a[0]].Length) 
                {
                    if (map[_y+a[0]][_x+a[1]] == '#') return false;
                    if (map[_y+a[0]][_x+a[1]] == 'L') break; 
                    _x += a[1];
                    _y += a[0];
                }
            }     
            return true; 
        }

        static bool OccupiedRule(char[][] map, int x, int y, int threshold = 4)
        {

            var occ = 0;


            foreach (var a in around) 
            {
                if (y+a[0]>=0 
                    && y+a[0]<map.Length
                    && x+a[1]>=0 
                    && x+a[1]<map[y+a[0]].Length
                    && map[y+a[0]][x+a[1]] == '#') occ++; 

                if (occ>=threshold) return true;
            }     

            return false; 

        }

        static bool OccupiedLOSRule(char[][] map, int x, int y, int threshold = 4)
        { 
            var occ = 0; 
            foreach (var a in around) 
            {
                int _y = y; 
                int _x = x; 

                while (_y+a[0]>=0 
                    && _y+a[0]<map.Length 
                    && _x+a[1]>=0 
                    && _x+a[1]<map[_y+a[0]].Length) 
                {
                    if (map[_y+a[0]][_x+a[1]] == 'L') break; 
                    if (map[_y+a[0]][_x+a[1]] == '#') 
                    {
                        occ++;
                        break;
                    } 
                    _x += a[1];
                    _y += a[0];
                }

                if (occ >= threshold) return true; 
            }     
            return false; 
        }

        static Tuple<char[][], bool> OneMove(char[][] map, Func<char[][], int, int, bool> emptyRule, Func<char[][], int, int, bool> occupiedRule) 
        {
            var _map = map.Select(l=>l.ToArray()).ToArray();
            var changed = false; 
            for (var y=0;y<map.Length;y++) {
                for (var x=0;x<map[y].Length;x++)
                {
                    var c = map[y][x];
                    if (c=='.')
                        continue; 

                    if(c == 'L' && emptyRule(map,x,y)) 
                    {
                        _map[y][x] = '#';
                        changed = true; 
                    }
                    else if (c == '#' && occupiedRule(map, x, y)) 
                    {
                        _map[y][x] = 'L';
                        changed = true; 
                    }
                }
            }
            return new Tuple<char[][], bool>(_map, changed);
        }

        static void PrintMap(char[][] map)
        {
            for(var y=0;y<map.Length;y++)
                Console.WriteLine(string.Join("", map[y]));
        }

        static int CountOccupied(char[][] map)
        {
            return map.SelectMany(l=>l).Where(l=>l=='#').Count(); 
        }

        static void P1(char[][] input)
        {
            var result = new Tuple<char[][], bool>(null, true); 
            while(result.Item2)
            {
                result = OneMove(input, (Func<char[][],int,int,bool>)EmptyRule, (m, x, y)=>{ return OccupiedRule(m, x, y, 4);}); 
                input = result.Item1;
                //PrintMap(input); 
            }
            Console.WriteLine($"Day 11/1 solution: {CountOccupied(input)}");
        }

        static void P2(char[][] input)
        {
            var result = new Tuple<char[][], bool>(null, true); 
            while(result.Item2)
            {
                result = OneMove(input, (Func<char[][],int,int,bool>)EmptyLOSRule, (m, x, y)=>{ return OccupiedLOSRule(m, x, y, 5);}); 
                input = result.Item1;
                //PrintMap(input); 
            }
            Console.WriteLine($"Day 11/2 solution: {CountOccupied(input)}");
        }


        static void Main(string[] args)
        {
            var input = File.ReadAllText("../1.txt").Split("\n").Select(l=>l.ToArray()).ToArray(); 

            P1(input); 
            P2(input);
        }
    }
}
