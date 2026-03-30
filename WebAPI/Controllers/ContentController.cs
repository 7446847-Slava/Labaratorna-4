using Microsoft.AspNetCore.Mvc;
using BLL;
using System.Collections.Generic;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        private readonly ILibraryService _service;

        // DI в дії: сервіс приходить автоматично, ми не пишемо "new LibraryService()"
        public ContentController(ILibraryService service)
        {
            _service = service;
        }

        // 1. GET: Отримати весь контент
        [HttpGet]
        public ActionResult<IEnumerable<ContentDto>> GetAll()
        {
            return Ok(_service.GetAllContent());
        }

        // 2. POST: Додати новий контент
        [HttpPost]
        public ActionResult Post([FromBody] ContentDto content)
        {
            // Для прикладу прив'язуємо до локації 1
            _service.AddContent(content.Title, content.ContentType, content.Format, 1);
            return Ok(new { message = "Контент успішно додано!" });
        }

        // 3. PUT: Оновити існуючий контент за ID
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] ContentDto content)
        {
            bool success = _service.UpdateContent(id, content.Title, content.Format);
            if (!success) return NotFound(new { message = "Контент не знайдено." });
            return Ok(new { message = "Контент успішно оновлено!" });
        }

        // 4. DELETE: Видалити контент за ID
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            bool success = _service.DeleteContent(id);
            if (!success) return NotFound(new { message = "Контент не знайдено." });
            return Ok(new { message = "Контент успішно видалено!" });
        }
    }
}