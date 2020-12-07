use std::{
fs::File,
io::{prelude::*, BufReader},
path::Path,
};

fn lines_from_file(filename: impl AsRef<Path>) -> Vec<i32> {
let file = File::open(filename).expect("no such file");
let buf = BufReader::new(file);
buf.lines()
    .map(|l| l.unwrap().parse::<i32>().unwrap())
    .collect()
}
/*
fn lines_from_file2(filename: impl AsRef<Path>) -> Vec<i32> {
    let mut result: Vec<i32> = Vec::new();
    let file = File::open(filename).expect("no such file");
    let buf = BufReader::new(file);
    for line in buf.lines() {
        result.push(line.unwrap().parse::<i32>().unwrap());
    }
    return result;
}
    */

fn main() {

    let lines = lines_from_file("input1.txt");

    for x in &lines {
        if lines.contains(&(2020-x)) {
            println!("result day1 / 1 = {}", x*(2020-x));
            break;
        }
    }


    'outer: for x in &lines {
        for y in &lines {
            if lines.contains(&(2020-x-y)) {
                println!("result day1 / 2 = {}", x*y*(2020-x-y));
                break 'outer;
            }
        }
    }
}

