using Microsoft.AspNetCore.Identity;

namespace RentCar.Data.Context
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        //Agrego mas campos al ASP.NET.USERS
        public string Nombre { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public bool Activo { get; set; }
    }

}
