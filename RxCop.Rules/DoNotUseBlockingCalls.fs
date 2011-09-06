namespace NorthHorizon.RxCop.Rules.Usage

open System
open System.Collections.Generic
open System.Reactive.Linq
open System.Reactive.Subjects
open Microsoft.FxCop.Sdk
open NorthHorizon.RxCop.Ops

type DoNotUseBlockingCalls() =
    inherit BaseIntrospectionRule("DoNotUseBlockingCalls", resourceFile, resourceAsm)

    let mutable behaviorSubjectTypeNode : TypeNode = null
    let mutable observableTypeNode : TypeNode = null
    let mutable blockingCalls : List<Member> = null

    let isBlockingCall (m : Method) (operands : ExpressionCollection) = 
        m.DeclaringType = observableTypeNode && 
        blockingCalls.Contains(getGeneric m) &&
        operands.[0].Type.Template <> behaviorSubjectTypeNode

    override this.BeforeAnalysis() =
        base.BeforeAnalysis()

        let blockingCallNames = [ "First"; "FirstOrDefault"; "Last"; "LastOrDefault"; "ToEnumerable"; "ToList"; "ToDictionary" ]
        
        behaviorSubjectTypeNode <- getTypeNode typedefof<BehaviorSubject<_>>
        observableTypeNode <- getTypeNode typedefof<Observable>

        blockingCalls <-
            blockingCallNames 
            |> List.map Identifier.For
            |> List.map observableTypeNode.GetMembersNamed
            |> List.fold (fun (s : List<Member>) x -> s.AddRange(x); s) (new List<Member>())

    member private this.Process (mc : MethodCall) : Option<Problem> =
        match mc.Callee with
        | :? MemberBinding as mb ->
            match mb.BoundMember with
            | :? Method as m when isBlockingCall m mc.Operands ->
                Some (new Problem(this.GetNamedResolution(getName m, m)))
            | _ -> None
        | _ -> None

    override this.Check(mbr : Member) : ProblemCollection =
        processMethodCalls this this.Process mbr
