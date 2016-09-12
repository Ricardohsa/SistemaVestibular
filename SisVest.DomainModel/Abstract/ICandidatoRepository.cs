using SisVest.DomainModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisVest.DomainModel.Abstract
{
    public interface ICandidatoRepository
    {
        IQueryable<Candidato> candidatos { get; }

        void RealizarInscricao(Candidato candidato);

        void AtualizarCadasto(Candidato candidato);

        void ExcluirCadastro(int iCandidatoID);

        void Aprovar(int iCandidatoID);

        Candidato Retornar(int iCandidatoId);

        IList<Candidato> RetornarTodos();

        IList<Candidato> RetornarCandidatossPorVestibularPorCurso(int iVestibularID, int iCursoID);  
    }
}
