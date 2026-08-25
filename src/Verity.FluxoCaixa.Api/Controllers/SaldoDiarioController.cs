using Microsoft.AspNetCore.Mvc;
using Verity.FluxoCaixa.Aplicacao.Dtos;
using Verity.FluxoCaixa.Aplicacao.Servicos;

namespace Verity.FluxoCaixa.Api.Controllers;

[ApiController]
[Route("api/saldo-diario")]
public class SaldoDiarioController(SaldoDiarioConsultaService saldoDiarioConsultaService)
    : ControllerBase
{
    [HttpGet("{data}")]
    [ProducesResponseType(typeof(SaldoDiarioResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<SaldoDiarioResponse>> Obter(
        DateOnly data,
        CancellationToken cancellationToken
    )
    {
        var response = await saldoDiarioConsultaService.ObterAsync(data, cancellationToken);
        return Ok(response);
    }
}
