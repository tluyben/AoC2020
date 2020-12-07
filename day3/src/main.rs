use std::{
    fs::File,
    io::{prelude::*, BufReader},
    path::Path,
    };

fn lines_from_file(filename: impl AsRef<Path>) -> Vec<Vec<bool>> {
    let file = File::open(filename).expect("no such file");
    let buf = BufReader::new(file);

    return buf.lines()
        .map(|l| l.unwrap().chars().map(|c| c=='#').collect())
        .collect()
}

fn jump(matrix: &Vec<Vec<bool>>, x: usize, y: usize, xadd: usize, yadd: usize) -> (usize, usize, bool) { // newx, newy, istree
    let newx = (x+xadd) % (matrix[y].len());
    (newx, y+yadd, matrix[y][x]==true)
}

fn count_trees(matrix: &Vec<Vec<bool>>, xadd: usize, yadd: usize) -> u32 {
    let mut tree_count = 0;

    let mut x: usize = 0;
    let mut y: usize = 0;
    
    while y < matrix.len() {

        let result = jump(matrix, x, y, xadd, yadd);
        if result.2 {
            tree_count = tree_count + 1;
        }
        x = result.0;
        y = result.1;
    }

    return tree_count;
}

fn main() {
    let matrix = lines_from_file("input1.txt");
    println!("Day 3 / 1: number of trees hit {}", count_trees(&matrix, 3, 1));
    
    // let total = count_trees(&matrix, 1, 1)
    // * count_trees(&matrix, 3, 1)
    // * count_trees(&matrix, 5, 1)
    // * count_trees(&matrix, 7, 1)
    // * count_trees(&matrix, 1, 2)


    let mut total = 1; 
    for j in &[(1,1), (3,1), (5,1), (7,1), (1,2)] {
        total = total * count_trees(&matrix, j.0, j.1);
    }
    
    println!("Day 3 / 2: number of trees hit and multiplied {}", total);
}
/*
--- Part Two ---

Time to check the rest of the slopes - you need to minimize the probability of a sudden arboreal stop, after all.

Determine the number of trees you would encounter if, for each of the following slopes, you start at the top-left
 corner and traverse the map all the way to the bottom:

    Right 1, down 1.
    Right 3, down 1. (This is the slope you already checked.)
    Right 5, down 1.
    Right 7, down 1.
    Right 1, down 2.

In the above example, these slopes would find 2, 7, 3, 4, and 2 tree(s) respectively; multiplied together, 
these produce the answer 336.

What do you get if you multiply together the number of trees encountered on each of the listed slopes?

*/
