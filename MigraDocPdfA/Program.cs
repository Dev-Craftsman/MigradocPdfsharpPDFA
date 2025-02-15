using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp.Pdf;

public class Program
{
    static void Main(string[] args)
    {
        // Create a MigraDoc document
        Document document = new Document();        

        document.DefineDefaults(new PdfData("test", "test", "test", "test"));
        Section section = document.AddSection();
        Paragraph paragraph = section.AddParagraphText(PdfFontStyle.Normal, "Hello, PDF/A World!");
 
        // Set PDF/A conformance
        PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer();
        pdfRenderer.Document = document;
        pdfRenderer.RenderDocument();

        // Get the PDF document
        PdfDocument pdfDocument = pdfRenderer.PdfDocument;
        pdfDocument.SetPdfA();

        // Set PDF/A properties (example)
        pdfDocument.Info.Title = "My PDF/A Document";
        // ... set other metadata

        // Save the PDF/A file
        pdfDocument.Save("MyPdfA.pdf");
    }
}