using Microsoft.AspNetCore.Mvc;
using MedicalApi.Models;

namespace MedicalApi.Controllers;



[ApiController]
[Route("api/[controller]")]
public class SpravkiController : ControllerBase
{
    public static List<Spravka> _spravki = new List<Spravka>
    {
        new Spravka { Id = 1, Name = "Справка 083", PatientId = 1, Price = 100, SpravkaTypeId = 1},
        new Spravka { Id = 2, Name = "Санитарная книжка", PatientId = 2, Price = 750, SpravkaTypeId = 2},
        new Spravka { Id = 3, Name = "Справка в детсад", PatientId = 3, Price = 250, SpravkaTypeId = 3}
    };

    [HttpGet]
    [ProducesResponseType(typeof(List<Spravka>), StatusCodes.Status200OK)]
    public List<Spravka> GetAll()
    {
        return _spravki;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Spravka), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Spravka> GetById(int id)
    {
        Spravka? spravka = _spravki.FirstOrDefault(s => s.Id == id);
        if (spravka == null)
        {
            return NotFound();
        }
        return spravka;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Spravka), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<Spravka> Create(Spravka spravka)
    {
        // ищем пациента в чужом (теперь уже публичном) списке
        Patient? patient = PatientsController._patients.FirstOrDefault(p => p.Id == spravka.PatientId);
        SpravkaType? type = SpravkaTypesController._spravkatype.FirstOrDefault(d => d.Id == spravka.SpravkaTypeId);
        if (patient == null)
        {
            return BadRequest($"Пациент с Id = {spravka.PatientId} не найден");
        }
        if (type == null)
        {
            return BadRequest($"Тип справки с Id = {spravka.SpravkaTypeId} не найден");
        }

        spravka.Id = _spravki.Count == 0 ? 1 : _spravki.Max(s => s.Id) + 1;
        spravka.Price = type.Price;
        spravka.Name = type.Name;
        _spravki.Add(spravka);
        return CreatedAtAction(nameof(GetById), new { id = spravka.Id }, spravka);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult Delete(int id)
    {
        Spravka? Spravka = _spravki.FirstOrDefault(s => s.Id == id);

        if (Spravka == null)
        {
            return NotFound();
        }

        _spravki.Remove(Spravka);

        return NoContent();
    }
}
