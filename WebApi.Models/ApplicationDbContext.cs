using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApi.Models
{
    public class ApplicationDbContext :  IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }



        public DbSet<TypeDocument> TypeDocuments { get; set; }
        public DbSet<Document> Documents { get; set; }


    }
}
