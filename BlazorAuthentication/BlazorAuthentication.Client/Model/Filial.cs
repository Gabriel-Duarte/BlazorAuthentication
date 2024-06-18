using System.ComponentModel.DataAnnotations;

namespace BlazorAuthentication.Client.Model
{
    public class Filial
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Guid IdCompany { get; set; }
    }
}
