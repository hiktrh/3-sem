function getString(str1 = "первый", str2, str3 = prompt("введите 3-й параметр", "")){
    return str1 + " " + str2 + " " + str3;
}
alert(getString());