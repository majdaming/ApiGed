using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WebApi.Models
{
    public class Document
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime PublishedAt { get; set; }
        
        public byte[] FileData { set; get; }
        [Required]
        [ForeignKey("TypeDocument")]
        public int TypeId { get; set; }
        public TypeDocument Type { get; set; }
    }
}
