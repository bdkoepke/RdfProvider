module RdfProviderTests

open NUnit.Framework
open RdfProvider

type private PricingSource = RdfProvider<Schema="http://www.omg.org/spec/FIGI/20150501/PricingSources.rdf">
type private SecurityType = RdfProvider<Schema="http://www.omg.org/spec/FIGI/20150501/SecurityTypes.rdf">

[<Test>]
let ``Should have currency future security type`` () =
    Assert.AreEqual("Currency future.", string SecurityType.SecurityTypes.``Currency future.``)

[<Test>]
let ``Should have BAAO pricing source`` () =
    Assert.AreEqual("BAAO", string PricingSource.PricingSources.BAAO)
