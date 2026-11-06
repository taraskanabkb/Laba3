using Microsoft.AspNetCore.Mvc;
using MedicalApi.Models;

namespace MedicalApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    public static List<Patient> _patients = new List<Patient>
    {
        new Patient { Id = 1, Name = "Иванов Иван Иванович", BirthDate = new DateTime(1995, 5, 20),  BloodType = "A+", Old = 31, inn = "12345678910" },
        new Patient { Id = 2, Name = "Петрова Мария Сергеевна", BirthDate = new DateTime(1990, 8, 15),  BloodType = "B-", Old = 36, inn = "10987654321" },
        new Patient { Id = 3, Name = "Сидоров Алексей Владимирович", BirthDate = new DateTime(1985, 12, 10),  BloodType = "AB+", Old = 41, inn = "11223344556" },
        new Patient { Id = 4, Name = "Козлова Елена Николаевна", BirthDate = new DateTime(1992, 3, 25),  BloodType = "O-", Old = 34, inn = "65432109876" }, //DateTime год и возраст должны соотвествовать 2026

    };
    [HttpGet]
    [ProducesResponseType(typeof(List<Patient>), StatusCodes.Status200OK)]
    public List<Patient> GetAll()
    {
        return _patients;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Patient), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Patient> GetById(int id)
    {
        Patient? patient = _patients.FirstOrDefault(s => s.Id == id);
        if (patient == null)
        {
            return NotFound();
        }
        return patient;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Patient), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<Patient> Create(Patient patient)
    {
        patient.Id = _patients.Count == 0 ? 1 : _patients.Max(p => p.Id) + 1;
        _patients.Add(patient);
        return CreatedAtAction(nameof(GetById), new { id = patient.Id }, patient);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<Patient> Update(int id, Patient updatedPatient)
    {
        int index = _patients.FindIndex(s => s.Id == id);

        if (index == -1) // Если FindIndex никого не нашел, он возвращает -1
        {
            return NotFound();
        }

        updatedPatient.Id = id;
        _patients[index] = updatedPatient;

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult Delete(int id) 
    {
        // 1. Ищем пациента в списке по ID
        Patient? patient = _patients.FirstOrDefault(s => s.Id == id);

        if (patient == null)
        {
            return NotFound();
        }

        // 3. Удаляем найденного пациента из нашего списка
        _patients.Remove(patient);
        PriemsController._priems.RemoveAll(p => p.PatientId == id);
        SpravkiController._spravki.RemoveAll(s => s.PatientId == id);

        return NoContent();
    }

    [HttpGet("{id}/spravki")]
    [ProducesResponseType(typeof(List<Spravka>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<List<Spravka>> GetSpravki(int id) 
    {
        Patient? patient = _patients.FirstOrDefault(s => s.Id == id);
        if (patient == null)
        {
            return NotFound();
        }

        // 2. Собираем все справки, у которых PatientId совпадает с id пациента
        List<Spravka> result = SpravkiController._spravki
            .Where(s => s.PatientId == id)
            .ToList();

        return result; // 200 со списком
    }

}

