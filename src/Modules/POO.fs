namespace POO

type Simple(message: string) =
    do printfn "Constructor: %s" message // constructor body
    member _.GetMessage() : string = "Message: " + message

type Abstract =
    abstract member Print : unit

type DefaultAbstract() =
    member this.Print = (this :> Abstract).Print

    interface Abstract with
        member _.Print = printfn "Hello"
