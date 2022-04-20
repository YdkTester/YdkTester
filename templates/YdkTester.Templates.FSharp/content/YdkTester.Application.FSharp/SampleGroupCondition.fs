
namespace YdkTesterNamespace

open System
open System.Collections.Generic
open YdkTester

type SampleGroupCondition () =
    interface IGroupCondition with
        member this.Name = "Sample Condition"

        member this.Check(conditions : Dictionary<Type, bool>)
            = conditions.[typedefof<SampleCondition>]
