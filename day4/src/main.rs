use std::collections::HashMap;
use std::{
    fs::File,
    io::{prelude::*, BufReader},
    path::Path
    };
    
fn lines_from_file(filename: impl AsRef<Path>) -> String {
    let file = File::open(filename).expect("no such file");
    let mut buf = BufReader::new(file);
    let mut f: String = "".to_string();
    match buf.read_to_string(&mut f) {
        Ok(_) => return f, 
        Err(_) => panic!("cannot read file")

    }
}

fn string_to_passport(buffer: &String) -> Vec<HashMap<String, String>> {
    let mut result: Vec<HashMap<String, String>> = Vec::new();
    let passports = buffer.split("\n\n");
    for passport in passports {
        let mut hmap: HashMap<String, String> = HashMap::new();
        let fields = passport.split_ascii_whitespace();

        for field in fields {
            
            let mut keyvalue = field.split(":");
            let key: String = keyvalue.nth(0).unwrap().to_owned();
            let value: String = keyvalue.nth(0).unwrap().to_owned();

            hmap.insert(key, value);
        }
        result.push(hmap);
    }
    result
}

fn count_valid_passports(pps: &Vec<HashMap<String, String>>) -> u32 {
    let mut valid_count = 0;
    let mandatory_fields = &["byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid"];
    for p in pps {
        let mut valid = true;
        for m in mandatory_fields {
            valid = valid && p.contains_key(m.to_owned()); 
        }
        if valid {
            valid_count = valid_count + 1; 
        }
    }
    return valid_count;
}


fn main() {
    let f = lines_from_file("./input1.txt");
    let pp = string_to_passport(&f);
    println!("Day4/1 {} valid passports", count_valid_passports(&pp));
    
}
