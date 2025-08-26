using Microsoft.AspNetCore.Mvc;
using RandomNumbersApi.Services; // <- importante para IRandomService
using RandomNumbersApi.Models;   // <- importante para CustomRandomRequest/RandomType

namespace RandomNumbersApi.Controllers;

[ApiController]
[Route("random")]
public class RandomController : ControllerBase
{
    private readonly IRandomService _rng;

    public RandomController(IRandomService rng) => _rng = rng;

    [HttpGet("number")]
    public IActionResult GetNumber([FromQuery] int? min, [FromQuery] int? max)
    {
        if (min.HasValue || max.HasValue)
        {
            if (min is null || max is null)
                return BadRequest(new { error = "Debe proporcionar min y max a la vez." });

            if (min > max)
                return BadRequest(new { error = "min no puede ser mayor que max." });

            var value = _rng.NextIntInclusive(min.Value, max.Value);
            return Ok(new { result = value });
        }

        var v = _rng.NextIntNonNegative();
        return Ok(new { result = v });
    }

    [HttpGet("decimal")]
    public IActionResult GetDecimal()
    {
        var value = _rng.NextUnitIntervalDouble();
        return Ok(new { result = value });
    }

    [HttpGet("string")]
    public IActionResult GetString([FromQuery] int length = 8)
    {
        if (length < 1 || length > 1024)
            return BadRequest(new { error = "length debe estar entre 1 y 1024." });

        var s = _rng.NextString(length);
        return Ok(new { result = s });
    }

    [HttpPost("custom")]
    public IActionResult PostCustom([FromBody] CustomRandomRequest req)
    {
        if (req is null) return BadRequest(new { error = "Body requerido." });

        switch (req.Type)
        {
            case RandomType.Number:
            {
                int min = req.Min ?? 0;
                int max = req.Max ?? int.MaxValue;
                if (min > max) return BadRequest(new { error = "min no puede ser mayor que max." });
                var v = _rng.NextIntInclusive(min, max);
                return Ok(new { result = v });
            }

            case RandomType.Decimal:
            {
                int decimals = req.Decimals.GetValueOrDefault(2);
                if (decimals < 0 || decimals > 10)
                    return BadRequest(new { error = "decimals debe estar entre 0 y 10." });

                double d = _rng.NextUnitIntervalDouble();
                double rounded = Math.Round(d, decimals, MidpointRounding.AwayFromZero);
                return Ok(new { result = rounded });
            }

            case RandomType.String:
            {
                int length = req.Length.GetValueOrDefault(8);
                if (length < 1 || length > 1024)
                    return BadRequest(new { error = "length debe estar entre 1 y 1024." });

                var s = _rng.NextString(length);
                return Ok(new { result = s });
            }

            default:
                return BadRequest(new { error = "type no soportado." });
        }
    }
}
