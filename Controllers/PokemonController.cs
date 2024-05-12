using Microsoft.AspNetCore.Mvc;

// Controllers/PokemonController.cs
[ApiController]
[Route("api/[controller]")]
public class PokemonController : ControllerBase
{
    private static List<Pokemon> pokemones = new List<Pokemon>();

    // 1. Para crear 1 pokémon
    [HttpPost]
    public IActionResult CrearPokemon(Pokemon pokemon)
    {
        pokemon.Id = pokemones.Count + 1;
        pokemon.Habilidades = pokemon.Habilidades?.Select(h => new Habilidad
        {
            Nombre = h.Nombre,
            Poder = h.Poder >= 0 && h.Poder <= 40 ? h.Poder : 0
        }).ToArray();
        pokemon.Defensa = pokemon.Defensa >= 1 && pokemon.Defensa <= 30 ? pokemon.Defensa : 1;
        pokemones.Add(pokemon);
        return CreatedAtAction(nameof(ObtenerPokemon), new { id = pokemon.Id }, pokemon);
    }

    // 2. Para crear múltiples pokemones
    [HttpPost("multiple")]
    public IActionResult CrearPokemones(List<Pokemon> pokemones)
    {
        foreach (var pokemon in pokemones)
        {
            pokemon.Id = pokemones.Count + 1;
            pokemon.Habilidades = pokemon.Habilidades?.Select(h => new Habilidad
            {
                Nombre = h.Nombre,
                Poder = h.Poder >= 0 && h.Poder <= 40 ? h.Poder : 0
            }).ToArray();
            pokemon.Defensa = pokemon.Defensa >= 1 && pokemon.Defensa <= 30 ? pokemon.Defensa : 1;
            pokemones.Add(pokemon);
        }
        return Ok(pokemones);
    }

    // 3. Para editar 1 pokémon
    [HttpPut("{id}")]
    public IActionResult EditarPokemon(int id, Pokemon pokemon)
    {
        var pokemonExistente = pokemones.Find(p => p.Id == id);
        if (pokemonExistente == null)
            return NotFound();

        pokemonExistente.Nombre = pokemon.Nombre;
        pokemonExistente.Tipo = pokemon.Tipo;
        pokemonExistente.Habilidades = pokemon.Habilidades?.Select(h => new Habilidad
        {
            Nombre = h.Nombre,
            Poder = h.Poder >= 0 && h.Poder <= 40 ? h.Poder : 0
        }).ToArray();
        pokemonExistente.Defensa = pokemon.Defensa >= 1 && pokemon.Defensa <= 30 ? pokemon.Defensa : 1;

        return NoContent();
    }

    // 4. Para eliminar 1 pokémon
    [HttpDelete("{id}")]
    public IActionResult EliminarPokemon(int id)
    {
        var pokemonExistente = pokemones.Find(p => p.Id == id);
        if (pokemonExistente == null)
            return NotFound();

        pokemones.Remove(pokemonExistente);
        return NoContent();
    }

    // 5. Para traer 1 pokémon
    [HttpGet("{id}")]
    public IActionResult ObtenerPokemon(int id)
    {
        var pokemon = pokemones.Find(p => p.Id == id);
        if (pokemon == null)
            return NotFound();

        return Ok(pokemon);
    }

    // 6. Para traer todos los pokemones de un tipo
    [HttpGet("tipo/{tipo}")]
    public IActionResult ObtenerPokemonesPorTipo(string tipo)
    {
        var pokemonesPorTipo = pokemones.FindAll(p => p.Tipo == tipo);
        if (pokemonesPorTipo.Count == 0)
            return NotFound();

        return Ok(pokemonesPorTipo);
    }

    // 7. Endpoint personalizado: Obtener pokemones con una defensa mayor a un valor específico
    [HttpGet("defensa/{valor}")]
    public IActionResult ObtenerPokemonesConDefensaMayor(double valor)
    {
        var pokemonesConDefensaMayor = pokemones.FindAll(p => p.Defensa > valor);
        if (pokemonesConDefensaMayor.Count == 0)
            return NotFound();

        return Ok(pokemonesConDefensaMayor);
    }

    // 8. Endpoint personalizado: Obtener pokemones con un poder de habilidad mayor a un valor específico
    [HttpGet("habilidad/{valor}")]
    public IActionResult ObtenerPokemonesConHabilidadMayor(int valor)
    {
        var pokemonesConHabilidadMayor = pokemones.FindAll(p => p.Habilidades != null && p.Habilidades.Any(h => h.Poder > valor));
        if (pokemonesConHabilidadMayor.Count == 0)
            return NotFound();

        return Ok(pokemonesConHabilidadMayor);
    }
}