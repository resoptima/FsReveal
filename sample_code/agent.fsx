type Msg =
    | Incr of int
    | Fetch of AsyncReplyChannel<int>

let counter =
    MailboxProcessor.Start(fun inbox ->
        let rec loop n =
            async {
                let! msg = inbox.Receive()

                match msg with
                | Incr x -> return! loop (n + x)
                | Fetch (replyChannel) ->
                    replyChannel.Reply(n)
                    return! loop (n)
            }

        loop 0)

counter.Post(Incr 7)
counter.Post(Incr 50)

Fetch
|> counter.PostAndReply 
|> printfn "%A"