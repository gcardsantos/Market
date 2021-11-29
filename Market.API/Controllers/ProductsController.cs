using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Market.Domain.Entities;
using Market.Infrastructure.Context;

namespace Market.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        /// <summary>
        /// Busca todos os produtos.
        /// </summary>
        /// <returns>Busca um produto já criado.</returns>
        /// <response code="200">Sucesso na requisição dos produtos.</response>
        /// <response code="404">Produtos não encontrados.</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        // GET: api/Products/5
        /// <summary>
        /// Busca um produto específico.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Busca um produto já criado.</returns>
        /// <response code="200">Sucesso na requisição do produto.</response>
        /// <response code="404">Product não encontrado.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Modifica um produto.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="product"></param>
        /// <returns>Modifica um Product já criado.</returns>
        /// <remarks>
        /// Exemplo:
        ///
        ///     {
        ///        "id": 5,
        ///        "name": "Nome do produto",
        ///        "price": 10
        ///        "quantity": 1
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Sucesso na requisição de alteração de específico produto.</response>
        /// <response code="204">Sucesso na requisição de alteração de específico produto.</response>
        /// <response code="400">Retorno nulo.</response>
        /// <response code="404">Produto não encontrado.</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /// <summary>
        /// Insere um produto.
        /// </summary>
        /// <param name="product"></param>
        /// <returns>Um novo Product criado.</returns>
        /// <remarks>
        /// Exemplo:
        ///
        ///     {
        ///        "name": "Nome do produto",
        ///        "price": 10,
        ///        "quantity": 1
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Sucesso na requisição</response>
        /// <response code="201">Cria um novo produto e retorna o novo item criado</response>
        /// <response code="400">Retorno nulo.</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        /// <summary>
        /// Remove um produto pelo Index.
        /// </summary>
        /// <param name="id"></param>
        /// <response code="404">Retorna se o produto não foi encontrado.</response>      
        /// <response code="204">Produto removido ou não existente.</response>      
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
