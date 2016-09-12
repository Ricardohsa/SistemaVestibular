using SisVest.DomainModel.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public IQueryable<Admin> admins
        {
            get
            {
                return vestContext.Admins.AsQueryable();
            }
        }

        public void Alterar(Admin admin)
        {
            throw new NotImplementedException();
        }

        public void Exclir(int iCandidatoID)
        {
            throw new NotImplementedException();
        }

        public void Inserir(Admin admin)
        {
            vestContext.Admins.Add(admin);
            vestContext.SaveChanges();
        }

        public Admin Retornar(int id)
        {
            throw new NotImplementedException();
        }
    }
}
