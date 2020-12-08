use std::{
    fs::File,
    io::{prelude::*, BufReader},
    path::Path,
    };
    
fn lines_from_file(filename: impl AsRef<Path>) -> Vec<String> {
    let file = File::open(filename).expect("no such file");
    let buf = BufReader::new(file);
    buf.lines()
        .map(|l| l.unwrap())
        .collect()
}

fn calculate_seat_number(line: &String) -> u32 {
    let coord = calculate_seat_coordinate(line);
    return coord.0*8+coord.1;
}

// y, x
fn calculate_seat_coordinate(line: &String) -> (u32, u32) {
    let mut row_min = 0; 
    let mut row_max = 127;
    let mut col_min = 0;
    let mut col_max = 7;

    for c in line.chars() {
        match c {
            'F' => row_max = row_min + (row_max-row_min-1)/2, 
            'B' => row_min = row_min + (row_max-row_min+1)/2,
            'L' => col_max = col_min + (col_max-col_min-1)/2,
            'R' => col_min = col_min + (col_max-col_min+1)/2, 
            _ => panic!("nope") 
        }
    }

    return (row_min, col_min)
}

fn highest_seat_id(lines: &Vec<String>) -> u32 {
    let mut result: u32 = 0;
    for line in lines {
        let a = calculate_seat_number(line);
        if a > result { result = a}
    }
    result
}
    
fn pretty_print_seats(seatmap: &Vec<Vec<bool>>) {
    for y in seatmap {
        for x in y {
            let marker = if *x { "*" } else { "!" };
            print!(" {}", marker);
        }
        println!("");
    }
}

fn fill_seats(lines: &Vec<String>) -> Vec<Vec<bool>> {
    let mut result = vec![vec![false; 8]; 128];

    for line in lines {
        let a = calculate_seat_coordinate(line);
        result[a.0 as usize][a.1 as usize] = true;
    }

    return result;
}

// y, x
fn find_untaken(seats: &Vec<Vec<bool>>) -> (u32, u32) {
    let mut x = 0;
    let mut y = 0; 
    let mut passed_first = false; 

    for row in 0..128 {
        for column in 0..8 {
        
            let marker = if !seats[row][column] && (!passed_first || (x!=0||y!=0)) { "X" }  else if seats[row][column]  { "*" } else { "!" };
            print!(" {}", marker);

            if !seats[row][column] {
                if passed_first && x==0 && y==0 {
                    print!("{},{}", row,column);
                    y = row as u32; 
                    x = column as u32;
                }
            } else {
                passed_first = true; 
            }
        }
        println!("");
    }
    return (y,x);
}
    
fn main() {
    let lines = lines_from_file("./input1.txt");

    println!("Day 5/1 The highest seat ID is: {}", highest_seat_id(&lines));

    let seatmap = fill_seats(&lines); 

    //pretty_print_seats(&seatmap);

    let untaken = find_untaken(&seatmap);
    println!("Day 5/2 Your chair ID is: {}", untaken.0*8+untaken.1);

}


/* 
--- Part Two ---

Ding! The "fasten seat belt" signs have turned on. Time to find your seat.

It's a completely full flight, so your seat should be the only missing boarding pass in your list. 
However, there's a catch: some of the seats at the very front and back of the plane don't exist on this aircraft, 
so they'll be missing from your list as well.

Your seat wasn't at the very front or back, though; the seats with IDs +1 and -1 from yours will be in your list.

What is the ID of your seat?
*/