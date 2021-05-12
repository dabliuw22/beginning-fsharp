module Reactive

open System
open System.Threading.Tasks
open System.Reactive.Concurrency
open System.Reactive.Subjects
open System.Reactive.Linq
open System.Reactive.Threading.Tasks
open FSharp.Control.Reactive

module Publisher =
    let single<'a> (item: 'a) =
        (Observable.single item)
            .SubscribeOn(ThreadPoolScheduler.Instance)

    let io<'a> (f: (unit -> 'a)) =
        (Observable.single (f ()))
            .SubscribeOn(NewThreadScheduler.Default)

    let ofSeq<'a> (items: 'a seq) = Observable.ofSeq items

    let async<'a> (task: Task<'a>) = Observable.FromAsync(fun () -> task)

    let toAsync<'a> (observable: IObservable<'a>) =
        observable
        |> TaskObservableExtensions.ToTask
        |> Async.AwaitTask
