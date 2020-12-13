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

        static int[][] cmp(char[][] input, char c)
        {
            return input.Select(l=>l.Select(c1=>c1==c?1:0).ToArray()).ToArray(); 
        }
        static int[][] cmp(int[][] input, int c)
        {
            return input.Select(l=>l.Select(c1=>c1==c?1:0).ToArray()).ToArray(); 
        }
        static void p(char[][] input)
        {
            for(var y=0;y<input.Length;y++)
                Console.WriteLine(string.Join(" ", input[y]));
        }
        static void p(int[][] input)
        {
            for(var y=0;y<input.Length;y++)
                Console.WriteLine(string.Join(" ", input[y]));
        }

        static int[][] not(int[][] input)
        {
            return input.Select(l=>l.Select(c1=>c1==0?1:0).ToArray()).ToArray(); 
        }       

        static int[][] enlist(int[][] input, int row)
        {
            return enlist(input, new int[] {row}); 
        }

        static int[][] enlist(int[][] input, int[] row) 
        {
            var l = input.ToList(); 
            if (input.Length>0) 
            {
                var ll = input[0].Length; 
                if (row.Length!=ll)
                {
                    var _row = row.ToList(); 
                    if (row.Length<ll) for(int i=0;i<ll-row.Length;i++)
                        _row.Add(0);
                    if (row.Length>ll) for (int i=0;i<row.Length-ll;i++)
                        _row.RemoveAt(row.Length-i);

                    row = _row.ToArray(); 
                }
               
            }
            l.Add(row);

            return l.ToArray();  
        }

        static int[][] drop(int[][] input, int amount)
        {
            var l = input.ToList(); 
            for(int i=0;i<amount;i++)
            {
                l.RemoveAt(0);
            }
            return l.ToArray(); 
        }

        static int[][] flip(int [][] input)
        {
            var l = new List<List<int>>(); 
            for(int y=0;y<input.Length;y++)
            {

                for (int x=0;x<input[y].Length;x++)
                {
                    if (l.Count-1<x)
                        l.Add(new List<int>());

                    l[x].Add(input[y][x]); 
                }
            }   
            return l.Select(l=>l.Select(l1=>l1).ToArray()).ToArray();         
        }

        static int[][] plus(int [][] input, int[][] add)
        {
            var cx =0;
            var cy =0; 
            for(int y=0;y<input.Length;y++)
            {
                
                for (int x=0;x<input[y].Length;x++)
                {
                    input[y][x] += add[cy][cx];

                    cx++;
                    if (cx>=add[cy].Length) cx = 0; 
                }

                cy++; 
                if (cy>=add.Length) cy = 0 ; 

            }
            return input;
        }

        static int[][] eachr(int [][] input, Func<int[][], int[][]> f, int n = 1)
        {
            
        }


        static void Main(string[] args)
        {
            var input = File.ReadAllText("../ex1.txt").Trim().Split("\n").Select(l=>l.ToArray()).ToArray(); 

            P1(input); 
            P2(input);

            var l = cmp(input, '#'); 
            var f = not(cmp(input, '.'));

            p(plus(l, flip(drop(enlist(l,0), 1))));

            Console.WriteLine("\n\n");
            p(flip(drop(enlist(not(cmp(input, '.')), 0), 1)));

        }
    }
}
