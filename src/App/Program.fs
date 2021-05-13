// Learn more about F# at http://docs.microsoft.com/dotnet/fsharp
open System
open IO.Handler
open Generic
open POO
open Actors
open Reactive
open Retry.Async

let eval num =
    if num > 0 then
        printfn "+"
    else
        printfn "-"

let pipe collection =
    collection
    |> List.map (fun item -> item * 3)
    |> List.filter (fun value -> value % 2 <> 0)
    |> List.fold (fun acum item -> acum + item) 0

[<EntryPoint>]
let main (argv: string []) : int =

    let list = [ 0 .. 10 ]

    for i in list do
        eval i

    let colection = [| 0 .. 10 |] // [||] -> Array, [] -> List
    Array.iter eval colection

    let result = pipe list

    printfn "Result: %A" result

    let agent = Agents.SimpleAgent()
    let _ = agent.Run().Post(Data.IntMessage 10)

    let pub = Publisher.single 10

    let _ =
        Observable.subscribe (fun item -> printfn "Item: %i" item) pub

    let filesA =
        async {
            let! files = retry { return! Data.Folder.make "resources" |> Files.readAsync }

            return! (async { return (files |> List.ofArray) })
        }

    let files =
        Data.Folder.make "resources" |> Files.read

    printfn "Files: %A" files

    let filesM = filesA |> Async.RunSynchronously

    printfn "FilesM: %A" filesM

    let maybe = Maybe.nothing
    printfn "Maybe Value %i" (Maybe.fromMaybe maybe 0)

    let oo = Simple("Hello")
    let message = oo.GetMessage()
    printfn "OO: %s" message

    0 // return an integer exit code
