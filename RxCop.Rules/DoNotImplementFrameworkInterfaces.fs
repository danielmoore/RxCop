namespace NorthHorizon.RxCop.Rules.Usage

open System
open System.Collections.Generic
open System.Reactive.Linq
open System.Reactive.Subjects
open Microsoft.FxCop.Sdk
open NorthHorizon.RxCop.Ops

type DoNotImplementFrameworkInterfaces() =
    inherit BaseIntrospectionRule("DoNotImplementFrameworkInterfaces", resourceFile, resourceAsm)

    let mutable frameworkInterfaces : seq<InterfaceNode * string> = null

    let getTypeNodeBaseName (t : TypeNode) =
        match t with
        | :? InterfaceNode as i -> i.GetUnmangledNameWithoutTypeParameters().Substring(1)
        | _ -> t.GetUnmangledNameWithoutTypeParameters()

    override this.BeforeAnalysis() =
        base.BeforeAnalysis()

        frameworkInterfaces <- [
            typedefof<IObservable<_>>
            typedefof<IObserver<_>>
            typedefof<ISubject<_,_>>
            typedefof<ISubject<_>>
        ] |> Seq.map (fun t ->
            let i = getInterfaceNode t
            (i, getTypeNodeBaseName i))

    member private this.Process (t : TypeNode) (i : InterfaceNode) : Option<Problem> = 
        let i = match i.Template with :? InterfaceNode as i -> i | _ -> i
        let baseName = getTypeNodeBaseName t
        let result = 
            Seq.fold 
                (fun s x -> 
                    match (s, x) with
                    | (Some _, _) -> s
                    | (None, (fi, s)) -> if fi = i && (not (baseName.EndsWith s)) then Some (fi, t) else None
                    | _ -> None)
                None
                frameworkInterfaces
        
        if result.IsNone then None
        else Some (new Problem(this.GetNamedResolution(i.Name.Name), fst result.Value))

    override this.Check(tn : TypeNode) : ProblemCollection =
        let problems = new ProblemCollection(this)

        let baseInterfaces = tn.Interfaces |> Seq.collect (fun i -> i.Interfaces)

        tn.Interfaces
        |> Seq.filter (fun i -> (Seq.exists (fun b -> b = i) baseInterfaces) |> not)
        |> Seq.choose (this.Process tn) |> Seq.iter problems.Add

        problems