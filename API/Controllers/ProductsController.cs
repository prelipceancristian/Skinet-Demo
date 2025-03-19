using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IGenericRepository<Product> repo) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts(string? brand, string? type, string? sort)
    {
        var spec = new ProductSpecification(brand, type, sort);
        var products = await repo.ListAsync(spec);
        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await repo.GetByIdAsync(id);
        if (product == null) return NotFound();
        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
    {
        repo.Add(product);
        if (await repo.SaveAllAsync())
        {
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }

        return BadRequest("Cannot create this product");
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, [FromBody] Product product)
    {
        if (product.Id != id || !repo.Exists(id)) return BadRequest("Cannot upadte this product");

        repo.Update(product);
        if (await repo.SaveAllAsync())
        {
            return NoContent();
        }

        return BadRequest("Cannot update this product");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await repo.GetByIdAsync(id);
        if (product == null) return NotFound();

        repo.Remove(product);
        if (await repo.SaveAllAsync())
        {
            return NoContent();
        }

        return BadRequest("Cannot delete this product");
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        var brandSpec = new BrandListSpecification();
        var brands = await repo.ListAsync(brandSpec);
        return Ok(brands);
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        var typesSpec = new TypeListSpecification();
        var types = await repo.ListAsync(typesSpec);
        return Ok(types);
    }
}
