namespace Parallel

open System.Threading.Tasks
open System.Drawing
open System.Drawing.Imaging

module Img =
    let convertImageTo3D (source: string) (destination: string) =
        let bitmap = Bitmap.FromFile(source) :?> Bitmap
        let w, h = bitmap.Width, bitmap.Height
        for x in 20 .. (w - 1) do
            for y in 0 .. (h-1) do
                let c1 = bitmap.GetPixel(x, y)
                let c2 = bitmap.GetPixel(x - 20, y)
                let color3D = Color.FromArgb(int c1.R, int c2.G, int c2.B)
                bitmap.SetPixel(x - 20 ,y,color3D)
        bitmap.Save(destination, ImageFormat.Jpeg)

    let setGrayscale (source: string) (destination: string) = 
        let bitmap = Bitmap.FromFile(source) :?> Bitmap
        let w, h = bitmap.Width, bitmap.Height
        for x = 0 to (w - 1) do
            for y = 0 to  (h - 1) do
                let c = bitmap.GetPixel(x, y)
                let gray = int(0.299 * float c.R + 0.587 * float c.G + 0.114 * float c.B)
                bitmap.SetPixel(x, y, Color.FromArgb(gray, gray, gray))
        bitmap.Save(destination, ImageFormat.Jpeg)

    let run unit : unit =
        try
            // El orden de ejecución de la tarea no está garantizado :(
            Parallel.Invoke(
                System.Action(fun () -> convertImageTo3D "/Users/will/Documents/workspaces/will-workspace/beginning-fsharp/resources/mona_lisa.jpeg" "/Users/will/Documents/workspaces/will-workspace/beginning-fsharp/resources/mona_lisa_3d.jpeg"),
                System.Action(fun () -> setGrayscale "/Users/will/Documents/workspaces/will-workspace/beginning-fsharp/resources/ginevra_de_benci.jpeg" "/Users/will/Documents/workspaces/will-workspace/beginning-fsharp/resources/ginevra_de_benci_gray.jpeg"))
        with
            | :? System.AggregateException -> printfn "Error"