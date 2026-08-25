using Verity.FluxoCaixa.Dominio.Entidades;
using Verity.FluxoCaixa.Dominio.Enums;

namespace Verity.FluxoCaixa.Testes;

public class LancamentoTests
{
    [Theory]
    [InlineData(100, TipoLancamento.Credito, "Venda de produto")]
    [InlineData(50.5, TipoLancamento.Debito, "Pagamento de fornecedor")]
    [InlineData(0.01, TipoLancamento.Credito, "Ajuste de centavos")]
    public void DeveCriarLancamentoValido(decimal valor, TipoLancamento tipo, string descricao)
    {
        var data = DateOnly.FromDateTime(DateTime.Today);

        var lancamento = new Lancamento(data, valor, tipo, descricao);

        Assert.NotEqual(Guid.Empty, lancamento.Id);
        Assert.Equal(data, lancamento.Data);
        Assert.Equal(valor, lancamento.Valor);
        Assert.Equal(tipo, lancamento.Tipo);
        Assert.Equal(descricao, lancamento.Descricao);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-0.01)]
    [InlineData(-1)]
    [InlineData(-10)]
    [InlineData(-100.5)]
    public void DeveRejeitarValorMenorOuIgualZero(decimal valor)
    {
        Assert.Throws<ArgumentException>(() =>
            new Lancamento(
                DateOnly.FromDateTime(DateTime.Today),
                valor,
                TipoLancamento.Credito,
                "Descrição Teste"
            )
        );
    }
}
