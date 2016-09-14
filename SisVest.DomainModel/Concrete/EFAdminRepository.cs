using SisVest.DomainModel.Abstract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using SisVest.DomainModel.Entities;

namespace SisVest.DomainModel.Concrete
{
    public class EfAdminRepository : IAdimRepository
    {
        private VestContext _vestContext;

        public EfAdminRepository(VestContext context)
        {
            _vestContext = context;
        }

        public IQueryable<Admin> Admins => _vestContext.Admins.AsQueryable();

        public void Alterar(Admin admin)
        {
            var result = from a in Admins
                         where (a.SLogin.ToUpper().Equals(admin.SLogin) || a.SEmail.ToUpper().Equals(admin.SEmail)) 
                         &&!a.IAdminId.Equals(a.IAdminId)
                         select a;

            if (result.Any())
                throw new InvalidOperationException("Já existe um Administrador cadastrado com este Login ou Email.");

            _vestContext.Entry(admin).State = EntityState.Modified;
            _vestContext.SaveChanges();
        }

        public void Excluir(int iCandidatoId)
        {
            var result = from a in Admins
                where a.IAdminId.Equals(iCandidatoId)
                select a;

            if (result.Count() < 0)
                throw new InvalidOperationException("Administrador não localizado no repositório.");
            
            _vestContext.Admins.Remove(result.FirstOrDefault());
            _vestContext.SaveChanges();
        }

        public void Inserir(Admin admin)
        {
            var result = from a in Admins
                         where a.SLogin.ToUpper().Equals(admin.SLogin) || a.SEmail.ToUpper().Equals(admin.SEmail)
                         select a;

            if (result.Any())
                throw new InvalidOperationException("Administrador já cadastrado com este Login ou Email.");

            _vestContext.Admins.Add(admin);
            _vestContext.SaveChanges();
        }

        public Admin Retornar(int id)
        {
            return _vestContext.Admins.FirstOrDefault(a => a.IAdminId == id);
        }
    }
}
