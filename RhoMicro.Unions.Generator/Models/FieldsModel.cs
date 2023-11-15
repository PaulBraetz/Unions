﻿namespace RhoMicro.Unions.Generator.Models;

using Microsoft.CodeAnalysis;

using RhoMicro.Unions.Generator;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

readonly struct FieldsModel
{
    public readonly String SourceText;
    private FieldsModel(String sourceText) => SourceText = sourceText;

    public static void Integrate(ModelIntegrationContext<FieldsModel> context) =>
        context.Source.SetFields(context.Model);

    public static FieldsModel Create(ModelFactoryInvocationContext context)
    {
        var attributes = context.Parameters.Attributes;

        var sourceTextBuilder = new StringBuilder();

        if(attributes.AllUnionTypeAttributes.Count > 1)
            _ = sourceTextBuilder.Append("private readonly Tag __tag;");

        if(attributes.ReferenceTypeAttributes.Count > 0)
            _ = sourceTextBuilder.Append("private readonly Object __referenceTypeContainer;");

        if(attributes.ValueTypeAttributes.Count > 0)
            _ = sourceTextBuilder.Append("private readonly ValueTypeContainer __valueTypeContainer;");

        var sourceText = sourceTextBuilder.ToString();
        var result = new FieldsModel(sourceText);

        return result;
    }
}