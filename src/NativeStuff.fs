module NativeStuff

open Fable.Core

type INativeStuff =
    abstract woot : int -> int

[<ImportAll("./nativestuff.ts")>]
let nativeFuncs: INativeStuff = jsNative

let Woot (value: int): int =
    nativeFuncs.woot value
