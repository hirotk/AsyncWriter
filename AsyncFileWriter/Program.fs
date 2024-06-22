open System
open System.Threading
open System.Threading.Tasks
open AsyncFileWriter
    open ResultMd
    open FileMd


let _main () =
      try
          let dummyWriteWaitTime = 4500 // [msec]
          
          let cts = new CancellationTokenSource ()
          let pathStr = "./SaveData.txt"
          let text = "Hello Async World!"

          let deleteRes =
              match PathMd.exists pathStr with
              | true ->
                  PathMd.create pathStr
                  >>= FileMd.delete                  
              | false ->
                  Ok false

          let fileCreateRes = FileMd.create pathStr
                    
          let pathRes = PathMd.create pathStr
                
          
          let acRes =
                (fun _ path _ ->              
                    writeAsync cts path text
                        (Some dummyWriteWaitTime)
                )
                <%> deleteRes
                <*> pathRes
                <*> fileCreateRes
                |> join
                
          match acRes with
          | Error err ->
             Console.Error.WriteLine $"Error: {err}"             
          | Ok (acWrite, acCancel) ->
                            
              // Example 1
              (*
              printfn "Main: Start writeAsync"
              let writeTask = Async.StartImmediateAsTask acWrite                  
              let completed = ref false  
              for i in 0..9 do
                  printfn $"Main: {i}"
                  Thread.Sleep 1000
                  
                  if not completed.Value && 
                     writeTask.IsCompletedSuccessfully then
                      printfn "Main: Complete writeAsync"
                      completed.Value <- true                  
              *)
              
              // Example 2 (Delay Start and Join)
              
              let startTime = 2000
              let joinTime = 4000    // wait 2500 msec
              //let joinTime = 7000  // no wait
              let completed = ref false  
              let writeTaskOp = ref Option<Task>.None
              for i in 0..9 do
                  printfn $"Main: {i}"
                  Thread.Sleep 1000
                  
                  if startTime <= i*1000 && writeTaskOp.Value = None then
                        printfn "Main: Start writeAsync"
                        writeTaskOp.Value <- Some (Async.StartAsTask acWrite)
                        
                  if joinTime <= i*1000 && writeTaskOp.Value.IsSome &&                     
                        not writeTaskOp.Value.Value.IsCompleted then                      
                        printfn "Main: Waiting to synchronize"               
                        writeTaskOp.Value.Value.Wait()
              
                  if not completed.Value && writeTaskOp.Value.IsSome &&
                     writeTaskOp.Value.Value.IsCompletedSuccessfully then
                      printfn "Main: Complete writeAsync"
                      completed.Value <- true                  
              
                  
              // Example 3 (Cancel Async writing)
              (*
              //let cancelTime = 2000 // cancel
              let cancelTime = 6000 // too late to cancel
              printfn "Main: Start writeAsync"
              let writeTask = Async.StartImmediateAsTask acWrite
              let completed = ref false              
              let cancelTaskOp = ref Option<unit>.None
              for i in 0..9 do
                  printfn $"Main: {i}"
                  Thread.Sleep 1000
                  
                  if not completed.Value && cancelTaskOp.Value.IsNone &&
                     writeTask.IsCompletedSuccessfully then
                      printfn "Main: Complete writeAsync"
                      completed.Value <- true                    
                  
                  if cancelTime <= i*1000 && not completed.Value
                        && cancelTaskOp.Value.IsNone then
                      printfn "Main: Cancel writeAsync"
                      cancelTaskOp.Value <- Some (Async.StartImmediate acCancel)
              *)
      with
      | ex ->
          Console.Error.WriteLine $"Exception: {ex.Message}"
          


[<EntryPoint>]
let main _ =
    _main ()
    0