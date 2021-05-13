namespace IO.Handler

module Data =
    type Folder =
        private
        | Folder of value: string
        override this.ToString() =
            match this with
            | Folder v -> "Folder " + v

    module Folder =
        let make v = Folder v

        let value folder =
            match folder with
            | Folder v -> v


    type Line =
        private
        | Line of value: string
        override this.ToString() =
            match this with
            | Line v -> "Line " + v

    module Line =
        let make v = Line v

        let value line =
            match line with
            | Line v -> v

    type File =
        struct
            val Dir: Folder
            val Name: string
            val Lines: Line list
            new(f, n, l) = { Dir = f; Name = n; Lines = l }

            override this.ToString() =
                "File { Dir: "
                + this.Dir.ToString()
                + ", Name: "
                + this.Name
                + ", Lines: "
                + this.Lines.ToString()
                + " }"
        end

    type Extension =
        | All
        | Txt
        | Custom of string
        override this.ToString() =
            match this with
            | All -> "All"
            | Txt -> "Txt"
            | Custom v -> "Custom { " + v + " }"

    exception FolderNotFound of string

open System.IO

module Extension =
    let rec filter (ext: Data.Extension) (filenames: string list) : string list =
        match ext with
        | Data.Custom e ->
            filenames
            |> List.filter (fun fn -> Path.GetExtension fn = e)
        | Data.Txt -> filter (Data.Custom ".txt") filenames
        | _ -> filenames

module Lines =

    let get (path: string) : Data.Line list =
        try
            File.ReadAllLines path
        with :? IOException -> Array.empty
        |> List.ofArray
        |> List.map (fun l -> Data.Line.make l)

    let getAsync (folder: Data.Folder) (path: string) : Async<Data.File> =
        async { return (new Data.File(folder, Path.GetFileName(path), get path)) }

module Files =
    let private readFolder (folder: Data.Folder) : string list =
        try
            Directory.GetFiles(Data.Folder.value folder)
        with e ->
            printfn "Error: %s" e.Message
            raise <| Data.FolderNotFound e.Message
        |> List.ofArray

    let read (folder: Data.Folder) =
        readFolder folder
        |> List.map (fun p -> new Data.File(folder, Path.GetFileName(p), Lines.get p))

    let readAsync (folder: Data.Folder) : Async<Data.File []> =
        try
            readFolder folder
        with _ -> List.empty
        |> List.map (fun p -> Lines.getAsync folder p)
        |> Async.Parallel
