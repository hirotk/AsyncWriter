module AsyncFileWriter.PathMd

open System.IO
open ResultMd


type Path =
    | PathC of body: string


let getExt : string list->string->
        Result<string, ErrMsg>=
    fun supportedExts path ->
        try
            Path.GetExtension path
            |> fun ext ->        
                match ext with
                 | s when List.contains s supportedExts ->
                     Ok ext
                 | _ ->
                     Error $"{ext} file is not supported!"
        with
        | ex ->
            Error ex.Message

let getFullPath : string->
        Result<string, ErrMsg> =
    fun pathStr ->
        try
            Path.GetFullPath pathStr
            |> fun fullPath ->
                match Path.Exists fullPath with
                | true -> Ok fullPath
                | false -> Error $"{pathStr} does not exist!"
        with
        | ex ->
            Error ex.Message

let exists : string -> bool =
    fun pathStr ->
        match getFullPath pathStr with
        | Ok _ -> true
        | Error _ -> false        


let create : string->
        Result<Path, ErrMsg> =
    fun pathStr ->
        getFullPath pathStr
        <&> fun _ -> PathC pathStr 

let value : Path->
        string =
    fun path ->
        match path with
        | PathC body -> body

