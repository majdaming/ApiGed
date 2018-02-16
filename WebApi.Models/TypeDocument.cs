using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebApi.Models
{
    public class TypeDocument
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
