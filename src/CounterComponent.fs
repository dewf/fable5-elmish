module CounterComponent

open Elmish
open Feliz
open Feliz.UseElmish

type Props =
    { Initial: int
      OnChanged: int -> unit }

type private State = {
    Count: int
}

type private Msg =
    | Increment
    | Decrement

let private init (initial: int) () =
    printfn "subcounter init ran"
    { Count = initial }, Cmd.none

let private update (props: Props) msg model =
    match msg with
    | Increment ->
        let newCount = model.Count + 1
        if newCount % 5 = 0 then
            props.OnChanged newCount
        { model with Count = newCount }, Cmd.none

    | Decrement ->
        let newCount = model.Count - 1
        { model with Count = newCount }, Cmd.none

let private render state dispatch =
    printfn "subcounter rendered"
    Html.div [
      Html.button [ prop.text "-"; prop.onClick (fun _ -> dispatch Decrement) ]
      Html.span [ prop.style [ style.margin 10 ]; prop.text (string state.Count) ]
      Html.button [ prop.text "+"; prop.onClick (fun _ -> dispatch Increment) ]
    ]

[<ReactComponent>]
let CounterComponent (props: Props) =
    // future: look into memoizing this
    // (React.memo, but that would also require React.useCallback for our "OnChanged" handler,
    // because F#/JS function values (lambdas) don't compare as equal (a new heap value every time, or whatever)
    // might be worth looking into for extremely heavyweight components
    let state, dispatch =
        React.useElmish (
            init props.Initial,
            update props,
            [| box props.Initial |]
        )
    render state dispatch
