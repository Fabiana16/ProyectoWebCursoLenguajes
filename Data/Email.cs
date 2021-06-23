using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using ProyectoWebCursoLenguajes.Models;

namespace ProyectoWebCursoLenguajes.Data
{
    public class Email
    {
		public void enviarFactura(Cliente cliente)
		{
			//Creación de la variable documento
			var doc = new Document();
			MemoryStream memoryStream = new MemoryStream();
			PdfWriter writer = PdfWriter.GetInstance(doc, memoryStream);

			//Abrir el documento para su escritura 
			doc.Open();

			//Tipos de fuentes del documento.
			Font font9 = FontFactory.GetFont(FontFactory.TIMES, 9);


			//Parrafos del documento
			doc.Add(new Paragraph(15, "A continuación detallamos " +
			"los datos registrados para la solicitud de su credito", font9));
			doc.Add(new Paragraph("First Paragraph"));
			doc.Add(new Paragraph("Second Paragraph"));


			writer.CloseStream = false;
			//Cierra el documento de escritura
			doc.Close();
			memoryStream.Position = 0;

			MailMessage mm = new MailMessage("distribucionesdelpacificocr@gmail.com", cliente.email)
			{
				Subject = "Detalles de Factura de Compra",
				IsBodyHtml = true,
				Body = "body"
			};

			mm.Attachments.Add(new Attachment(memoryStream, "FacturaCompra.pdf"));
			SmtpClient smtp = new SmtpClient
			{
				Host = "smtp.gmail.com",
				Port = 587,
				EnableSsl = true,
				Credentials = new NetworkCredential("distribucionesdelpacificocr@gmail.com", "Ucr2021*")

			};

			smtp.Send(mm);
		}
	}
}
