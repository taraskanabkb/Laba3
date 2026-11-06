using Microsoft.AspNetCore.Mvc;
using MedicalApi.Models;

namespace MedicalApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PriemsController : ControllerBase
{
    public static List<Priem> _priems = new List<Priem>
    {
        new Priem { Id = 1, PatientId = 1, SpravkaId = 1, Date = "2023-10-01", Doctor = "Иванов Иван Иванович" },
        new Priem { Id = 2, PatientId = 2, SpravkaId = 2, Date = "2023-10-02", Doctor = "Петрова Мария Сергеевна" },

    };

    [HttpGet]
    [ProducesResponseType(typeof(List<Priem>), StatusCodes.Status200OK)]
    public List<Priem> GetAll()
    {
        return _priems;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Priem), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]

    public ActionResult<Priem> GetById(int id)
    {
        Priem? Priem = _priems.FirstOrDefault(s => s.Id == id);
        if (Priem == null)
        {
            return NotFound();
        }
        return Priem;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Priem), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<Priem> Create(Priem priem)
    {
        Patient? patient = PatientsController._patients.FirstOrDefault(p => p.Id == priem.PatientId);
        if (patient == null)
            return BadRequest($"Пациент с Id = {priem.PatientId} не найден");

        Spravka? spravka = SpravkiController._spravki.FirstOrDefault(s => s.Id == priem.SpravkaId);
        if (spravka == null)
            return BadRequest($"Справка с Id = {priem.SpravkaId} не найдена");

        priem.Id = _priems.Count == 0 ? 1 : _priems.Max(p => p.Id) + 1;
        _priems.Add(priem);
        return CreatedAtAction(nameof(GetById), new { id = priem.Id }, priem);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<Priem> Update(int id, Priem updatedPriem)
    {
        int index = _priems.FindIndex(s => s.Id == id);

        if (index == -1) // Если FindIndex никого не нашел, он возвращает -1
        {
            return NotFound();
        }

        updatedPriem.Id = id;
        _priems[index] = updatedPriem;

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult Delete(int id) 
    {
        // 1. Ищем прием в списке по ID
        Priem? Priem = _priems.FirstOrDefault(s => s.Id == id);

        if (Priem == null)
        {
            return NotFound();
        }

        // 3. Удаляем найденный прием списка
        _priems.Remove(Priem);

        return NoContent();
    }



}

