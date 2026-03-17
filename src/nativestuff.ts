class CoolClass {
    static evaluate(value: number): number {
        console.log("your value is: " + value + "! 😎");
        return value + 1;
    }
}

function woot(value: number): number {
    return CoolClass.evaluate(value);
}

export { woot };
