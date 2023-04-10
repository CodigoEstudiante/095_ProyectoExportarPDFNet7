using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Previewer;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _host;

        public HomeController(IWebHostEnvironment host)
        {
            _host = host;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }


        public IActionResult DescargarPDF()
        {

            var data = Document.Create(document =>
             {
                 document.Page(page =>
                 {
                     page.Margin(30);

                     page.Header().ShowOnce().Row(row =>
                     {
                         var rutaImagen = Path.Combine(_host.WebRootPath, "images/VisualStudio.png");
                         byte[] imageData = System.IO.File.ReadAllBytes(rutaImagen);

                         //row.ConstantItem(140).Height(60).Placeholder();
                         row.ConstantItem(150).Image(imageData);


                         row.RelativeItem().Column(col =>
                         {
                             col.Item().AlignCenter().Text("Codigo Estudiante SAC").Bold().FontSize(14);
                             col.Item().AlignCenter().Text("Jr. Las mercedes N378 - Lima").FontSize(9);
                             col.Item().AlignCenter().Text("987 987 123 / 02 213232").FontSize(9);
                             col.Item().AlignCenter().Text("codigo@example.com").FontSize(9);

                         });

                         row.RelativeItem().Column(col =>
                         {
                             col.Item().Border(1).BorderColor("#257272")
                             .AlignCenter().Text("RUC 21312312312");

                             col.Item().Background("#257272").Border(1)
                             .BorderColor("#257272").AlignCenter()
                             .Text("Boleta de venta").FontColor("#fff");

                             col.Item().Border(1).BorderColor("#257272").
                             AlignCenter().Text("B0001 - 234");


                         });
                     });

                     page.Content().PaddingVertical(10).Column(col1 =>
                     {
                         col1.Item().Column(col2 =>
                         {
                             col2.Item().Text("Datos del cliente").Underline().Bold();

                             col2.Item().Text(txt =>
                             {
                                 txt.Span("Nombre: ").SemiBold().FontSize(10);
                                 txt.Span("Mario mendoza").FontSize(10);
                             });

                             col2.Item().Text(txt =>
                             {
                                 txt.Span("DNI: ").SemiBold().FontSize(10);
                                 txt.Span("978978979").FontSize(10);
                             });

                             col2.Item().Text(txt =>
                             {
                                 txt.Span("Direccion: ").SemiBold().FontSize(10);
                                 txt.Span("av. miraflores 123").FontSize(10);
                             });
                         });

                         col1.Item().LineHorizontal(0.5f);

                         col1.Item().Table(tabla =>
                         {
                             tabla.ColumnsDefinition(columns =>
                             {
                                 columns.RelativeColumn(3);
                                 columns.RelativeColumn();
                                 columns.RelativeColumn();
                                 columns.RelativeColumn();

                             });

                             tabla.Header(header =>
                             {
                                 header.Cell().Background("#257272")
                                 .Padding(2).Text("Producto").FontColor("#fff");

                                 header.Cell().Background("#257272")
                                .Padding(2).Text("Precio Unit").FontColor("#fff");

                                 header.Cell().Background("#257272")
                                .Padding(2).Text("Cantidad").FontColor("#fff");

                                 header.Cell().Background("#257272")
                                .Padding(2).Text("Total").FontColor("#fff");
                             });

                             foreach (var item in Enumerable.Range(1, 45))
                             {
                                 var cantidad = Placeholders.Random.Next(1, 10);
                                 var precio = Placeholders.Random.Next(5, 15);
                                 var total = cantidad * precio;

                                 tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                                 .Padding(2).Text(Placeholders.Label()).FontSize(10);

                                 tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                          .Padding(2).Text(cantidad.ToString()).FontSize(10);

                                 tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                          .Padding(2).Text($"S/. {precio}").FontSize(10);

                                 tabla.Cell().BorderBottom(0.5f).BorderColor("#D9D9D9")
                          .Padding(2).AlignRight().Text($"S/. {total}").FontSize(10);
                             }

                         });

                         col1.Item().AlignRight().Text("Total: 1500").FontSize(12);

                         if (1 == 1)
                             col1.Item().Background(Colors.Grey.Lighten3).Padding(10)
                             .Column(column =>
                             {
                                 column.Item().Text("Comentarios").FontSize(14);
                                 column.Item().Text(Placeholders.LoremIpsum());
                                 column.Spacing(5);
                             });

                         col1.Spacing(10);
                     });


                     page.Footer()
                     .AlignRight()
                     .Text(txt =>
                     {
                         txt.Span("Pagina ").FontSize(10);
                         txt.CurrentPageNumber().FontSize(10);
                         txt.Span(" de ").FontSize(10);
                         txt.TotalPages().FontSize(10);
                     });
                 });
             }).GeneratePdf();

            Stream stream = new MemoryStream(data);
            return File(stream, "application/pdf", "detalleventa.pdf");

        }


    }
}