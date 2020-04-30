using Microsoft.AspNetCore.Mvc;

namespace Domain.Authorize
{
    public static class NoAuthorize
    {
        public static IActionResult DenyAccess()
        {
            var naoAutorizado = new ObjectResult(new { statusCode = 401, message = "Não autorizado!" })
            {
                StatusCode = 401
            };

            return naoAutorizado;
        }

        public static IActionResult DenyAccessLogin()
        {
            var naoAutorizado = new ObjectResult(new { statusCode = 401, message = "Email e/ou senha incorretos" })
            {
                StatusCode = 401
            };

            return naoAutorizado;
        }

        public static IActionResult LogOut()
        {
            var logOut = new ObjectResult(new { statusCode = 440, message = "Sessão expirou" })
            {
                StatusCode = 440
            };

            return logOut;
        }
    }
}
