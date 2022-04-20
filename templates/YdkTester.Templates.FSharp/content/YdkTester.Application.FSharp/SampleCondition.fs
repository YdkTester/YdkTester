
namespace YdkTesterNamespace

open YdkTester

type SampleCondition () =
    interface ICondition with
        member this.Name = "Sample Condition"
        member this.Check(deck : Deck, hand : CardSet) = true
