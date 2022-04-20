
namespace YdkTesterNamespace

open System.Diagnostics
open YdkTester.Runner

module Program =
    [<EntryPoint>]
    let main argv =
        let mutable options = new Options (DeckPath = Cards.DeckPath, NumberOfIterations = 100000)

        if Debugger.IsAttached then
            options.Debug <- true
            options.NumberOfIterations <- 10

        Runner.Run(options)
        0
