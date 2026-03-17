module App

open Elmish
open Elmish.React
open Feliz
open NativeStuff

type Model = int

type Msg =
    | Increment
    | Decrement

let init () =
    0

let update (msg: Msg) count =
    match msg with
    | Increment -> count + 1
    | Decrement -> count - 1

let view model dispatch =
    Html.div [
        Html.button [ 
            prop.onClick (fun _ -> dispatch Increment)
            prop.text "Inc"
        ]
        Html.span [
            prop.className "cool"
            prop.text (string model)
        ]
        Html.button [
            prop.onClick (fun _ -> dispatch Decrement)
            prop.text "Dec"
        ]
    ]

Woot 1001 |> ignore

Program.mkSimple init update view
|> Program.withReactBatched "elmish-app"
|> Program.run
