function SumOfCubes (n)
{
    let sum = 0;
    for (i=1; i <= n; ++i)
    {
        sum += Math.pow(i,3);
    }
    return sum;
}
let number = SumOfCubes(4);
console.log(number);