class CoolClass {
    static evaluate(value: number): number {
        console.log(`evaluate(${value})`);
        return value + 1;
    }
}

function woot(value: number): number {
    return CoolClass.evaluate(value);
}

export { woot };
