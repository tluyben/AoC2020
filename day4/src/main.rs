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

fn count_valid_passports(pps: &Vec<HashMap<String, String>>) -> (u32, u32) {
    let mut valid_count = 0;
    let mut valid_data_count = 0;
    let mandatory_fields = &["byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid"];
    let eye_colours = &["amb", "blu", "brn", "gry", "grn", "hzl", "oth"];
    for p in pps {
        let mut valid = true;
        for m in mandatory_fields {
            valid = valid && p.contains_key(m.to_owned()); 
        }
        if valid {
            valid_count = valid_count + 1; 

            let mut valid_data = true; 

            let mut i = p["byr"].parse::<u32>().unwrap();
            valid_data = valid_data && (i>=1920 && i<=2002); 
            
            i = p["iyr"].parse::<u32>().unwrap();
            valid_data = valid_data && (i>=2010 && i<=2020); 
                    
            i = p["eyr"].parse::<u32>().unwrap();
            valid_data = valid_data && (i>=2020 && i<=2030); 
              
            let mut hgt = p["hgt"].to_lowercase();
            let cm = hgt.contains("cm");
            hgt = hgt.replace("cm", "").replace("in", "");
            i = hgt.parse::<u32>().unwrap();
            if cm {
                valid_data = valid_data && (i>=150 && i<=193); 
            } else {
                valid_data = valid_data && (i>=59 && i<=76); 
            }
            
            valid_data = valid_data && p["hcl"].len() == 7; 

            let hcl = p["hcl"][1..].to_lowercase();
            
            valid_data = valid_data && hcl.chars().all(|c|char::is_numeric(c) || (c>='a'&& c<='f'));

            valid_data = valid_data && eye_colours.iter().any(|&s| s == p["ecl"]);

            valid_data = valid_data && p["pid"].len()==9 && p["pid"].chars().all(|c|char::is_numeric(c));

            if valid_data {
                valid_data_count = valid_data_count + 1; 
            } 
        }
    }
    return (valid_count, valid_data_count);
}

fn main() {
    let f = lines_from_file("./input1.txt");
    let pp = string_to_passport(&f);
    let valids =  count_valid_passports(&pp); 
    println!("Day4/1 {} valid passports", valids.0);
    println!("Day4/2 {} valid passports", valids.1);

    
}
/*
--- Part Two ---

The line is moving more quickly now, but you overhear airport security talking about how passports with invalid data are getting through. Better add some data validation, quick!

You can continue to ignore the cid field, but each other field has strict rules about what values are valid for automatic validation:

    byr (Birth Year) - four digits; at least 1920 and at most 2002.
    iyr (Issue Year) - four digits; at least 2010 and at most 2020.
    eyr (Expiration Year) - four digits; at least 2020 and at most 2030.
    hgt (Height) - a number followed by either cm or in:
        If cm, the number must be at least 150 and at most 193.
        If in, the number must be at least 59 and at most 76.
    hcl (Hair Color) - a # followed by exactly six characters 0-9 or a-f.
    ecl (Eye Color) - exactly one of: amb blu brn gry grn hzl oth.
    pid (Passport ID) - a nine-digit number, including leading zeroes.
    cid (Country ID) - ignored, missing or not.

Your job is to count the passports where all required fields are both present and valid according to the above rules. Here are some example values:

byr valid:   2002
byr invalid: 2003

hgt valid:   60in
hgt valid:   190cm
hgt invalid: 190in
hgt invalid: 190

hcl valid:   #123abc
hcl invalid: #123abz
hcl invalid: 123abc

ecl valid:   brn
ecl invalid: wat

pid valid:   000000001
pid invalid: 0123456789

Here are some invalid passports:

eyr:1972 cid:100
hcl:#18171d ecl:amb hgt:170 pid:186cm iyr:2018 byr:1926

iyr:2019
hcl:#602927 eyr:1967 hgt:170cm
ecl:grn pid:012533040 byr:1946

hcl:dab227 iyr:2012
ecl:brn hgt:182cm pid:021572410 eyr:2020 byr:1992 cid:277

hgt:59cm ecl:zzz
eyr:2038 hcl:74454a iyr:2023
pid:3556412378 byr:2007

Here are some valid passports:

pid:087499704 hgt:74in ecl:grn iyr:2012 eyr:2030 byr:1980
hcl:#623a2f

eyr:2029 ecl:blu cid:129 byr:1989
iyr:2014 pid:896056539 hcl:#a97842 hgt:165cm

hcl:#888785
hgt:164cm byr:2001 iyr:2015 cid:88
pid:545766238 ecl:hzl
eyr:2022

iyr:2010 hgt:158cm hcl:#b6652a ecl:blu byr:1944 eyr:2021 pid:093154719

Count the number of valid passports - those that have all required fields and valid values. Continue to treat cid as optional. In your batch file, how many passports are valid?

*/
