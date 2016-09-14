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
        IQueryable<Admin> Admins { get; }

        void Inserir(Admin admin);

        void Alterar(Admin admin);

        void Excluir(int iCandidatoId);

        Admin Retornar(int id);

    }
}
