using Microsoft.AspNetCore.Mvc;
using Verity.FluxoCaixa.Aplicacao.Dtos;
using Verity.FluxoCaixa.Aplicacao.Servicos;

namespace Verity.FluxoCaixa.Api.Controllers;

[ApiController]
[Route("api/lancamentos")]
public class LancamentosController(LancamentoService lancamentoService) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(LancamentoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<LancamentoResponse>> Registrar(
        [FromBody] RegistrarLancamentoRequest request,
        CancellationToken cancellationToken
    )
    {
        var response = await lancamentoService.RegistrarAsync(request, cancellationToken);

        return CreatedAtAction(
            nameof(ObterPorData),
            new { data = response.Data.ToString("yyyy-MM-dd") },
            response
        );
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<LancamentoResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<LancamentoResponse>>> ObterPorData(
        [FromQuery] DateOnly data,
        CancellationToken cancellationToken
    )
    {
        var response = await lancamentoService.ObterPorDataAsync(data, cancellationToken);
        return Ok(response);
    }
}
