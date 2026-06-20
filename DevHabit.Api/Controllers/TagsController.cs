using DevHabit.Api.Database;
using DevHabit.Api.DTOs.Tags;
using DevHabit.Api.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DevHabit.Api.Controllers;

[ApiController]
[Route("tags")]
public sealed class TagsController(ApplicationDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<TagsCollectionDto>> GetTags()
    {
        List<TagDto> tags = await dbContext.Tags.Select(TagQueries.ProjectToDto()).ToListAsync();
        return Ok(new TagsCollectionDto { Data = tags });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TagDto>> GetTag(string id)
    {
        TagDto? tag = await dbContext.Tags.Where(t => t.Id == id).Select(TagQueries.ProjectToDto()).FirstOrDefaultAsync();
        if(tag is null)
        {
            return NotFound();
        }
        return Ok(tag);
    }

    [HttpPost]
    public async Task<ActionResult<TagDto>> CreateTag(CreateTagDto dto, IValidator<CreateTagDto> validator, ProblemDetailsFactory probDetailsFactory)
    {
        //ValidationResult vs = await validator.ValidateAsync(dto);
        await validator.ValidateAndThrowAsync(dto);
        //if (!vs.IsValid)
        //{
        //    ProblemDetails problem = probDetailsFactory.CreateProblemDetails(
        //        HttpContext, 
        //        StatusCodes.Status400BadRequest
        //    );
        //    problem.Extensions.Add("errors", vs.ToDictionary());
        //    return BadRequest(problem);
        //}

        Tag tag = dto.ToEntity();
        if(await dbContext.Tags.AnyAsync(t => t.Name == tag.Name))
        {
            return Problem(
                detail: $"The tag '{tag.Name}' already exists",
                statusCode: StatusCodes.Status409Conflict);
        }

        dbContext.Add(tag);
        await dbContext.SaveChangesAsync();

        TagDto tagDto = tag.ToDto();

        return CreatedAtAction(nameof(GetTag), new { id = tagDto.Id }, tagDto);

    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateTag(string id, UpdateTagDto dto)
    {
        Tag? tag = await dbContext.Tags.FirstOrDefaultAsync(t => t.Id == id);
        if(tag is null) { return NotFound(); }

        tag.UpdateFromDto(dto);

        await dbContext.SaveChangesAsync();

        return NoContent();
        
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTag(string id)
    {
        Tag? tag = await dbContext.Tags.FirstOrDefaultAsync(t => t.Id == id);
        if(tag is null) { return NotFound(); }

        dbContext.Remove(tag);

        await dbContext.SaveChangesAsync();

        return NoContent();
    }


}
