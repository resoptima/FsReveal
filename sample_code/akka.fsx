#r "nuget:Akkling"
open Akkling

let system =
    System.create "my-system"
    <| Configuration.defaultConfig ()

let aref =
    spawnAnonymous system
    <| props (actorOf (fun m -> printfn "%s" m |> ignored))

aref <! "hello world"
//aref <! 1 // ERROR: we have statically typed actors here


type Message =
    | Hi
    | Greet of string

let rec greeter lastKnown =
    function
    | Hi -> printfn "Who sent Hi? %s?" lastKnown |> ignored
    | Greet (who) ->
        printfn "%s sends greetings" who
        become (greeter who)

let bref =
    spawn system "greeter"
    <| props (actorOf (greeter "Unknown"))

bref <! Greet "Tom"
bref <! Greet "Jane"
bref <! Hi

type Msg =
    | Incr of int
    | Fetch

let counter =
    spawn system "counter-actor"
    <| props (fun context ->
        let rec loop (state) =
            actor {
                let! msg = context.Receive()

                match msg with
                | Incr x -> return! loop (state + x) // effect - stops current actor
                | Fetch -> context.Sender() <! state // effect - marks message as unhandled
            }

        loop 0)

counter <! (Incr 7)
counter <! (Incr 50)

async {
    let! res = counter <? Fetch
    printfn "%A" res
}
|> Async.RunSynchronously
