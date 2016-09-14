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
        IQueryable<Candidato> Candidatos { get; }

        void RealizarInscricao(Candidato candidato);

        void AtualizarCadasto(Candidato candidato);

        void ExcluirCadastro(int iCandidatoId);

        void Aprovar(int iCandidatoId);

        Candidato Retornar(int iCandidatoId);

        IList<Candidato> RetornarTodos();

        IList<Candidato> RetornarCandidatossPorVestibularPorCurso(int iVestibularId, int iCursoId);  
    }
}
