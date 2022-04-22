/*
   Copyright 2012-2022 Marco De Salvo

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/

using Microsoft.VisualStudio.TestTools.UnitTesting;
using RDFSharp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace RDFSharp.Test.Model
{
    [TestClass]
    public class RDFXoneConstraintTest
    {
        #region Tests
        [TestMethod]
        public void ShouldCreateXoneConstraint()
        {
            RDFXoneConstraint xoneConstraint = new RDFXoneConstraint();

            Assert.IsNotNull(xoneConstraint);
            Assert.IsNotNull(xoneConstraint.XoneShapes);
            Assert.IsTrue(xoneConstraint.XoneShapes.Count == 0);
        }

        [TestMethod]
        public void ShouldAddShape()
        {
            RDFXoneConstraint xoneConstraint = new RDFXoneConstraint();
            xoneConstraint.AddShape(new RDFNodeShape(new RDFResource("ex:NodeShape1")));
            xoneConstraint.AddShape(new RDFNodeShape(new RDFResource("ex:NodeShape2")));
            xoneConstraint.AddShape(new RDFNodeShape(new RDFResource("ex:NodeShape1"))); //Will be discarded
            xoneConstraint.AddShape(null); //Will be discarded

            Assert.IsNotNull(xoneConstraint);
            Assert.IsNotNull(xoneConstraint.XoneShapes);
            Assert.IsTrue(xoneConstraint.XoneShapes.Count == 2);
        }

        [TestMethod]
        public void ShouldExportXoneConstraint()
        {
            RDFXoneConstraint xoneConstraint = new RDFXoneConstraint();
            xoneConstraint.AddShape(new RDFNodeShape(new RDFResource("ex:NodeShape1")));
            xoneConstraint.AddShape(new RDFNodeShape(new RDFResource("ex:NodeShape2")));
            RDFGraph graph = xoneConstraint.ToRDFGraph(new RDFNodeShape(new RDFResource("ex:NodeShape")));

            Assert.IsNotNull(graph);
            Assert.IsTrue(graph.TriplesCount == 7);
            Assert.IsTrue(graph.Triples.Any(t => t.Value.Subject.Equals(new RDFResource("ex:NodeShape"))
                                                    && t.Value.Predicate.Equals(RDFVocabulary.SHACL.XONE)
                                                        && t.Value.Object is RDFResource objRes && objRes.IsBlank));
            Assert.IsTrue(graph.Triples.Any(t => t.Value.Subject is RDFResource subjRes && subjRes.IsBlank
                                                    && t.Value.Predicate.Equals(RDFVocabulary.RDF.TYPE)
                                                        && t.Value.Object.Equals(RDFVocabulary.RDF.LIST))); //2 occurrences of this
            Assert.IsTrue(graph.Triples.Any(t => t.Value.Subject is RDFResource subjRes && subjRes.IsBlank
                                                    && t.Value.Predicate.Equals(RDFVocabulary.RDF.FIRST)
                                                        && t.Value.Object.Equals(new RDFResource("ex:NodeShape1"))));
            Assert.IsTrue(graph.Triples.Any(t => t.Value.Subject is RDFResource subjRes && subjRes.IsBlank
                                                    && t.Value.Predicate.Equals(RDFVocabulary.RDF.REST)
                                                        && t.Value.Object is RDFResource objRes && objRes.IsBlank));
            Assert.IsTrue(graph.Triples.Any(t => t.Value.Subject is RDFResource subjRes && subjRes.IsBlank
                                                    && t.Value.Predicate.Equals(RDFVocabulary.RDF.FIRST)
                                                        && t.Value.Object.Equals(new RDFResource("ex:NodeShape2"))));
            Assert.IsTrue(graph.Triples.Any(t => t.Value.Subject is RDFResource subjRes && subjRes.IsBlank
                                                    && t.Value.Predicate.Equals(RDFVocabulary.RDF.REST)
                                                        && t.Value.Object.Equals(RDFVocabulary.RDF.NIL)));
        }

        //NS-CONFORMS:TRUE

        [TestMethod]
        public void ShouldConformNodeShapeWithClassTarget()
        {
            //DataGraph
            RDFGraph dataGraph = new RDFGraph().SetContext(new Uri("ex:DataGraph"));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Man"), RDFVocabulary.RDFS.SUB_CLASS_OF, new RDFResource("ex:Person")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Woman"), RDFVocabulary.RDFS.SUB_CLASS_OF, new RDFResource("ex:Person")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Alice"), RDFVocabulary.RDF.TYPE, new RDFResource("ex:Woman")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Bob"), RDFVocabulary.RDF.TYPE, new RDFResource("ex:Man")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Alice"), RDFVocabulary.FOAF.KNOWS, new RDFResource("ex:Bob")));

            //ShapesGraph
            RDFShapesGraph shapesGraph = new RDFShapesGraph(new RDFResource("ex:ShapesGraph"));
            RDFNodeShape nodeShape1 = new RDFNodeShape(new RDFResource("ex:NodeShape1"));
            nodeShape1.AddConstraint(new RDFMinLengthConstraint(4));
            RDFNodeShape nodeShape2 = new RDFNodeShape(new RDFResource("ex:NodeShape2"));
            nodeShape2.AddConstraint(new RDFMaxLengthConstraint(4));
            RDFNodeShape nodeShape = new RDFNodeShape(new RDFResource("ex:NodeShape"));
            nodeShape.AddTarget(new RDFTargetClass(new RDFResource("ex:Person")));
            nodeShape.AddConstraint(new RDFXoneConstraint().AddShape(new RDFResource("ex:NodeShape1"))
                                                           .AddShape(new RDFResource("ex:NodeShape2")));
            shapesGraph.AddShape(nodeShape);
            shapesGraph.AddShape(nodeShape1);
            shapesGraph.AddShape(nodeShape2);

            //Validate
            RDFValidationReport validationReport = shapesGraph.Validate(dataGraph);

            Assert.IsNotNull(validationReport);
            Assert.IsTrue(validationReport.Conforms);
        }

        [TestMethod]
        public void ShouldConformNodeShapeWithNodeTarget()
        {
            //DataGraph
            RDFGraph dataGraph = new RDFGraph().SetContext(new Uri("ex:DataGraph"));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Man"), RDFVocabulary.RDFS.SUB_CLASS_OF, new RDFResource("ex:Person")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Woman"), RDFVocabulary.RDFS.SUB_CLASS_OF, new RDFResource("ex:Person")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Alice"), RDFVocabulary.RDF.TYPE, new RDFResource("ex:Woman")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Bob"), RDFVocabulary.RDF.TYPE, new RDFResource("ex:Man")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Alice"), RDFVocabulary.FOAF.KNOWS, new RDFResource("ex:Bob")));

            //ShapesGraph
            RDFShapesGraph shapesGraph = new RDFShapesGraph(new RDFResource("ex:ShapesGraph"));
            RDFNodeShape nodeShape1 = new RDFNodeShape(new RDFResource("ex:NodeShape1"));
            nodeShape1.AddConstraint(new RDFMinLengthConstraint(4));
            RDFNodeShape nodeShape2 = new RDFNodeShape(new RDFResource("ex:NodeShape2"));
            nodeShape2.AddConstraint(new RDFMaxLengthConstraint(4));
            RDFNodeShape nodeShape = new RDFNodeShape(new RDFResource("ex:NodeShape"));
            nodeShape.AddTarget(new RDFTargetNode(new RDFResource("ex:Alice")));
            nodeShape.AddConstraint(new RDFXoneConstraint().AddShape(new RDFResource("ex:NodeShape1"))
                                                           .AddShape(new RDFResource("ex:NodeShape2")));
            shapesGraph.AddShape(nodeShape);
            shapesGraph.AddShape(nodeShape1);
            shapesGraph.AddShape(nodeShape2);

            //Validate
            RDFValidationReport validationReport = shapesGraph.Validate(dataGraph);

            Assert.IsNotNull(validationReport);
            Assert.IsTrue(validationReport.Conforms);
        }

        [TestMethod]
        public void ShouldConformNodeShapeWithSubjectsOfTarget()
        {
            //DataGraph
            RDFGraph dataGraph = new RDFGraph().SetContext(new Uri("ex:DataGraph"));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Man"), RDFVocabulary.RDFS.SUB_CLASS_OF, new RDFResource("ex:Person")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Woman"), RDFVocabulary.RDFS.SUB_CLASS_OF, new RDFResource("ex:Person")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Alice"), RDFVocabulary.RDF.TYPE, new RDFResource("ex:Woman")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Bob"), RDFVocabulary.RDF.TYPE, new RDFResource("ex:Man")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Alice"), RDFVocabulary.FOAF.KNOWS, new RDFResource("ex:Bob")));

            //ShapesGraph
            RDFShapesGraph shapesGraph = new RDFShapesGraph(new RDFResource("ex:ShapesGraph"));
            RDFNodeShape nodeShape1 = new RDFNodeShape(new RDFResource("ex:NodeShape1"));
            nodeShape1.AddConstraint(new RDFMinLengthConstraint(4));
            RDFNodeShape nodeShape2 = new RDFNodeShape(new RDFResource("ex:NodeShape2"));
            nodeShape2.AddConstraint(new RDFMaxLengthConstraint(4));
            RDFNodeShape nodeShape = new RDFNodeShape(new RDFResource("ex:NodeShape"));
            nodeShape.AddTarget(new RDFTargetSubjectsOf(RDFVocabulary.FOAF.KNOWS));
            nodeShape.AddConstraint(new RDFXoneConstraint().AddShape(new RDFResource("ex:NodeShape1"))
                                                           .AddShape(new RDFResource("ex:NodeShape2")));
            shapesGraph.AddShape(nodeShape);
            shapesGraph.AddShape(nodeShape1);
            shapesGraph.AddShape(nodeShape2);

            //Validate
            RDFValidationReport validationReport = shapesGraph.Validate(dataGraph);

            Assert.IsNotNull(validationReport);
            Assert.IsTrue(validationReport.Conforms);
        }

        [TestMethod]
        public void ShouldConformNodeShapeWithObjectsOfTarget()
        {
            //DataGraph
            RDFGraph dataGraph = new RDFGraph().SetContext(new Uri("ex:DataGraph"));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Man"), RDFVocabulary.RDFS.SUB_CLASS_OF, new RDFResource("ex:Person")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Woman"), RDFVocabulary.RDFS.SUB_CLASS_OF, new RDFResource("ex:Person")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Alice"), RDFVocabulary.RDF.TYPE, new RDFResource("ex:Woman")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Bob"), RDFVocabulary.RDF.TYPE, new RDFResource("ex:Man")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Alice"), RDFVocabulary.FOAF.KNOWS, new RDFResource("ex:Bob")));

            //ShapesGraph
            RDFShapesGraph shapesGraph = new RDFShapesGraph(new RDFResource("ex:ShapesGraph"));
            RDFNodeShape nodeShape1 = new RDFNodeShape(new RDFResource("ex:NodeShape1"));
            nodeShape1.AddConstraint(new RDFMinLengthConstraint(4));
            RDFNodeShape nodeShape2 = new RDFNodeShape(new RDFResource("ex:NodeShape2"));
            nodeShape2.AddConstraint(new RDFMaxLengthConstraint(4));
            RDFNodeShape nodeShape = new RDFNodeShape(new RDFResource("ex:NodeShape"));
            nodeShape.AddTarget(new RDFTargetObjectsOf(RDFVocabulary.FOAF.KNOWS));
            nodeShape.AddConstraint(new RDFXoneConstraint().AddShape(new RDFResource("ex:NodeShape1"))
                                                           .AddShape(new RDFResource("ex:NodeShape2")));
            shapesGraph.AddShape(nodeShape);
            shapesGraph.AddShape(nodeShape1);
            shapesGraph.AddShape(nodeShape2);

            //Validate
            RDFValidationReport validationReport = shapesGraph.Validate(dataGraph);

            Assert.IsNotNull(validationReport);
            Assert.IsTrue(validationReport.Conforms);
        }

        //PS-CONFORMS:TRUE

        [TestMethod]
        public void ShouldConformPropertyShapeWithClassTarget()
        {
            //DataGraph
            RDFGraph dataGraph = new RDFGraph().SetContext(new Uri("ex:DataGraph"));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Man"), RDFVocabulary.RDFS.SUB_CLASS_OF, new RDFResource("ex:Person")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Woman"), RDFVocabulary.RDFS.SUB_CLASS_OF, new RDFResource("ex:Person")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Alice"), RDFVocabulary.RDF.TYPE, new RDFResource("ex:Woman")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Bob"), RDFVocabulary.RDF.TYPE, new RDFResource("ex:Man")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Alice"), RDFVocabulary.FOAF.KNOWS, new RDFResource("ex:Bob")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Bob"), RDFVocabulary.FOAF.KNOWS, new RDFResource("ex:Alice")));

            //ShapesGraph
            RDFShapesGraph shapesGraph = new RDFShapesGraph(new RDFResource("ex:ShapesGraph"));
            RDFNodeShape nodeShape1 = new RDFNodeShape(new RDFResource("ex:NodeShape1"));
            nodeShape1.AddConstraint(new RDFMinLengthConstraint(4));
            RDFNodeShape nodeShape2 = new RDFNodeShape(new RDFResource("ex:NodeShape2"));
            nodeShape2.AddConstraint(new RDFMaxLengthConstraint(4));
            RDFPropertyShape propertyShape = new RDFPropertyShape(new RDFResource("ex:PropertyShape"), RDFVocabulary.FOAF.KNOWS);
            propertyShape.AddTarget(new RDFTargetClass(new RDFResource("ex:Person")));
            propertyShape.AddConstraint(new RDFXoneConstraint().AddShape(new RDFResource("ex:NodeShape1"))
                                                               .AddShape(new RDFResource("ex:NodeShape2")));
            shapesGraph.AddShape(propertyShape);
            shapesGraph.AddShape(nodeShape1);
            shapesGraph.AddShape(nodeShape2);

            //Validate
            RDFValidationReport validationReport = shapesGraph.Validate(dataGraph);

            Assert.IsNotNull(validationReport);
            Assert.IsTrue(validationReport.Conforms);
        }

        [TestMethod]
        public void ShouldConformPropertyShapeWithNodeTarget()
        {
            //DataGraph
            RDFGraph dataGraph = new RDFGraph().SetContext(new Uri("ex:DataGraph"));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Man"), RDFVocabulary.RDFS.SUB_CLASS_OF, new RDFResource("ex:Person")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Woman"), RDFVocabulary.RDFS.SUB_CLASS_OF, new RDFResource("ex:Person")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Alice"), RDFVocabulary.RDF.TYPE, new RDFResource("ex:Woman")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Bob"), RDFVocabulary.RDF.TYPE, new RDFResource("ex:Man")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Alice"), RDFVocabulary.FOAF.KNOWS, new RDFResource("ex:Bob")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Bob"), RDFVocabulary.FOAF.KNOWS, new RDFResource("ex:Alice")));

            //ShapesGraph
            RDFShapesGraph shapesGraph = new RDFShapesGraph(new RDFResource("ex:ShapesGraph"));
            RDFNodeShape nodeShape1 = new RDFNodeShape(new RDFResource("ex:NodeShape1"));
            nodeShape1.AddConstraint(new RDFMinLengthConstraint(4));
            RDFNodeShape nodeShape2 = new RDFNodeShape(new RDFResource("ex:NodeShape2"));
            nodeShape2.AddConstraint(new RDFMaxLengthConstraint(4));
            RDFPropertyShape propertyShape = new RDFPropertyShape(new RDFResource("ex:PropertyShape"), RDFVocabulary.FOAF.KNOWS);
            propertyShape.AddTarget(new RDFTargetNode(new RDFResource("ex:Alice")));
            propertyShape.AddConstraint(new RDFXoneConstraint().AddShape(new RDFResource("ex:NodeShape1"))
                                                               .AddShape(new RDFResource("ex:NodeShape2")));
            shapesGraph.AddShape(propertyShape);
            shapesGraph.AddShape(nodeShape1);
            shapesGraph.AddShape(nodeShape2);

            //Validate
            RDFValidationReport validationReport = shapesGraph.Validate(dataGraph);

            Assert.IsNotNull(validationReport);
            Assert.IsTrue(validationReport.Conforms);
        }

        [TestMethod]
        public void ShouldConformPropertyShapeWithSubjectsOfTarget()
        {
            //DataGraph
            RDFGraph dataGraph = new RDFGraph().SetContext(new Uri("ex:DataGraph"));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Man"), RDFVocabulary.RDFS.SUB_CLASS_OF, new RDFResource("ex:Person")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Woman"), RDFVocabulary.RDFS.SUB_CLASS_OF, new RDFResource("ex:Person")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Alice"), RDFVocabulary.RDF.TYPE, new RDFResource("ex:Woman")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Bob"), RDFVocabulary.RDF.TYPE, new RDFResource("ex:Man")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Alice"), RDFVocabulary.FOAF.KNOWS, new RDFResource("ex:Bob")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Bob"), RDFVocabulary.FOAF.KNOWS, new RDFResource("ex:Alice")));

            //ShapesGraph
            RDFShapesGraph shapesGraph = new RDFShapesGraph(new RDFResource("ex:ShapesGraph"));
            RDFNodeShape nodeShape1 = new RDFNodeShape(new RDFResource("ex:NodeShape1"));
            nodeShape1.AddConstraint(new RDFMinLengthConstraint(4));
            RDFNodeShape nodeShape2 = new RDFNodeShape(new RDFResource("ex:NodeShape2"));
            nodeShape2.AddConstraint(new RDFMaxLengthConstraint(4));
            RDFPropertyShape propertyShape = new RDFPropertyShape(new RDFResource("ex:PropertyShape"), RDFVocabulary.FOAF.KNOWS);
            propertyShape.AddTarget(new RDFTargetSubjectsOf(RDFVocabulary.FOAF.KNOWS));
            propertyShape.AddConstraint(new RDFXoneConstraint().AddShape(new RDFResource("ex:NodeShape1"))
                                                               .AddShape(new RDFResource("ex:NodeShape2")));
            shapesGraph.AddShape(propertyShape);
            shapesGraph.AddShape(nodeShape1);
            shapesGraph.AddShape(nodeShape2);

            //Validate
            RDFValidationReport validationReport = shapesGraph.Validate(dataGraph);

            Assert.IsNotNull(validationReport);
            Assert.IsTrue(validationReport.Conforms);
        }

        [TestMethod]
        public void ShouldConformPropertyShapeWithObjectsOfTarget()
        {
            //DataGraph
            RDFGraph dataGraph = new RDFGraph().SetContext(new Uri("ex:DataGraph"));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Man"), RDFVocabulary.RDFS.SUB_CLASS_OF, new RDFResource("ex:Person")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Woman"), RDFVocabulary.RDFS.SUB_CLASS_OF, new RDFResource("ex:Person")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Alice"), RDFVocabulary.RDF.TYPE, new RDFResource("ex:Woman")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Bob"), RDFVocabulary.RDF.TYPE, new RDFResource("ex:Man")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Alice"), RDFVocabulary.FOAF.KNOWS, new RDFResource("ex:Bob")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Bob"), RDFVocabulary.FOAF.KNOWS, new RDFResource("ex:Alice")));

            //ShapesGraph
            RDFShapesGraph shapesGraph = new RDFShapesGraph(new RDFResource("ex:ShapesGraph"));
            RDFNodeShape nodeShape1 = new RDFNodeShape(new RDFResource("ex:NodeShape1"));
            nodeShape1.AddConstraint(new RDFMinLengthConstraint(4));
            RDFNodeShape nodeShape2 = new RDFNodeShape(new RDFResource("ex:NodeShape2"));
            nodeShape2.AddConstraint(new RDFMaxLengthConstraint(4));
            RDFPropertyShape propertyShape = new RDFPropertyShape(new RDFResource("ex:PropertyShape"), RDFVocabulary.FOAF.KNOWS);
            propertyShape.AddTarget(new RDFTargetObjectsOf(RDFVocabulary.FOAF.KNOWS));
            propertyShape.AddConstraint(new RDFXoneConstraint().AddShape(new RDFResource("ex:NodeShape1"))
                                                               .AddShape(new RDFResource("ex:NodeShape2")));
            shapesGraph.AddShape(propertyShape);
            shapesGraph.AddShape(nodeShape1);
            shapesGraph.AddShape(nodeShape2);

            //Validate
            RDFValidationReport validationReport = shapesGraph.Validate(dataGraph);

            Assert.IsNotNull(validationReport);
            Assert.IsTrue(validationReport.Conforms);
        }

        //NS-CONFORMS:FALSE

        [TestMethod]
        public void ShouldNotConformNodeShapeWithClassTarget()
        {
            //DataGraph
            RDFGraph dataGraph = new RDFGraph().SetContext(new Uri("ex:DataGraph"));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Man"), RDFVocabulary.RDFS.SUB_CLASS_OF, new RDFResource("ex:Person")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Woman"), RDFVocabulary.RDFS.SUB_CLASS_OF, new RDFResource("ex:Person")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Alice"), RDFVocabulary.RDF.TYPE, new RDFResource("ex:Woman")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Bob"), RDFVocabulary.RDF.TYPE, new RDFResource("ex:Man")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Alice"), RDFVocabulary.FOAF.KNOWS, new RDFResource("ex:Bob")));

            //ShapesGraph
            RDFShapesGraph shapesGraph = new RDFShapesGraph(new RDFResource("ex:ShapesGraph"));
            RDFNodeShape nodeShape1 = new RDFNodeShape(new RDFResource("ex:NodeShape1"));
            nodeShape1.AddConstraint(new RDFMinLengthConstraint(8));
            RDFNodeShape nodeShape2 = new RDFNodeShape(new RDFResource("ex:NodeShape2"));
            nodeShape2.AddConstraint(new RDFMaxLengthConstraint(8));
            RDFNodeShape nodeShape = new RDFNodeShape(new RDFResource("ex:NodeShape"));
            nodeShape.AddTarget(new RDFTargetClass(new RDFResource("ex:Person")));
            nodeShape.AddConstraint(new RDFXoneConstraint().AddShape(new RDFResource("ex:NodeShape1"))
                                                           .AddShape(new RDFResource("ex:NodeShape2")));
            shapesGraph.AddShape(nodeShape);
            shapesGraph.AddShape(nodeShape1);
            shapesGraph.AddShape(nodeShape2);

            //Validate
            RDFValidationReport validationReport = shapesGraph.Validate(dataGraph);

            Assert.IsNotNull(validationReport);
            Assert.IsFalse(validationReport.Conforms);
            Assert.IsTrue(validationReport.ResultsCount == 1);
            Assert.IsTrue(validationReport.Results[0].Severity == RDFValidationEnums.RDFShapeSeverity.Violation);
            Assert.IsTrue(validationReport.Results[0].ResultMessages.Count == 1);
            Assert.IsTrue(validationReport.Results[0].ResultMessages[0].Equals(new RDFPlainLiteral($"Value does not have exactly one of the shapes in sh:xone enumeration")));
            Assert.IsTrue(validationReport.Results[0].FocusNode.Equals(new RDFResource("ex:Alice")));
            Assert.IsTrue(validationReport.Results[0].ResultValue.Equals(new RDFResource("ex:Alice")));
            Assert.IsNull(validationReport.Results[0].ResultPath);
            Assert.IsTrue(validationReport.Results[0].SourceConstraintComponent.Equals(RDFVocabulary.SHACL.XONE_CONSTRAINT_COMPONENT));
            Assert.IsTrue(validationReport.Results[0].SourceShape.Equals(new RDFResource("ex:NodeShape")));
        }

        [TestMethod]
        public void ShouldNotConformNodeShapeWithNodeTarget()
        {
            //DataGraph
            RDFGraph dataGraph = new RDFGraph().SetContext(new Uri("ex:DataGraph"));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Man"), RDFVocabulary.RDFS.SUB_CLASS_OF, new RDFResource("ex:Person")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Woman"), RDFVocabulary.RDFS.SUB_CLASS_OF, new RDFResource("ex:Person")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Alice"), RDFVocabulary.RDF.TYPE, new RDFResource("ex:Woman")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Bob"), RDFVocabulary.RDF.TYPE, new RDFResource("ex:Man")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Alice"), RDFVocabulary.FOAF.KNOWS, new RDFResource("ex:Bob")));

            //ShapesGraph
            RDFShapesGraph shapesGraph = new RDFShapesGraph(new RDFResource("ex:ShapesGraph"));
            RDFNodeShape nodeShape1 = new RDFNodeShape(new RDFResource("ex:NodeShape1"));
            nodeShape1.AddConstraint(new RDFMinLengthConstraint(8));
            RDFNodeShape nodeShape2 = new RDFNodeShape(new RDFResource("ex:NodeShape2"));
            nodeShape2.AddConstraint(new RDFMaxLengthConstraint(8));
            RDFNodeShape nodeShape3 = new RDFNodeShape(new RDFResource("ex:NodeShape3"));
            nodeShape3.AddConstraint(new RDFPatternConstraint(new Regex("^ex:")));
            RDFNodeShape nodeShape = new RDFNodeShape(new RDFResource("ex:NodeShape"));
            nodeShape.AddTarget(new RDFTargetNode(new RDFResource("ex:Alice")));
            nodeShape.AddConstraint(new RDFXoneConstraint().AddShape(new RDFResource("ex:NodeShape1"))
                                                           .AddShape(new RDFResource("ex:NodeShape2"))
                                                           .AddShape(new RDFResource("ex:NodeShape3")));
            shapesGraph.AddShape(nodeShape);
            shapesGraph.AddShape(nodeShape1);
            shapesGraph.AddShape(nodeShape2);
            shapesGraph.AddShape(nodeShape3);

            //Validate
            RDFValidationReport validationReport = shapesGraph.Validate(dataGraph);

            Assert.IsNotNull(validationReport);
            Assert.IsFalse(validationReport.Conforms);
            Assert.IsTrue(validationReport.ResultsCount == 1);
            Assert.IsTrue(validationReport.Results[0].Severity == RDFValidationEnums.RDFShapeSeverity.Violation);
            Assert.IsTrue(validationReport.Results[0].ResultMessages.Count == 1);
            Assert.IsTrue(validationReport.Results[0].ResultMessages[0].Equals(new RDFPlainLiteral($"Value does not have exactly one of the shapes in sh:xone enumeration")));
            Assert.IsTrue(validationReport.Results[0].FocusNode.Equals(new RDFResource("ex:Alice")));
            Assert.IsTrue(validationReport.Results[0].ResultValue.Equals(new RDFResource("ex:Alice")));
            Assert.IsNull(validationReport.Results[0].ResultPath);
            Assert.IsTrue(validationReport.Results[0].SourceConstraintComponent.Equals(RDFVocabulary.SHACL.XONE_CONSTRAINT_COMPONENT));
            Assert.IsTrue(validationReport.Results[0].SourceShape.Equals(new RDFResource("ex:NodeShape")));
        }

        [TestMethod]
        public void ShouldNotConformNodeShapeWithSubjectsOfTarget()
        {
            //DataGraph
            RDFGraph dataGraph = new RDFGraph().SetContext(new Uri("ex:DataGraph"));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Man"), RDFVocabulary.RDFS.SUB_CLASS_OF, new RDFResource("ex:Person")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Woman"), RDFVocabulary.RDFS.SUB_CLASS_OF, new RDFResource("ex:Person")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Alice"), RDFVocabulary.RDF.TYPE, new RDFResource("ex:Woman")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Bob"), RDFVocabulary.RDF.TYPE, new RDFResource("ex:Man")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Alice"), RDFVocabulary.FOAF.KNOWS, new RDFResource("ex:Bob")));

            //ShapesGraph
            RDFShapesGraph shapesGraph = new RDFShapesGraph(new RDFResource("ex:ShapesGraph"));
            RDFNodeShape nodeShape1 = new RDFNodeShape(new RDFResource("ex:NodeShape1"));
            nodeShape1.AddConstraint(new RDFMinLengthConstraint(12));
            RDFNodeShape nodeShape2 = new RDFNodeShape(new RDFResource("ex:NodeShape2"));
            nodeShape2.AddConstraint(new RDFMaxLengthConstraint(6));
            RDFNodeShape nodeShape3 = new RDFNodeShape(new RDFResource("ex:NodeShape3"));
            nodeShape3.AddConstraint(new RDFPatternConstraint(new Regex("^ez:")));
            RDFNodeShape nodeShape = new RDFNodeShape(new RDFResource("ex:NodeShape"));
            nodeShape.AddTarget(new RDFTargetSubjectsOf(RDFVocabulary.FOAF.KNOWS));
            nodeShape.AddConstraint(new RDFXoneConstraint().AddShape(new RDFResource("ex:NodeShape1"))
                                                           .AddShape(new RDFResource("ex:NodeShape2"))
                                                           .AddShape(new RDFResource("ex:NodeShape3")));
            shapesGraph.AddShape(nodeShape);
            shapesGraph.AddShape(nodeShape1);
            shapesGraph.AddShape(nodeShape2);
            shapesGraph.AddShape(nodeShape3);

            //Validate
            RDFValidationReport validationReport = shapesGraph.Validate(dataGraph);

            Assert.IsNotNull(validationReport);
            Assert.IsFalse(validationReport.Conforms);
            Assert.IsTrue(validationReport.ResultsCount == 1);
            Assert.IsTrue(validationReport.Results[0].Severity == RDFValidationEnums.RDFShapeSeverity.Violation);
            Assert.IsTrue(validationReport.Results[0].ResultMessages.Count == 1);
            Assert.IsTrue(validationReport.Results[0].ResultMessages[0].Equals(new RDFPlainLiteral($"Value does not have exactly one of the shapes in sh:xone enumeration")));
            Assert.IsTrue(validationReport.Results[0].FocusNode.Equals(new RDFResource("ex:Alice")));
            Assert.IsTrue(validationReport.Results[0].ResultValue.Equals(new RDFResource("ex:Alice")));
            Assert.IsNull(validationReport.Results[0].ResultPath);
            Assert.IsTrue(validationReport.Results[0].SourceConstraintComponent.Equals(RDFVocabulary.SHACL.XONE_CONSTRAINT_COMPONENT));
            Assert.IsTrue(validationReport.Results[0].SourceShape.Equals(new RDFResource("ex:NodeShape")));
        }

        [TestMethod]
        public void ShouldNotConformNodeShapeWithObjectsOfTarget()
        {
            //DataGraph
            RDFGraph dataGraph = new RDFGraph().SetContext(new Uri("ex:DataGraph"));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Man"), RDFVocabulary.RDFS.SUB_CLASS_OF, new RDFResource("ex:Person")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Woman"), RDFVocabulary.RDFS.SUB_CLASS_OF, new RDFResource("ex:Person")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Alice"), RDFVocabulary.RDF.TYPE, new RDFResource("ex:Woman")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Bob"), RDFVocabulary.RDF.TYPE, new RDFResource("ex:Man")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Alice"), RDFVocabulary.FOAF.KNOWS, new RDFResource("ex:Bob")));

            //ShapesGraph
            RDFShapesGraph shapesGraph = new RDFShapesGraph(new RDFResource("ex:ShapesGraph"));
            RDFNodeShape nodeShape1 = new RDFNodeShape(new RDFResource("ex:NodeShape1"));
            nodeShape1.AddConstraint(new RDFMinLengthConstraint(6));
            RDFNodeShape nodeShape2 = new RDFNodeShape(new RDFResource("ex:NodeShape2"));
            nodeShape2.AddConstraint(new RDFMaxLengthConstraint(8));
            RDFNodeShape nodeShape = new RDFNodeShape(new RDFResource("ex:NodeShape"));
            nodeShape.AddTarget(new RDFTargetObjectsOf(RDFVocabulary.FOAF.KNOWS));
            nodeShape.AddConstraint(new RDFXoneConstraint().AddShape(new RDFResource("ex:NodeShape1"))
                                                           .AddShape(new RDFResource("ex:NodeShape2")));
            shapesGraph.AddShape(nodeShape);
            shapesGraph.AddShape(nodeShape1);
            shapesGraph.AddShape(nodeShape2);

            //Validate
            RDFValidationReport validationReport = shapesGraph.Validate(dataGraph);

            Assert.IsNotNull(validationReport);
            Assert.IsFalse(validationReport.Conforms);
            Assert.IsTrue(validationReport.ResultsCount == 1);
            Assert.IsTrue(validationReport.Results[0].Severity == RDFValidationEnums.RDFShapeSeverity.Violation);
            Assert.IsTrue(validationReport.Results[0].ResultMessages.Count == 1);
            Assert.IsTrue(validationReport.Results[0].ResultMessages[0].Equals(new RDFPlainLiteral($"Value does not have exactly one of the shapes in sh:xone enumeration")));
            Assert.IsTrue(validationReport.Results[0].FocusNode.Equals(new RDFResource("ex:Bob")));
            Assert.IsTrue(validationReport.Results[0].ResultValue.Equals(new RDFResource("ex:Bob")));
            Assert.IsNull(validationReport.Results[0].ResultPath);
            Assert.IsTrue(validationReport.Results[0].SourceConstraintComponent.Equals(RDFVocabulary.SHACL.XONE_CONSTRAINT_COMPONENT));
            Assert.IsTrue(validationReport.Results[0].SourceShape.Equals(new RDFResource("ex:NodeShape")));
        }

        //PS-CONFORMS:FALSE

        [TestMethod]
        public void ShouldNotConformPropertyShapeWithClassTarget()
        {
            //DataGraph
            RDFGraph dataGraph = new RDFGraph().SetContext(new Uri("ex:DataGraph"));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Man"), RDFVocabulary.RDFS.SUB_CLASS_OF, new RDFResource("ex:Person")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Woman"), RDFVocabulary.RDFS.SUB_CLASS_OF, new RDFResource("ex:Person")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Alice"), RDFVocabulary.RDF.TYPE, new RDFResource("ex:Woman")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Bob"), RDFVocabulary.RDF.TYPE, new RDFResource("ex:Man")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Alice"), RDFVocabulary.FOAF.KNOWS, new RDFResource("ex:Bob")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Bob"), RDFVocabulary.FOAF.KNOWS, new RDFResource("ex:Alice")));

            //ShapesGraph
            RDFShapesGraph shapesGraph = new RDFShapesGraph(new RDFResource("ex:ShapesGraph"));
            RDFNodeShape nodeShape1 = new RDFNodeShape(new RDFResource("ex:NodeShape1"));
            nodeShape1.AddConstraint(new RDFMinLengthConstraint(6));
            RDFNodeShape nodeShape2 = new RDFNodeShape(new RDFResource("ex:NodeShape2"));
            nodeShape2.AddConstraint(new RDFMaxLengthConstraint(6));
            RDFPropertyShape propertyShape = new RDFPropertyShape(new RDFResource("ex:PropertyShape"), RDFVocabulary.FOAF.KNOWS);
            propertyShape.AddTarget(new RDFTargetClass(new RDFResource("ex:Person")));
            propertyShape.AddConstraint(new RDFXoneConstraint().AddShape(new RDFResource("ex:NodeShape1"))
                                                               .AddShape(new RDFResource("ex:NodeShape2")));
            shapesGraph.AddShape(propertyShape);
            shapesGraph.AddShape(nodeShape1);
            shapesGraph.AddShape(nodeShape2);

            //Validate
            RDFValidationReport validationReport = shapesGraph.Validate(dataGraph);

            Assert.IsNotNull(validationReport);
            Assert.IsFalse(validationReport.Conforms);
            Assert.IsTrue(validationReport.ResultsCount == 1);
            Assert.IsTrue(validationReport.Results[0].Severity == RDFValidationEnums.RDFShapeSeverity.Violation);
            Assert.IsTrue(validationReport.Results[0].ResultMessages.Count == 1);
            Assert.IsTrue(validationReport.Results[0].ResultMessages[0].Equals(new RDFPlainLiteral($"Value does not have exactly one of the shapes in sh:xone enumeration")));
            Assert.IsTrue(validationReport.Results[0].FocusNode.Equals(new RDFResource("ex:Alice")));
            Assert.IsTrue(validationReport.Results[0].ResultValue.Equals(new RDFResource("ex:Bob")));
            Assert.IsTrue(validationReport.Results[0].ResultPath.Equals(RDFVocabulary.FOAF.KNOWS));
            Assert.IsTrue(validationReport.Results[0].SourceConstraintComponent.Equals(RDFVocabulary.SHACL.XONE_CONSTRAINT_COMPONENT));
            Assert.IsTrue(validationReport.Results[0].SourceShape.Equals(new RDFResource("ex:PropertyShape")));
        }

        [TestMethod]
        public void ShouldNotConformPropertyShapeWithNodeTarget()
        {
            //DataGraph
            RDFGraph dataGraph = new RDFGraph().SetContext(new Uri("ex:DataGraph"));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Man"), RDFVocabulary.RDFS.SUB_CLASS_OF, new RDFResource("ex:Person")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Woman"), RDFVocabulary.RDFS.SUB_CLASS_OF, new RDFResource("ex:Person")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Alice"), RDFVocabulary.RDF.TYPE, new RDFResource("ex:Woman")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Bob"), RDFVocabulary.RDF.TYPE, new RDFResource("ex:Man")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Alice"), RDFVocabulary.FOAF.KNOWS, new RDFResource("ex:Bob")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Bob"), RDFVocabulary.FOAF.KNOWS, new RDFResource("ex:Alice")));

            //ShapesGraph
            RDFShapesGraph shapesGraph = new RDFShapesGraph(new RDFResource("ex:ShapesGraph"));
            RDFNodeShape nodeShape1 = new RDFNodeShape(new RDFResource("ex:NodeShape1"));
            nodeShape1.AddConstraint(new RDFMinLengthConstraint(6));
            RDFNodeShape nodeShape2 = new RDFNodeShape(new RDFResource("ex:NodeShape2"));
            nodeShape2.AddConstraint(new RDFMaxLengthConstraint(6));
            RDFPropertyShape propertyShape = new RDFPropertyShape(new RDFResource("ex:PropertyShape"), RDFVocabulary.FOAF.KNOWS);
            propertyShape.AddTarget(new RDFTargetNode(new RDFResource("ex:Alice")));
            propertyShape.AddConstraint(new RDFXoneConstraint().AddShape(new RDFResource("ex:NodeShape1"))
                                                               .AddShape(new RDFResource("ex:NodeShape2")));
            shapesGraph.AddShape(propertyShape);
            shapesGraph.AddShape(nodeShape1);
            shapesGraph.AddShape(nodeShape2);

            //Validate
            RDFValidationReport validationReport = shapesGraph.Validate(dataGraph);

            Assert.IsNotNull(validationReport);
            Assert.IsFalse(validationReport.Conforms);
            Assert.IsTrue(validationReport.ResultsCount == 1);
            Assert.IsTrue(validationReport.Results[0].Severity == RDFValidationEnums.RDFShapeSeverity.Violation);
            Assert.IsTrue(validationReport.Results[0].ResultMessages.Count == 1);
            Assert.IsTrue(validationReport.Results[0].ResultMessages[0].Equals(new RDFPlainLiteral($"Value does not have exactly one of the shapes in sh:xone enumeration")));
            Assert.IsTrue(validationReport.Results[0].FocusNode.Equals(new RDFResource("ex:Alice")));
            Assert.IsTrue(validationReport.Results[0].ResultValue.Equals(new RDFResource("ex:Bob")));
            Assert.IsTrue(validationReport.Results[0].ResultPath.Equals(RDFVocabulary.FOAF.KNOWS));
            Assert.IsTrue(validationReport.Results[0].SourceConstraintComponent.Equals(RDFVocabulary.SHACL.XONE_CONSTRAINT_COMPONENT));
            Assert.IsTrue(validationReport.Results[0].SourceShape.Equals(new RDFResource("ex:PropertyShape")));
        }

        [TestMethod]
        public void ShouldNotConformPropertyShapeWithSubjectsOfTarget()
        {
            //DataGraph
            RDFGraph dataGraph = new RDFGraph().SetContext(new Uri("ex:DataGraph"));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Man"), RDFVocabulary.RDFS.SUB_CLASS_OF, new RDFResource("ex:Person")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Woman"), RDFVocabulary.RDFS.SUB_CLASS_OF, new RDFResource("ex:Person")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Alice"), RDFVocabulary.RDF.TYPE, new RDFResource("ex:Woman")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Bob"), RDFVocabulary.RDF.TYPE, new RDFResource("ex:Man")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Alice"), RDFVocabulary.FOAF.KNOWS, new RDFResource("ex:Bob")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Bob"), RDFVocabulary.FOAF.KNOWS, new RDFResource("ex:Alice")));

            //ShapesGraph
            RDFShapesGraph shapesGraph = new RDFShapesGraph(new RDFResource("ex:ShapesGraph"));
            RDFNodeShape nodeShape1 = new RDFNodeShape(new RDFResource("ex:NodeShape1"));
            nodeShape1.AddConstraint(new RDFMinLengthConstraint(6));
            RDFNodeShape nodeShape2 = new RDFNodeShape(new RDFResource("ex:NodeShape2"));
            nodeShape2.AddConstraint(new RDFMaxLengthConstraint(6));
            RDFPropertyShape propertyShape = new RDFPropertyShape(new RDFResource("ex:PropertyShape"), RDFVocabulary.FOAF.KNOWS);
            propertyShape.AddTarget(new RDFTargetSubjectsOf(RDFVocabulary.FOAF.KNOWS));
            propertyShape.AddConstraint(new RDFXoneConstraint().AddShape(new RDFResource("ex:NodeShape1"))
                                                               .AddShape(new RDFResource("ex:NodeShape2")));
            shapesGraph.AddShape(propertyShape);
            shapesGraph.AddShape(nodeShape1);
            shapesGraph.AddShape(nodeShape2);

            //Validate
            RDFValidationReport validationReport = shapesGraph.Validate(dataGraph);

            Assert.IsNotNull(validationReport);
            Assert.IsFalse(validationReport.Conforms);
            Assert.IsTrue(validationReport.ResultsCount == 1);
            Assert.IsTrue(validationReport.Results[0].Severity == RDFValidationEnums.RDFShapeSeverity.Violation);
            Assert.IsTrue(validationReport.Results[0].ResultMessages.Count == 1);
            Assert.IsTrue(validationReport.Results[0].ResultMessages[0].Equals(new RDFPlainLiteral($"Value does not have exactly one of the shapes in sh:xone enumeration")));
            Assert.IsTrue(validationReport.Results[0].FocusNode.Equals(new RDFResource("ex:Alice")));
            Assert.IsTrue(validationReport.Results[0].ResultValue.Equals(new RDFResource("ex:Bob")));
            Assert.IsTrue(validationReport.Results[0].ResultPath.Equals(RDFVocabulary.FOAF.KNOWS));
            Assert.IsTrue(validationReport.Results[0].SourceConstraintComponent.Equals(RDFVocabulary.SHACL.XONE_CONSTRAINT_COMPONENT));
            Assert.IsTrue(validationReport.Results[0].SourceShape.Equals(new RDFResource("ex:PropertyShape")));
        }

        [TestMethod]
        public void ShouldNotConformPropertyShapeWithObjectsOfTarget()
        {
            //DataGraph
            RDFGraph dataGraph = new RDFGraph().SetContext(new Uri("ex:DataGraph"));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Man"), RDFVocabulary.RDFS.SUB_CLASS_OF, new RDFResource("ex:Person")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Woman"), RDFVocabulary.RDFS.SUB_CLASS_OF, new RDFResource("ex:Person")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Alice"), RDFVocabulary.RDF.TYPE, new RDFResource("ex:Woman")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Bob"), RDFVocabulary.RDF.TYPE, new RDFResource("ex:Man")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Alice"), RDFVocabulary.FOAF.KNOWS, new RDFResource("ex:Bob")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Bob"), RDFVocabulary.FOAF.KNOWS, new RDFResource("ex:Alice")));

            //ShapesGraph
            RDFShapesGraph shapesGraph = new RDFShapesGraph(new RDFResource("ex:ShapesGraph"));
            RDFNodeShape nodeShape1 = new RDFNodeShape(new RDFResource("ex:NodeShape1"));
            nodeShape1.AddConstraint(new RDFMinLengthConstraint(6));
            RDFNodeShape nodeShape2 = new RDFNodeShape(new RDFResource("ex:NodeShape2"));
            nodeShape2.AddConstraint(new RDFMaxLengthConstraint(6));
            RDFPropertyShape propertyShape = new RDFPropertyShape(new RDFResource("ex:PropertyShape"), RDFVocabulary.FOAF.KNOWS);
            propertyShape.AddTarget(new RDFTargetObjectsOf(RDFVocabulary.FOAF.KNOWS));
            propertyShape.AddConstraint(new RDFXoneConstraint().AddShape(new RDFResource("ex:NodeShape1"))
                                                               .AddShape(new RDFResource("ex:NodeShape2")));
            shapesGraph.AddShape(propertyShape);
            shapesGraph.AddShape(nodeShape1);
            shapesGraph.AddShape(nodeShape2);

            //Validate
            RDFValidationReport validationReport = shapesGraph.Validate(dataGraph);

            Assert.IsNotNull(validationReport);
            Assert.IsFalse(validationReport.Conforms);
            Assert.IsTrue(validationReport.ResultsCount == 1);
            Assert.IsTrue(validationReport.Results[0].Severity == RDFValidationEnums.RDFShapeSeverity.Violation);
            Assert.IsTrue(validationReport.Results[0].ResultMessages.Count == 1);
            Assert.IsTrue(validationReport.Results[0].ResultMessages[0].Equals(new RDFPlainLiteral($"Value does not have exactly one of the shapes in sh:xone enumeration")));
            Assert.IsTrue(validationReport.Results[0].FocusNode.Equals(new RDFResource("ex:Alice")));
            Assert.IsTrue(validationReport.Results[0].ResultValue.Equals(new RDFResource("ex:Bob")));
            Assert.IsTrue(validationReport.Results[0].ResultPath.Equals(RDFVocabulary.FOAF.KNOWS));
            Assert.IsTrue(validationReport.Results[0].SourceConstraintComponent.Equals(RDFVocabulary.SHACL.XONE_CONSTRAINT_COMPONENT));
            Assert.IsTrue(validationReport.Results[0].SourceShape.Equals(new RDFResource("ex:PropertyShape")));
        }

        //MIXED-CONFORMS:TRUE

        [TestMethod]
        public void ShouldConformNodeShapeWithPropertyShapes()
        {
            //DataGraph
            RDFGraph dataGraph = new RDFGraph().SetContext(new Uri("ex:DataGraph"));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Alice"), RDFVocabulary.FOAF.KNOWS, new RDFResource("ex:Bob")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Bob"), RDFVocabulary.FOAF.KNOWS, new RDFResource("ex:Alice")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Alice"), RDFVocabulary.FOAF.AGE, new RDFTypedLiteral("22", RDFModelEnums.RDFDatatypes.XSD_INTEGER)));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:Bob"), RDFVocabulary.FOAF.AGE, new RDFTypedLiteral("26", RDFModelEnums.RDFDatatypes.XSD_INTEGER)));

            //ShapesGraph
            RDFShapesGraph shapesGraph = new RDFShapesGraph(new RDFResource("ex:ShapesGraph"));
            RDFNodeShape nodeShape = new RDFNodeShape(new RDFResource("ex:NodeShape"));
            nodeShape.AddTarget(new RDFTargetNode(new RDFResource("ex:Alice")));
            nodeShape.AddTarget(new RDFTargetNode(new RDFResource("ex:Bob")));
            nodeShape.AddConstraint(new RDFXoneConstraint().AddShape(new RDFResource("ex:PropShape1"))
                                                           .AddShape(new RDFResource("ex:PropShape2")));
            RDFPropertyShape propShape1 = new RDFPropertyShape(new RDFResource("ex:PropShape1"), RDFVocabulary.FOAF.KNOWS);
            propShape1.AddConstraint(new RDFPatternConstraint(new Regex(@"^ex:", RegexOptions.IgnoreCase)));
            RDFPropertyShape propShape2 = new RDFPropertyShape(new RDFResource("ex:PropShape2"), RDFVocabulary.FOAF.AGE);
            propShape2.AddConstraint(new RDFMaxExclusiveConstraint(new RDFTypedLiteral("22.0", RDFModelEnums.RDFDatatypes.XSD_DOUBLE)));
            shapesGraph.AddShape(nodeShape);
            shapesGraph.AddShape(propShape1);
            shapesGraph.AddShape(propShape2);

            //Validate
            RDFValidationReport validationReport = shapesGraph.Validate(dataGraph);

            Assert.IsNotNull(validationReport);
            Assert.IsTrue(validationReport.Conforms);
        }

        //MIXED-CONFORMS:FALSE

        [TestMethod]
        public void ShouldNotConformNodeShapeWithPropertyShapes()
        {
            //DataGraph
            RDFGraph dataGraph = new RDFGraph().SetContext(new Uri("ex:DataGraph"));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:A"), new RDFResource("ex:property"), new RDFPlainLiteral("One")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:B"), new RDFResource("ex:property"), new RDFPlainLiteral("One")));
            dataGraph.AddTriple(new RDFTriple(new RDFResource("ex:B"), new RDFResource("ex:property"), new RDFPlainLiteral("Two")));

            //ShapesGraph
            RDFShapesGraph shapesGraph = new RDFShapesGraph(new RDFResource("ex:ShapesGraph"));
            RDFNodeShape nodeShape1 = new RDFNodeShape(new RDFResource("ex:SuperShape"));
            nodeShape1.AddConstraint(new RDFPropertyConstraint(new RDFResource("ex:PropShape1")));
            RDFPropertyShape propShape1 = new RDFPropertyShape(new RDFResource("ex:PropShape1"), new RDFResource("ex:property"));
            propShape1.AddConstraint(new RDFMinCountConstraint(2));
            RDFPropertyShape propShape2 = new RDFPropertyShape(new RDFResource("ex:PropShape2"), new RDFResource("ex:property"));
            propShape2.AddConstraint(new RDFMinLengthConstraint(3));
            RDFNodeShape nodeShape2 = new RDFNodeShape(new RDFResource("ex:ExampleXoneShape"));
            nodeShape2.AddTarget(new RDFTargetNode(new RDFResource("ex:A")));
            nodeShape2.AddTarget(new RDFTargetNode(new RDFResource("ex:B")));
            nodeShape2.AddConstraint(new RDFXoneConstraint().AddShape(nodeShape1)
                                                            .AddShape(propShape2));
            shapesGraph.AddShape(nodeShape1);
            shapesGraph.AddShape(nodeShape2);
            shapesGraph.AddShape(propShape1);
            shapesGraph.AddShape(propShape2);

            //Validate
            RDFValidationReport validationReport = shapesGraph.Validate(dataGraph);

            Assert.IsNotNull(validationReport);
            Assert.IsFalse(validationReport.Conforms);
            Assert.IsTrue(validationReport.ResultsCount == 1);
            Assert.IsTrue(validationReport.Results[0].Severity == RDFValidationEnums.RDFShapeSeverity.Violation);
            Assert.IsTrue(validationReport.Results[0].ResultMessages.Count == 1);
            Assert.IsTrue(validationReport.Results[0].ResultMessages[0].Equals(new RDFPlainLiteral($"Value does not have exactly one of the shapes in sh:xone enumeration")));
            Assert.IsTrue(validationReport.Results[0].FocusNode.Equals(new RDFResource("ex:B")));
            Assert.IsTrue(validationReport.Results[0].ResultValue.Equals(new RDFResource("ex:B")));
            Assert.IsNull(validationReport.Results[0].ResultPath);
            Assert.IsTrue(validationReport.Results[0].SourceConstraintComponent.Equals(RDFVocabulary.SHACL.XONE_CONSTRAINT_COMPONENT));
            Assert.IsTrue(validationReport.Results[0].SourceShape.Equals(new RDFResource("ex:ExampleXoneShape")));
        }
        #endregion
    }
}
