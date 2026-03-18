module App

open Elmish
open Elmish.React
open Feliz
open Feliz.Router
open NativeStuff
open CounterComponent

type State = {
    Count: int
    CurrentUrl: string list
    LocationHistory: string list list
    LastCounterEvent: (int * int) option
}

type Msg =
    | Increment
    | Decrement
    | UrlChanged of string list
    | NavigateHome
    | SubcounterEvent of int * int

let init () =
    { Count = 0
      CurrentUrl = Router.currentUrl ()
      LocationHistory = []
      LastCounterEvent = None }, Cmd.none

let update (msg: Msg) (state: State) =
    match msg with
    | Increment ->
        { state with Count = state.Count + 1 }, Cmd.none
    | Decrement ->
        { state with Count = state.Count - 1 }, Cmd.none
    | UrlChanged segments ->
        let nextHistory =
            segments :: state.LocationHistory
        { state with
            CurrentUrl = segments
            LocationHistory = nextHistory }, Cmd.none
    | NavigateHome ->
        state, Cmd.navigate()
    | SubcounterEvent (which, value) ->
        printfn "subcounter event occurred! %d" value
        { state with LastCounterEvent = Some (which, value) }, Cmd.none

let render (state: State) (dispatch: Msg -> unit) =
    printfn "outer render ran"
    let content =
        match state.CurrentUrl with
        | [ "users"; Route.Int userId ] ->
            Html.div [
                Html.p $"welcome to users page, user {userId}"
                Html.button [
                    prop.onClick (fun _ -> dispatch NavigateHome)
                    prop.text "return home"
                ]
            ]
        | [] -> // home
            Html.div [
                Html.h3 "Home"

                Html.button [
                    prop.onClick (fun _ -> dispatch Decrement)
                    prop.text "Decrement"
                ]
                Html.span [
                    prop.className "cool"
                    prop.text (string state.Count)
                ]
                Html.button [
                    prop.onClick (fun _ -> dispatch Increment)
                    prop.text "Increment"
                ]

                Html.br []

                Html.div [
                    let subcounterFunc (id: int) =
                        (fun x -> SubcounterEvent (id, x) |> dispatch)
                    CounterComponent {| Initial = 0; Step = 1; OnChanged = subcounterFunc 1 |} // nb: anonymous record, following Feliz docs
                    CounterComponent {| Initial = 10; Step = 5; OnChanged = subcounterFunc 2 |}
                ]

                Html.br []

                Html.div [
                    Html.p "last subcounter event:"
                    match state.LastCounterEvent with
                    | Some (id, value) ->
                        Html.p (sprintf "ID %d, value %d" id value)
                    | None ->
                        Html.p "(none)"
                ]

                Html.div [
                    Html.a [
                        prop.href "#/users/345"
                        prop.text "test user page OK?"
                    ]
                ]

                Html.div [
                    for location in state.LocationHistory ->
                        Html.div [
                            prop.className "green-background"
                            prop.text (sprintf "%A" location)
                        ]
                ]
            ]
        | _ -> // 404
            Html.div [
                Html.h3 "that's a 404, homedawg"
                Html.button [
                    prop.onClick (fun _ -> dispatch NavigateHome)
                    prop.text "Go home, you're drunk"
                ]
            ]

    React.router [
        router.onUrlChanged (UrlChanged >> dispatch)
        router.children [
            content
        ]
    ]

// quick console test to make sure Typescript is wireed up properly
Woot 1001 |> ignore

Program.mkProgram init update render
// |> Program.withReactSynchronous "elmish-app"
|> Program.withReactBatched "elmish-app"
|> Program.run
