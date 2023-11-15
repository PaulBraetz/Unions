namespace RhoMicro.Unions.Generator.Models;
using RhoMicro.Unions.Generator;

using System;
using System.Collections.Generic;
using System.Linq;

readonly struct TagInflectionValueModel : IEquatable<TagInflectionValueModel>
{
    private TagInflectionValueModel(String sourceText) => SourceText = sourceText;
    public readonly String SourceText;

    public static TagInflectionValueModel Create(AttributesModel model)
    {
        var sourceText = $"(Tag){model.ReferenceTypeAttributes.Count}";

        var result = new TagInflectionValueModel(sourceText);

        return result;
    }

    public override Boolean Equals(Object obj) => obj is TagInflectionValueModel model && Equals(model);
    public Boolean Equals(TagInflectionValueModel other) => SourceText == other.SourceText;
    public override Int32 GetHashCode() => -1893336041 + EqualityComparer<String>.Default.GetHashCode(SourceText);

    public static Boolean operator ==(TagInflectionValueModel left, TagInflectionValueModel right) => left.Equals(right);
    public static Boolean operator !=(TagInflectionValueModel left, TagInflectionValueModel right) => !(left == right);
}
