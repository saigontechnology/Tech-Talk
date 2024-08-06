//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using SharedDomains;
//using WebAPI.Data;

//namespace WebAPI
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class StoreController : ControllerBase
//    {
//        private readonly StoreContext _context;

//        public StoreController(StoreContext context)
//        {
//            _context = context;
//        }

//        // GET: api/Store
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<StoreModel>>> GetStoreModel()
//        {
//            return await _context.StoreModel.ToListAsync();
//        }

//        // GET: api/Store/5
//        [HttpGet("{id}")]
//        public async Task<ActionResult<StoreModel>> GetStoreModel(Guid id)
//        {
//            var storeModel = await _context.StoreModel.FindAsync(id);

//            if (storeModel == null)
//            {
//                return NotFound();
//            }

//            return storeModel;
//        }

//        // PUT: api/Store/5
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPut("{id}")]
//        public async Task<IActionResult> PutStoreModel(Guid id, StoreModel storeModel)
//        {
//            if (id != storeModel.Id)
//            {
//                return BadRequest();
//            }

//            _context.Entry(storeModel).State = EntityState.Modified;

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!StoreModelExists(id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }

//            return NoContent();
//        }

//        // POST: api/Store
//        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
//        [HttpPost]
//        public async Task<ActionResult<StoreModel>> PostStoreModel(StoreModel storeModel)
//        {
//            _context.StoreModel.Add(storeModel);
//            await _context.SaveChangesAsync();

//            return CreatedAtAction("GetStoreModel", new { id = storeModel.Id }, storeModel);
//        }

//        // DELETE: api/Store/5
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteStoreModel(Guid id)
//        {
//            var storeModel = await _context.StoreModel.FindAsync(id);
//            if (storeModel == null)
//            {
//                return NotFound();
//            }

//            _context.StoreModel.Remove(storeModel);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }

//        private bool StoreModelExists(Guid id)
//        {
//            return _context.StoreModel.Any(e => e.Id == id);
//        }
//    }
//}
