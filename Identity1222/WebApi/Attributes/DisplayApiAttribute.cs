using System;

namespace WebApi.Attributes
{
    public class DisplayApiAttribute : Attribute
    {
        private string role;
        public DisplayApiAttribute(string Role)
        {
            role = Role;
        }
    }
}
