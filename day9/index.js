const fs = require('fs');

(async()=> {

    let buf = ''
    let numbers = []
    for await ( const chunk of fs.createReadStream('1.txt') ) {
        const lines = buf.concat(chunk).split(/\r?\n/);
        buf = lines.pop();
        for( const line of lines ) {
            numbers.push(parseInt(line))
        }
    } 

    let solution1 = 0
    for(let i=25;i<numbers.length-25;i++) {
        let hasCombo = false
        found: for (let j=i-25;j<i;j++) {
            for (let k=i-25;k<i;k++) {
                if (numbers[i]==numbers[k]+numbers[j]) {
                    hasCombo = true
                    break found
                }
            }
        }
        if (!hasCombo) {
            solution1 = numbers[i]
            console.log(`Day 9/1 solution: ${numbers[i]}`)
            break;
        }
    }

    found2: for (let i=0;i<numbers.length;i++) {
        let total = 0; 
        let smallest = Number.MAX_SAFE_INTEGER; 
        let largest = 0; 

        for (let j=i+1;j<numbers.length;j++) {
            if (numbers[j]>=solution1) break;
            
            total += numbers[j];
            
            if (numbers[j]<smallest) smallest = numbers[j]
            if (numbers[j]>largest) largest = numbers[j]

            if (total == solution1) {
                console.log(`Day 9/2 solution first ${smallest}+${largest} = ${smallest + largest}`)
                break found2
            }
        }

    }


})()

