namespace NorthHorizon.RxCop.Rules.Usage

open System
open System.Collections.Generic
open System.Reactive.Linq
open System.Reactive.Subjects
open Microsoft.FxCop.Sdk
open NorthHorizon.RxCop.Ops

type UseExplicitSchedulers() =
    inherit BaseIntrospectionRule("UseExplicitSchedulers", resourceFile, resourceAsm)

    let mutable observableTypeNode : TypeNode = null
    let mutable implicitConcurrentCalls : seq<Method> = null

    let isConcurrentCall (m : Method) = 
        let m = getGeneric m
        m.DeclaringType = observableTypeNode && 
        (Seq.exists (fun c -> c = m) implicitConcurrentCalls)

    override this.BeforeAnalysis() =
        base.BeforeAnalysis()
        
        observableTypeNode <- getTypeNode typedefof<Observable>

        let nonConcurrentCalls = [ "Range"; "Return"; "ToAsync"; "Repeat"; "Replay"; "Merge" ]

        let hasSchedulerParameter (m : Method) =
            Seq.exists (fun (p : Parameter) -> p.Name.Name = "scheduler") m.Parameters

        implicitConcurrentCalls <- observableTypeNode.Members
            |> Seq.choose (function :? Method as m -> Some m | _ -> None)
            |> Seq.groupBy getName
            |> Seq.filter (fst >> (fun i -> Seq.exists (fun c -> c = i) nonConcurrentCalls) >> not)
            |> Seq.map snd
            |> Seq.filter (Seq.exists hasSchedulerParameter)
            |> Seq.collect (Seq.filter (hasSchedulerParameter >> not))

    member private this.Process (mc : MethodCall) : Option<Problem> =
        match mc.Callee with
        | :? MemberBinding as mb ->
            match mb.BoundMember with
            | :? Method as m when isConcurrentCall m ->
                Some (new Problem(this.GetResolution(), m))
            | _ -> None
        | _ -> None

    override this.Check(mbr : Member) : ProblemCollection =
        processMethodCalls this this.Process mbr
