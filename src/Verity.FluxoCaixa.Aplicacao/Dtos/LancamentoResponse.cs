using Verity.FluxoCaixa.Dominio.Entidades;
using Verity.FluxoCaixa.Dominio.Enums;

namespace Verity.FluxoCaixa.Aplicacao.Dtos;

public record LancamentoResponse(
    Guid Id,
    DateOnly Data,
    decimal Valor,
    TipoLancamento Tipo,
    string Descricao,
    DateTime CriadoEm
)
{
    public static LancamentoResponse ParaResponse(Lancamento lancamento) =>
        new(
            lancamento.Id,
            lancamento.Data,
            lancamento.Valor,
            lancamento.Tipo,
            lancamento.Descricao,
            lancamento.CriadoEm
        );
}
