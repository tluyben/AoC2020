const fs = require('fs');

(async()=> {

    let buf = ''
    let numbers = [0]
    for await ( const chunk of fs.createReadStream('1.txt') ) {
        const lines = buf.concat(chunk).split(/\r?\n/);
        buf = lines.pop();
        for( const line of lines ) {
            numbers.push(parseInt(line))
        }
    } 

    let highest = 0
   
    numbers.sort((a, b) => a - b)
    highest = numbers[numbers.length-1]
    numbers.push(highest+3)


    numbers.reverse()
    let poss = {}
    let len = {}
    len[highest] = 1
    
    numbers.forEach((elem)=>{
        poss[elem] = numbers.filter(el=>el>elem && el <=elem+3)
        if(elem!==highest){
            len[elem] = 0
            for(let i=0;i<poss[elem].length;i++){
                len[elem] += len[poss[elem][i]]  
           }       
        }
    })

    
    console.log("Day 10/2 solution: " +len[0])

    numbers.reverse()

    let start = 0
    let i1 = 0
    let i3 = 0 
    let countTwo = true
    
    for(let j=0;j<numbers.length-1;j++) {

        let  i = numbers[j]
        for (k=j+1;k<numbers.length;k++) {
            if (numbers[k]>=0 && i==numbers[k]-1) {
                i1++
                numbers[j] = -1
                break
            } else if (countTwo && numbers[k]>=0 && i==numbers[k]-2) {
                numbers[j] = -1
                break
            }else if (numbers[k]>=0 && i==numbers[k]-3) {
                numbers[j] = -1
                i3++
                break
            }
        }
    }
  
    console.log(`Day 10/1 solution: ${i1} ${i3} ${i1*i3}`)


})()

