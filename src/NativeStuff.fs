module NativeStuff

open Fable.Core

type INativeStuff =
    abstract woot : int -> int

[<ImportAll("./nativestuff.ts")>]
let nativeFuncs: INativeStuff = jsNative

let Woot (value: int): int =
    nativeFuncs.woot value

// // alternative single-function import
// // obviates this whole F# module -
// // App.js will simply pull in 'woot' from nativestuff.ts directly!
// [<Import("woot", "./nativestuff.ts")>]
// let Woot (value: int): int = jsNative
