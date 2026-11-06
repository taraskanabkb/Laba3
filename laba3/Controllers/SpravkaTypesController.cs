using Microsoft.AspNetCore.Mvc;
using MedicalApi.Models;

namespace MedicalApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SpravkaTypesController : ControllerBase
{
    public static List<SpravkaType> _spravkatype = new List<SpravkaType>
    {
        new SpravkaType { Id = 1, Name = "Справка 083", Price = 100, Date = "До 5-го дня следующего месяца" },
        new SpravkaType { Id = 2, Name = "Санитарная книжка", Price = 750, Date = "Три дня с дня получения" },
        new SpravkaType { Id = 3, Name = "Справка в детсад", Price = 250, Date = "Семь дней с дня получения" },
        new SpravkaType { Id = 4, Name = "Справка от врача", Price = 150, Date = "До 5 лет" },
    };

    [HttpGet]
    [ProducesResponseType(typeof(List<SpravkaType>), StatusCodes.Status200OK)]
    public List<SpravkaType> GetAll()
    {
        return _spravkatype;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(SpravkaType), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<SpravkaType> GetById(int id)
    {
        SpravkaType? SpravkaType = _spravkatype.FirstOrDefault(s => s.Id == id);
        if (SpravkaType == null)
        {
            return NotFound();
        }
        return SpravkaType; //возвращаем только одну справку по айди
    }

    [HttpPost]
    [ProducesResponseType(typeof(SpravkaType), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<SpravkaType> Create(SpravkaType SpravkaType)
    {
        SpravkaType.Id = _spravkatype.Count == 0 ? 1 : _spravkatype.Max(t => t.Id) + 1;
        _spravkatype.Add(SpravkaType);
        return CreatedAtAction(nameof(GetById), new { id = SpravkaType.Id }, SpravkaType);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<SpravkaType> Update(int id, SpravkaType updatedSpravkaType)
    {
        int index = _spravkatype.FindIndex(s => s.Id == id);

        if (index == -1) // Если FindIndex никого не нашел, он возвращает -1
        {
            return NotFound();
        }

        updatedSpravkaType.Id = id;
        _spravkatype[index] = updatedSpravkaType;

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult Delete(int id) 
    {
        // 1. Ищем пациента в списке по ID
        SpravkaType? SpravkaType = _spravkatype.FirstOrDefault(s => s.Id == id);

        if (SpravkaType == null)
        {
            return NotFound();
        }

        // 3. Удаляем найденного пациента из нашего списка
        _spravkatype.Remove(SpravkaType);

        return NoContent();
    }


}

