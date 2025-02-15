// <copyright file="MigraDocExtensions.cs" company="LinoPro GmbH">
// Copyright (c) LinoPro GmbH. All rights reserved.
// </copyright>

using MigraDoc.DocumentObjectModel;
using PdfSharp.Fonts;

internal static class MigraDocExtensions
{
    internal const string DefaultFontFamilyName = "LiberationSans";

    static MigraDocExtensions() => GlobalFontSettings.FontResolver = new CustomFontResolver();

    internal static Paragraph AddParagraphText(this Section section, PdfFontStyle style, string text)
    {
        text ??= string.Empty;
        var paragraph = section.AddParagraph();
        paragraph.Style = style.ToString();
        paragraph.AddText(text);
        return paragraph;
    }

    internal static void DefineDefaults(this Document document, PdfData pdfData)
    {
        document.Info.Title = pdfData.DocumentTitle;
        document.Info.Subject = pdfData.DocumentDescription;
        document.Info.Author = pdfData.CreatedBy;

        // Get the predefined style Normal.
        var style = document.Styles[PdfFontStyle.Normal.ToString()];
        style.Font.Name = DefaultFontFamilyName;
        style.Font.Size = 10;
        style.Font.Bold = false;
    }

    internal sealed class CustomFontResolver : IFontResolver
    {
        private const string FontFaceNameBold = "LiberationSans-Bold";
        private const string FontFaceNameBoldItalic = "LiberationSans-BoldItalic";
        private const string FontFaceNameItalic = "LiberationSans-Italic";
        private const string FontFaceNameRegular = "LiberationSans-Regular";

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            if (familyName != MigraDocExtensions.DefaultFontFamilyName)
            {
                //throw new ArgumentException($"invalid font family - {familyName}!");
            }

            return isBold switch
            {
                true when !isItalic => new FontResolverInfo(FontFaceNameBold),
                true when isItalic => new FontResolverInfo(FontFaceNameBoldItalic),
                false when isItalic => new FontResolverInfo(FontFaceNameItalic),
                false when !isItalic => new FontResolverInfo(FontFaceNameRegular),
                _ => throw new ArgumentException($"Font-Resolve failed: familyName: {familyName}, isBold: {isBold}, isItalic: {isItalic}")
            };
        }

        public byte[] GetFont(string faceName) => File.ReadAllBytes($"{AppContext.BaseDirectory}/Resources/fonts/{faceName}.ttf");
    }
}
