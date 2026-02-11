function convertToCode(symbols) {
    let total1 = "";
    let total2 = "";

    for (let i = 0; i < symbols.length; i++) {
        let c = symbols.charCodeAt(i).toString();
        total1 += c;
        if (c.includes("7")) {
            c = c.replace("7", "1"); 
        }
        total2 += c;
    }
    console.log(`Значение total1: ${total1} 
        \nЗначение total2: ${total2} 
        \ntotal1 - total2 = ${total1 - total2}`);
}

symbols = "ABC";
convertToCode(symbols);