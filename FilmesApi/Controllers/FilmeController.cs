using AutoMapper;
using FilmesApi.Data;
using FilmesAPI.Data.Dtos;
using FilmesAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace FilmesAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmeController : ControllerBase
{
    private FilmeContext _context;
    private IMapper _mapper;

    public FilmeController(FilmeContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    /// Adiciona um filme ao banco de dados
    /// </summary>
    /// <param name="filmeDto">Objeto com os campos necessários para criação de um filme</param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult AdicionaFilme([FromBody] CreateFilmeDto filmeDto)
    {
        Filme filme = _mapper.Map<Filme>(filmeDto);
        _context.Filmes.Add(filme);
        _context.SaveChanges();
        return CreatedAtAction(nameof(RecuperaFilmesPorId), new { Id = filme.Id }, filme);
    }

    /// <summary>
    /// Recupera uma lista de filmes do banco de dados
    /// </summary>
    /// <param name="skip">Número de filmes que serão pulados</param>
    /// <param name="take">Número de filmes que serão recuperados</param>
    /// <returns>Informações dos filmes buscados</returns>
    /// <response code="200">Com a lista de filmes presentes na base de dados</response>
    [HttpGet]
    public IEnumerable<Filme> RecuperaFilmes(int skip = 0, int take = 10)
    {
        return _context.Filmes.Skip(skip).Take(take);
    }

    /// <summary>
    /// Recupera um filme no banco de dados usando seu id
    /// </summary>
    /// <param name="id">Id do filme a ser recuperado no banco</param>
    /// <returns>Informações do filme buscado</returns>
    /// <response code="200">Caso o id seja existente na base de dados</response>
    /// <response code="404">Caso o id seja inexistente na base de dados</response>
    [HttpGet("{id}")]
    public IActionResult RecuperaFilmesPorId(int id)
    {
        Filme filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme != null)
        {
            ReadFilmeDto filmeDto = _mapper.Map<ReadFilmeDto>(filme);
            return Ok(filmeDto);
        }
        return NotFound();
    }

    /// <summary>
    /// Atualiza um filme no banco de dados usando seu id
    /// </summary>
    /// <param name="id">Id do filme a ser atualizado no banco</param>
    /// <param name="filmeDto">Objeto com os campos necessários para atualização de um filme</param>
    /// <returns>Sem conteúdo de retorno</returns>
    /// <response code="204">Caso o id seja existente na base de dados e o filme tenha sido atualizado</response>
    /// <response code="404">Caso o id seja inexistente na base de dados</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult AtualizaFilme(int id, [FromBody] UpdateFilmeDto filmeDto)
    {
        Filme filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme != null)
        {
            _mapper.Map(filmeDto, filme);
            _context.SaveChanges();
            return NoContent();
        }
        return NotFound();
    }

    /// <summary>
    /// Deleta um filme do banco de dados usando seu id
    /// </summary>
    /// <param name="id">Id do filme a ser removido do banco</param>
    /// <returns>Sem conteúdo de retorno</returns>
    /// <response code="204">Caso o id seja existente na base de dados e o filme tenha sido removido</response>
    /// <response code="404">Caso o id seja inexistente na base de dados</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeletaFilme(int id)
    {
        Filme filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme != null)
        {
            _context.Remove(filme);
            _context.SaveChanges();
            return NoContent();
        }
        return NotFound();
    }

}
