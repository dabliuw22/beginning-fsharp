namespace Actors

module Data =
    type Message =
        | StringMessage of string
        | IntMessage of int

module Agents =
    type SimpleAgent() =
        member _.Run() : MailboxProcessor<Data.Message> =
            MailboxProcessor<Data.Message>.Start
                (fun inbox ->
                    let rec recive () =
                        async {
                            let! message = inbox.Receive()

                            match message with
                            | Data.StringMessage s -> printfn "String: %s" s
                            | Data.IntMessage i -> printfn "Int %i" i

                            return! recive ()
                        }

                    recive ())
