using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProyectoWebCursoLenguajes.Models;
using ProyectoWebCursoLenguajes.Data;
//libreria para utilizar la autenticacion por formulario
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace ProyectoWebCursoLenguajes.Controllers
{
    public class LoginController : Controller
    {
        private ProyectoWebCursoLenguajesContext context;
        public IActionResult Index()
        {
            return View();
        }

        public LoginController(ProyectoWebCursoLenguajesContext cnt)
        {
            this.context = cnt;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index([Bind] Usuario user)
        {
            try
            {
                var temp = this.ValidarUsuario(user);
                //se pregunta si el suario se valido correctamente


                if (temp != null)
                {
                    var userClaims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, temp.login),
                        new Claim(ClaimTypes.Email, temp.email)

                    };

                    var grandmaIdentity = new ClaimsIdentity(userClaims, "User Identity");

                    var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });

                    HttpContext.SignInAsync(userPrincipal);

                    return RedirectToAction("Index", "Home");

                }

                TempData["mensaje"] = "Error usuario o password incorrecto";

                return View(user);
            }
            catch (Exception)
            {

                throw;
            }
        }
    

        [HttpGet]
        public IActionResult CrearCuenta()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CrearCuenta([Bind] Usuario usuario)
        {
            try
            {
                var user = this.context.Usuario.FirstOrDefault(u => u.login == usuario.login);
                if (user == null)
                {
                    if (ModelState.IsValid)
                    {

                        usuario.password = this.generarClave();


                        this.context.Usuario.Add(usuario);
                        this.context.SaveChanges();

                        //se el obj email
                        Email email = new Email();

                        //se utiliza el met enviar con los datos del usuario y la firma
                        email.enviar(usuario, @"wwwroot/css/img/firmaDistribuidora.png");

                        TempData["mensaje"] = "Usuario creado correctamente";
                        return RedirectToAction("CrearCuenta");
                    }
                    else
                    {
                        TempData["mensaje"] = "No se logro crear el usuario";
                        return View(usuario);
                    }
                }
                return View(usuario);
            }//try
            catch (Exception ex)
            {
                TempData["mensaje"] = "Error " + ex.Message;
                TempData["mensaje"] = "El usuario ya existe";
                return View(usuario);
            }

        }


        private string generarClave()
        {
            try
            {
                Random random = new Random();
                string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

                return new string(Enumerable.Repeat(chars, 12).Select(s => s[random.Next(s.Length)]).ToArray());

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //programacion 31 de mayo hora 14:10
        private Usuario ValidarUsuario(Usuario temp)
        {
            Usuario autorizado = null;

            //aqui se valida el login
            var user = this.context.Usuario.FirstOrDefault(u => u.login == temp.login);


            //se pregunta si existe el usuario
            if (user != null)
            {
                if (user.password.Equals(temp.password))
                {

                    autorizado = user;

                }

            }
            return autorizado;
        }

       

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public IActionResult CambiarContrasena()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CambiarContrasena([Bind] Usuario user)
        {

            try
            {
                var login = this.context.Usuario.FirstOrDefault(u => u.login == user.login);

                if (login != null)
                {
                    if (!login.password.Equals(user.password))
                    {
                        login.password = user.password;
                    }

                    this.context.Usuario.Update(login);
                    this.context.SaveChanges();
                    TempData["mensaje"] = "contraseña cambiada correctamente";
                    return RedirectToAction("CambiarContrasena");
                }
                else
                {
                    TempData["mensaje"] = "Error al cambiar la contrasenia, no se encontró ese usuario";
                    return View(user);
                }
            }
            catch (Exception ex)
            {
                TempData["mensaje"] = "Error al cambiar la contrasenia" + ex.Message;
                return View(user);
            }

        }
    }//PUBLIC
}//NAME
