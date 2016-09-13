using SisVest.DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisVest.DomainModel.Concrete
{
    public class VestContext :DbContext
    {
        public VestContext()
            :base("SISVEST")
        {
         
        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    Database.SetInitializer(new DropCreateDatabaseIfModelChanges<VestContext>()); 
        //}

        public DbSet<Admin> Admins { get; set; }

        public DbSet<Candidato> Candidatos { get; set; }

        public DbSet<Curso> Cursos { get; set; }

        public DbSet<Vestibular> Vestibulares { get; set; }
    }
}
