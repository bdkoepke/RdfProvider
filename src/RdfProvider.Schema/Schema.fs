namespace RdfProvider

module Schema =

    open FSharp.Data

    type RdfSchema =
        XmlProvider<Sample="""<?xml version="1.0" encoding="UTF-8"?>
    <!DOCTYPE rdf:RDF [
        <!ENTITY rdf "http://www.w3.org/1999/02/22-rdf-syntax-ns#" >
        <!ENTITY rdfs "http://www.w3.org/2000/01/rdf-schema#" >
        <!ENTITY owl "http://www.w3.org/2002/07/owl#" >
        <!ENTITY xsd "http://www.w3.org/2001/XMLSchema#" >
        <!ENTITY figi-gii "http://www.omg.org/spec/FIGI/GlobalInstrumentIdentifiers/" >
        <!ENTITY figi-ps "http://www.omg.org/spec/FIGI/PricingSources/" >
        <!ENTITY figi-st "http://www.omg.org/spec/FIGI/SecurityTypes/" >
    ]>
    <rdf:RDF
         xmlns:rdf="http://www.w3.org/1999/02/22-rdf-syntax-ns#"
         xmlns:rdfs="http://www.w3.org/2000/01/rdf-schema#"
         xmlns:owl="http://www.w3.org/2002/07/owl#"
         xmlns:skos="http://www.w3.org/2004/02/skos/core#"
         xmlns:sm="http://www.omg.org/techprocess/ab/SpecificationMetadata/"
         xmlns:figi-gii="http://www.omg.org/spec/FIGI/GlobalInstrumentIdentifiers/"
         xmlns:figi-ps="http://www.omg.org/spec/FIGI/PricingSources/"
         xmlns:figi-st="http://www.omg.org/spec/FIGI/SecurityTypes/">

        <owl:Ontology rdf:about="http://www.omg.org/spec/FIGI/PricingSources/">
            <rdfs:label>FIGI Pricing Sources Vocabulary</rdfs:label>
            <sm:filename rdf:datatype="&xsd;string">SecurityTypes.rdf</sm:filename>
        </owl:Ontology>

        <owl:NamedIndividual rdf:about="&figi-ps;AAAA">
            <rdf:type rdf:resource="&figi-gii;PricingSource"/>
            <figi-gii:hasUniqueTextIdentifier>AAAA</figi-gii:hasUniqueTextIdentifier>
            <skos:prefLabel>asset mgmt consult</skos:prefLabel>
        </owl:NamedIndividual>

        <owl:NamedIndividual rdf:about="&figi-st;absCard">
            <rdf:type rdf:resource="&figi-gii;SecurityType"/>
            <skos:prefLabel>abs card</skos:prefLabel>
            <figi-gii:hasUniqueTextIdentifier>ABS Card</figi-gii:hasUniqueTextIdentifier>
            <skos:definition>Asset Backed Security (backed by Other less common loans and receivables) i.e. Charge accounts, Reference Notes, etc.</skos:definition>
        </owl:NamedIndividual>

        <owl:NamedIndividual rdf:about="http://www.omg.org/spec/FIGI/PricingSources/BABC">
            <rdf:type rdf:resource="http://www.omg.org/spec/FIGI/GlobalInstrumentIdentifiers/PricingSource"/>
            <figi-gii:hasUniqueTextIdentifier>BABC</figi-gii:hasUniqueTextIdentifier>
            <skos:prefLabel>banco abc brasil</skos:prefLabel>
            <skos:prefLabel>babson capital mgmt</skos:prefLabel>
        </owl:NamedIndividual>
    </rdf:RDF>
    """>
