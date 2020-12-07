use std::{
    fs::File,
    io::{prelude::*, BufReader},
    path::Path,
    };

fn lines_from_file(filename: impl AsRef<Path>) -> (i32, i32, i32) {
    let mut valid = 0 ; 
    let mut part2_valid = 0; 
    let mut total = 0;

    let file = File::open(filename).expect("no such file");
    let buf = BufReader::new(file);

    for line in buf.lines() {
        total = total + 1;
        let parsed = parse_rule(&(line.unwrap()));
        if valid_password(&parsed.0, &parsed.1) {
            valid = valid + 1;
        }
        if valid_password2(&parsed.0, &parsed.1) {
            part2_valid = part2_valid + 1
        }
    }

    return (total, valid, part2_valid);
}

struct Rule {
    min: u32,
    max: u32,
    c: char
}

fn parse_rule(s: &String) -> (Rule, String) {
    let flubber: Vec<&str> = s.split(':').collect();
    
    let r: Vec<&str> = flubber[0].split('-').collect();
    let min = r[0].parse::<u32>().unwrap();

    let m: Vec<&str> = r[1].split(' ').collect();
    let max = m[0].parse::<u32>().unwrap();
    
    let c = m[1].chars().nth(0).unwrap();

    (Rule{min, max, c}, flubber[1].trim().to_owned())
}

fn valid_password(rule: &Rule, pw: &String) -> bool {
    let occ = pw.len() - pw.replace(rule.c, "").len();
    return occ>=(rule.min as usize) && occ<=(rule.max as usize);
}

fn valid_password2(rule: &Rule, pw: &String) -> bool {

    return (pw.as_bytes()[((rule.min-1) as usize)] as char == rule.c)
            ^ (pw.as_bytes()[((rule.max-1) as usize)] as char == rule.c)
            ;
}
    
fn main() {
    let valid_passwords = lines_from_file("input1.txt");
    println!("Day 2 / 1: We found {} lines with {} valid passwords", valid_passwords.0, valid_passwords.1); 
    println!("Day 2 / 2: We found {} lines with {} valid passwords", valid_passwords.0, valid_passwords.2); 

}


/*
--- Part Two ---

While it appears you validated the passwords correctly, they don't seem to be what the Official 
Toboggan Corporate Authentication System is expecting.

The shopkeeper suddenly realizes that he just accidentally explained the password policy rules 
from his old job at the sled rental place down the street! The Official Toboggan Corporate Policy 
actually works a little differently.

Each policy actually describes two positions in the password, where 1 means the first character, 2 
means the second character, and so on. (Be careful; Toboggan Corporate Policies have no concept of 
    "index zero"!) Exactly one of these positions must contain the given letter. Other occurrences 
    of the letter are irrelevant for the purposes of policy enforcement.

Given the same example list from above:

    1-3 a: abcde is valid: position 1 contains a and position 3 does not.
    1-3 b: cdefg is invalid: neither position 1 nor position 3 contains b.
    2-9 c: ccccccccc is invalid: both position 2 and position 9 contain c.

How many passwords are valid according to the new interpretation of the policies?
*/