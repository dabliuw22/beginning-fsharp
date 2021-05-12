namespace Retry

module Async =
    type RetryBuilder(max: int, sleep: int) =
        let rec retry n (task: Async<'a>) (continuation: 'a -> Async<'b>) =
            async {
                try
                    let! result = task
                    return! continuation result
                with error ->
                    if n = 0 then
                        return raise error
                    else
                        do! Async.Sleep sleep
                        return! retry (n - 1) task continuation
            }

        member this.ReturnFrom(f) = f
        member this.Delay(f) = async { return! f () }
        member this.Return(v) = async { return v }
        member this.Bind(task: Async<'a>, continuation: 'a -> Async<'b>) = retry max task continuation

    let retry = RetryBuilder(3, 100)
