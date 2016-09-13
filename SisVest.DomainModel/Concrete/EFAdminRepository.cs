using SisVest.DomainModel.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using SisVest.DomainModel.Entities;

namespace SisVest.DomainModel.Concrete
{
    public class EFAdminRepository : IAdimRepository
    {
        private VestContext vestContext;

        public EFAdminRepository(VestContext context)
        {
            vestContext = context;
        }

        public IQueryable<Admin> admins => vestContext.Admins.AsQueryable();

        public void Alterar(Admin admin)
        {
            var result = from a in admins
                         where (a.sLogin.ToUpper().Equals(admin.sLogin) || a.sEmail.ToUpper().Equals(admin.sEmail)) 
                         &&!a.iAdminId.Equals(a.iAdminId)
                         select a;

            if (result.Count() > 0)
                throw new InvalidOperationException("Já existe um Administrador cadastrado com este Login ou Email.");

            vestContext.SaveChanges();
        }

        public void Excluir(int iCandidatoID)
        {
            var result = from a in admins
                where a.iAdminId.Equals(iCandidatoID)
                select a;

            if (result.Count() == 0)
                throw new InvalidOperationException("Administrador não localizado no repositório.");
            
            vestContext.Admins.Remove(result.FirstOrDefault());
            vestContext.SaveChanges();
        }

        public void Inserir(Admin admin)
        {
            var result = from a in admins
                         where a.sLogin.ToUpper().Equals(admin.sLogin) || a.sEmail.ToUpper().Equals(admin.sEmail)
                         select a;

            if (result.Count() > 0)
                throw new InvalidOperationException("Administrador já cadastrado com este Login ou Email.");

            vestContext.Admins.Add(admin);
            vestContext.SaveChanges();
        }

        public Admin Retornar(int id)
        {
            return vestContext.Admins.Where(a => a.iAdminId == id).FirstOrDefault();
        }
    }
}
