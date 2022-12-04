module RdfProviderImplementation

open System
open System.Collections.Concurrent
open System.Reflection
open FSharp.Core.CompilerServices
open RdfProvider.Schema
open ProviderImplementation
open ProviderImplementation.ProvidedTypes

type NameSpace = private NameSpace of string

module NameSpace =
    let ofString = NameSpace
    let toString (NameSpace nameSpace) = nameSpace

type Documentation = private Documentation of string

module Documentation =
    let ofString = Documentation
    let toString (Documentation documentation) = documentation

type Label = private Label of string

module Label =
    let ofString = Label
    let toString (Label label) = label

type Name = private Name of string

module Name =
    let ofString = Name
    let toString (Name name) = name

type EnumerationError = Empty of Name: Name

let internal createEnumerationType
    (assembly: Assembly)
    (nameSpace: NameSpace)
    (name: Name)
    (documentation: Documentation)
    (enumerators: Map<Label, Documentation>)
    =
    match enumerators |> Map.toList with
    | [] -> Error <| Empty(name)
    | enumerators ->
        let providedEnumType =
            ProvidedTypeDefinition(
                assembly,
                NameSpace.toString nameSpace,
                Name.toString name,
                baseType = Some typeof<Enum>,
                hideObjectMethods = true,
                isErased = false
            )

        let description = Documentation.toString documentation
        providedEnumType.AddXmlDoc(description)

        enumerators
        |> List.map (fun (label, description) ->
            let label = Label.toString label
            let providedField = ProvidedField.Literal(label, providedEnumType, label)
            let description = Documentation.toString description

            match description with
            | "" -> ()
            | _ -> providedField.AddXmlDoc(description)

            providedField)
        |> providedEnumType.AddMembers

        providedEnumType |> Ok

[<TypeProvider>]
type BasicGenerativeProvider(config: TypeProviderConfig) as this =
    inherit
        TypeProviderForNamespaces(
            config,
            assemblyReplacementMap = [ ("RdfProvider.DesignTime", "RdfProvider.Runtime") ]
        )

    let ns = NameSpace.ofString "RdfProvider"

    let createRootType (typeName, schemaPath: string) =
        let tempAssembly = ProvidedAssembly()

        let schema = RdfSchema.Load(schemaPath)

        let rootType =
            ProvidedTypeDefinition(
                tempAssembly,
                NameSpace.toString ns,
                typeName,
                baseType = Some typeof<obj>,
                hideObjectMethods = true,
                isErased = false
            )

        let mutable typeCache = Map.empty<string, Type>

        let enumerators =
            schema.NamedIndividuals
            |> Array.map (fun x ->
                Label.ofString x.HasUniqueTextIdentifier,
                match x.Definition with
                | Some d -> Documentation.ofString d
                | None -> Documentation.ofString (x.PrefLabels |> Array.head))
            |> Map.ofArray

        let name = schema.Ontology.Filename.Value.Replace(".rdf", "") |> Name.ofString
        let documentation = schema.Ontology.Label |> Documentation.ofString

        let providedType =
            createEnumerationType tempAssembly ns name documentation enumerators

        match providedType with
        | Ok providedType ->
            typeCache <- typeCache |> Map.add (Name.toString name) providedType
            rootType.AddMember providedType
        | Error errors -> failwithf "%A" errors

        tempAssembly.AddTypes [ rootType ]
        rootType

    let assembly = Assembly.GetExecutingAssembly()
    // check we contain a copy of runtime files, and are not referencing the runtime DLL
    do assert (typeof<RdfProvider.Runtime>.Assembly.GetName().Name = assembly.GetName().Name)

    let cache = ConcurrentDictionary<string, Lazy<ProvidedTypeDefinition>>()

    let rootType =
        let rootType =
            ProvidedTypeDefinition(
                assembly,
                NameSpace.toString ns,
                "RdfProvider",
                Some typeof<obj>,
                hideObjectMethods = true,
                isErased = false
            )

        let staticParams = [ ProvidedStaticParameter("Schema", typeof<string>) ]

        rootType.AddXmlDoc
            """
        <summary>Rdf Provider types based on rdf schema file.</summary>
        <param name='SchemaFile'>The full path to the schema file to base this provider on.</param>
        """

        rootType.DefineStaticParameters(
            parameters = staticParams,
            instantiationFunction =
                fun typeName args ->
                    cache
                        .GetOrAdd(
                            typeName,
                            lazy (createRootType (typeName, (string args.[0])))
                        )
                        .Value
        )

        rootType

    do this.AddNamespace(NameSpace.toString ns, [ rootType ])
