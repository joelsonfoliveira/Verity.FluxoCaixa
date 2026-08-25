using Verity.FluxoCaixa.Dominio.Entidades;

namespace Verity.FluxoCaixa.Aplicacao.Dtos;

public record SaldoDiarioResponse(
    DateOnly Data,
    decimal TotalCreditos,
    decimal TotalDebitos,
    decimal Saldo
)
{
    public static SaldoDiarioResponse ParaResponse(SaldoDiario saldoDiario) =>
        new(
            saldoDiario.Data,
            saldoDiario.TotalCreditos,
            saldoDiario.TotalDebitos,
            saldoDiario.Saldo
        );
}
