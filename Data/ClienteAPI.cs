using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace ProyectoWebCursoLenguajes.Data
{
    public class ClienteAPI
    {
        public HttpClient Inicial()
        {

            var client = new HttpClient();

            client.BaseAddress = new Uri("http://www.apiclientes.somee.com");

            return client;
        }

    }
}
