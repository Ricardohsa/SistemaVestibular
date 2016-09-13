using SisVest.DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisVest.DomainModel.Abstract
{
    public interface IAdimRepository
    {
        IQueryable<Admin> admins { get; }

        void Inserir(Admin admin);

        void Alterar(Admin admin);

        void Excluir(int iCandidatoID);

        Admin Retornar(int id);

    }
}
