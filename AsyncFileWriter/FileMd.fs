module AsyncFileWriter.FileMd

open System.IO
open System.Threading
open System.Threading.Tasks

open ResultMd
open PathMd


let create :
        string->
        Result<bool, ErrMsg> =
    fun pathStr ->
        match PathMd.exists pathStr with
        | false ->
            try
                use fs = File.Create(pathStr)
                
                Ok true
            with
            | ex ->
                Error ex.Message
        | true ->
            Error $"{pathStr} already exists!"

let delete :
        Path->
        Result<bool, ErrMsg> =
    fun path ->
        try            
            File.Delete(PathMd.value path)
            Ok true
        with
        | ex ->
            Error ex.Message

let writeAsync :
        CancellationTokenSource->
        Path->
        string->
        int32 option->
        Result<Async<Task>*Async<unit>, string> =
    fun cts path text waitTimeOp ->
        try
            let pathStr = PathMd.value path
            let acWrite =
                async {
                    match waitTimeOp with
                    | Some time -> do! Async.Sleep(time)
                    | None -> ()                    
                    
                    return File.WriteAllTextAsync(pathStr, text, cts.Token)                    
                }
            
            let acCancel =
                async {
                    return cts.Cancel()               
                }      
            
            Ok (acWrite, acCancel)
        with
        | ex ->
            Error ex.Message