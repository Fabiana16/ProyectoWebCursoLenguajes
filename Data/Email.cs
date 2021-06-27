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

			//Abrir el documento para su escritura.. 
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

        public void enviar(Usuario usuario, string urlFirma)
        {
            try
            {
                //se crea una instancia del obj mail
                MailMessage email = new MailMessage();

                email.To.Add(new MailAddress("distribucionesdelpacificocr@gmail.com"));

                //se agrega el email del usuario
                email.To.Add(new MailAddress(usuario.email));

                //se agrega el emisor
                email.From = new MailAddress("distribucionesdelpacificocr@gmail.com");

                //asunto mail
                email.Subject = "Datos de registro en platafor,a web d eveterinaria CR";

                //se construye la vista html del body del email
                string html = "Bienvenidos a Veterinaria CR gracias por formar parte de nuestra plataforma web";
                html += "<br>A continuacion detallamos los datos registrados en nuestra plataforma web:";
                html += "<br><b>Login:</br>" + usuario.login;
                html += "<br><b>Nombre completo:</br>" + usuario.nombre;
                html += "<br><b>Email:</br>" + usuario.email;
                html += "<br><b>Contraseña:</br>" + usuario.password;
                html += "<br><b>No responda a este correo porque fue generado de forma automatica";
                html += "por la plataforma web Veterinaria CR </br>";

                //se indica en contenido es el html
                email.IsBodyHtml = true;

                //se indica la prioridad del email
                email.Priority = MailPriority.Normal;

                //aqui se crea el adjunto de la fotografia utilizada como firma
                Attachment attachment = new Attachment(urlFirma);

                //se crea la etiqueta img para agregar la imagen como firma al body del email
                html += "<br><br><img src: 'cd:imagen' />";

                //se crea la instancia para la vista html del body del email

                AlternateView view = AlternateView.CreateAlternateViewFromString(html, Encoding.UTF8, MediaTypeNames.Text.Html);
                //se crea la instancia del obj incrustado como una imagen de archivo adjunto

                LinkedResource img = new LinkedResource(urlFirma, MediaTypeNames.Image.Jpeg);

                //se indica el id para la imagen
                img.ContentId = "imagen";

                //se adjunta la imagen
                view.LinkedResources.Add(img);

                //se agrega la vista al email
                email.AlternateViews.Add(view);

                //se instancia un obj SmtpClient
                SmtpClient smtp = new SmtpClient();

                //se indica el servidor de correo a implementar
                smtp.Host = "smtp.gmail.com";

                //puerto de comunicacion
                smtp.Port = 587;

                //se indica si utiliza seguridad tipo sll
                smtp.EnableSsl = true;

                //se indica si tenemos credenciales por default
                //es este caso no
                smtp.UseDefaultCredentials = false;

                //aqui indicamos las credenciales del servidor de correos
                smtp.Credentials = new NetworkCredential("distribucionesdelpacificocr@gmail.com", "Ucr2021*");

                //se envia el email
                smtp.Send(email);

                //se liberan los recursos
                email.Dispose();
                smtp.Dispose();

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
