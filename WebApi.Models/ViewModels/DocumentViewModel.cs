using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi.Models.ViewModels
{
    public class DocumentViewModel : Document
    {

        public IFormFile uploadedFile { get; set; }
    }
}
