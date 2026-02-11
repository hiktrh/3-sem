function getReversedEnglishString(str){
    let result = "";
    for (let i = str.length - 1; i >= 0; i--){
        if ((str[i] >= 'A' && str[i] <= 'Z') || (str[i] >= 'a' && str[i] <= 'z')) {
            result += str[i];
        }
    }
    return result;
}

console.log(getReversedEnglishString("JavaScr53э? ipt")); 
