(**
- title : FsReveal 
- description : Introduction to FsReveal
- author : Karlkim Suwanmongkol
- theme : Night
- transition : default

***

### What is an Actor?
![Complementary to functional programming](https://encrypted-tbn0.gstatic.com/images?q=tbn%3AANd9GcSwn5n9HU_0joYxU3BXU9ceO3AKq5m6FZOQMw&usqp=CAU)

- Encapsulates state and behavior.

- Complements functional paradigm.

- Elmish loop is an actor!

***

### Functional state

- Source of truth as database

![z](https://ars.els-cdn.com/content/image/3-s2.0-B9780123820204000239-f09-19-9780123820204.gif)
  

***

### FSharp Agents
<img src="https://assets.cdn.moviepilot.de/files/a5dc7c924fc63c5f1c7e1e35e10bc2222509d350c26bd399ca9b9d17e768/fill/1280/614/matrix-agent-smith.jpg" width="400"/>

*)
let counter =
    MailboxProcessor.Start(fun inbox ->
        let rec loop n =
            async {
                let! msg = inbox.Receive()
                match msg with
                //handle the message
                | Incr x -> return! loop (n + x)
                | Fetch (replyChannel) ->
                    replyChannel.Reply(n)
                    return! loop (n)
            }
        loop 0) 
(**
***

### Plain Objects vs Actors

| Objects       | Actors        |
| ------------- |-------------|
| Methods       |  Messages     |
| Bound to a memory Loc     | Can be completetly abstract      |
| References can be passed in proc | References can be passed out proc    |
| Exceptions handled by caller | Exceptions handled by parent|
| Type safety | Weak type safety|
| Non reactive | Reactive |


--- 

#### Objects vs Actors

| Objects       | Actors        |
| ------------- |-------------|
| Non thread safe | Thread Safe | 
| No persistence but ORM/Serialization | Offers persistence and restoring via Event Sourcing|


***
#### Actor frameworks

- Microsoft Orleans
- Akka.NET
- Proto.Actor

***

#### Use cases

- Micro-services/Nano Services
- Domain Driven Design w/ CQRS and E/S
- ETL and Pipelining
- Blue Green deployments
- Highly Available Distributed Systems
- Distributed Pub Sub and Locking
- General Distributed Computing
- Conflict Free Data Structures

***

### Akkling

*)
let counter =
    MailboxProcessor.Start(fun inbox ->
        let rec loop n =
            async {
                let! msg = inbox.Receive()
                match msg with
                //handle the message
                | Incr x -> return! loop (n + x)
                | Fetch (replyChannel) ->
                    replyChannel.Reply(n)
                    return! loop (n)
            }
        loop 0) 
(** 

***

### Streams

![](https://getakka.net/images/simple-graph-example.png)

*)
let graphFlow b sink =
    graph b {
        //block elements
        let! source = (Source.ofSeq rows)
        let! broadcast = Broadcast 2
        let! zip = ZipWith.create combineResults
        //glue the elements
        b.From broadcast =>> someFlow =>> anotherFlow =>> zip.In0 |> ignore
        b.From broadcast =>> flow3 =>> zip.In1 |> ignore
        b.From source =>> broadcast.In |> ignore
        b.From zip.Out =>> beforeSink =>> sink|> ignore
    }
(**
***

### Error handling

![](https://getakka.net/images/OneForOne.png)

***

### Remote Actor Systems

![](https://getakka.net/images/RemoteDeployment.png)

***

### Cluster Sharding

![](https://petabridge.com/images/2017/cluster-sharding-overview/sharded-system.png)

*)