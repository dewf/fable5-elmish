module CounterComponent

open Elmish
open Feliz
open Feliz.UseElmish

type Props =
    // this has to be an anonymous record because,
    // according to Feliz docs, regular records are problematic for React props (due to how Fable converts them, I think?)
    {| Initial: int
       Step: int
       OnChanged: int -> unit |}

type private State = {
    Count: int
    Step: int
}

type private Msg =
    | Increment
    | Decrement

let private init (props: Props) () =
    printfn "subcounter init ran"
    { Count = props.Initial; Step = props.Step }, Cmd.none

let private update (props: Props) msg state =
    printfn "counter component update"
    match msg with
    | Increment ->
        let newCount = state.Count + 1
        if newCount % 5 = 0 then
            props.OnChanged newCount
        { state with Count = newCount }, Cmd.none

    | Decrement ->
        let newCount = state.Count - 1
        { state with Count = newCount }, Cmd.none

let private render state dispatch =
    printfn "subcounter render"
    Html.div [
      Html.button [ prop.text "-"; prop.onClick (fun _ -> dispatch Decrement) ]
      Html.span [ prop.style [ style.margin 10 ]; prop.text (string state.Count) ]
      Html.button [ prop.text "+"; prop.onClick (fun _ -> dispatch Increment) ]
    ]

[<ReactMemoComponent>]
let private CounterComponentInner (props: Props) =
    printfn "counter component inner (memoized)"
    let state, dispatch =
        React.useElmish (
            init props,
            update props,
            [| box props.Initial; box props.Step |] // only if either of these change, does 'init' run again?
        )
    render state dispatch

// this outer wrapper is I think only necessary because:
// 1) we need to wrap callbacks with React.useCallback, otherwise they will appear to be new/unique, and memoization will fail (F#/JS language lambda issue)
// 2) you can only call React.useCallback from within something that is itself a ReactComponent ... which our root-level Elmish app is not
[<ReactComponent>]
let CounterComponent (props: Props) =
    let stabilized =
        let cb =
            React.useCallback(props.OnChanged, [||])
        {| props with OnChanged = cb |}
    // below is memoized, so in theory should only render when necessary
    CounterComponentInner stabilized
