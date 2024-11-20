using System.Buffers.Text;
using BloodBankWebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BloodBankWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BloodBankController : ControllerBase
    {
        //Adding default entries in bloodbank list
        static List<BloodBankEntry> BloodBanksList = new List<BloodBankEntry>
        {
            new BloodBankEntry
            {
                Id=1,
                DonorName="Alice",
                Age=30,
                BloodType="A+",
                ContactInfo = "alice.johnson@example.com",
                Quantity = 500,
                CollectionDate = DateTime.Now.AddDays(-2),
                ExpirationDate = DateTime.Now.AddDays(28),
                Status = "Available"
            },
            new BloodBankEntry
        {
             Id=2,
            DonorName = "Bob",
            Age = 40,
            BloodType = "O-",
            ContactInfo = "bob.smith@example.com",
            Quantity = 450,
            CollectionDate = DateTime.Now.AddDays(-5),
            ExpirationDate = DateTime.Now.AddDays(25),
            Status = "Requested"
        },
         new BloodBankEntry
        {
            Id=3,
            DonorName = "Charlie",
            Age = 35,
            BloodType = "B+",
            ContactInfo = "charlie.brown@example.com",
            Quantity = 300,
            CollectionDate = DateTime.Now.AddDays(-1),
            ExpirationDate = DateTime.Now.AddDays(29),
            Status = "Expired"
        }

        };

        //Create and Add BloodBankEntry
        [HttpPost]
        public ActionResult<BloodBankEntry> AddBloodBankEntry(BloodBankEntry entry)
        {
            string validationStatus = IsValidEntry(entry);
            if (validationStatus != null)
            {
                return BadRequest(validationStatus);
            }
            entry.Id = BloodBanksList.Any() ? BloodBanksList.Max(i => i.Id) + 1 : 1;
            BloodBanksList.Add(entry);
            return CreatedAtAction(nameof(GetBloodBankEntryById), new { id = entry.Id }, entry);

        }

        //Retrieve all entries
        [HttpGet]
        public ActionResult<IEnumerable<BloodBankEntry>> GetAllBloodBankEntries()
        {
            return BloodBanksList.ToList();
        }

        //Retrieve bloodbank entry by id
        [HttpGet("{id}")]
        public ActionResult<BloodBankEntry> GetBloodBankEntryById(int id)
        {
            var entry = BloodBanksList.Find(e => e.Id == id);
            if (entry == null)
            {
                return NotFound();
            }
            return entry;
        }

        //Update an existing entry
        [HttpPut("{id}")]
        public IActionResult UpdateBloodBankEntry(int id, BloodBankEntry Updatedentry)
        {
            var ExistingEntry = BloodBanksList.Find(e => e.Id == id);
            if (ExistingEntry == null)
            {
                return NotFound();
            }
            string validationStatus = IsValidEntry(Updatedentry);
            if (validationStatus != null)
            {
                return BadRequest(validationStatus);
            }

            ExistingEntry.DonorName = Updatedentry.DonorName;
            ExistingEntry.Age = Updatedentry.Age;
            ExistingEntry.BloodType = Updatedentry.BloodType;
            ExistingEntry.ContactInfo = Updatedentry.ContactInfo;
            ExistingEntry.Quantity = Updatedentry.Quantity;
            ExistingEntry.CollectionDate = Updatedentry.CollectionDate;
            ExistingEntry.ExpirationDate = Updatedentry.ExpirationDate;
            ExistingEntry.Status = Updatedentry.Status;

            return NoContent();
        }

        //Remove an entry from the list based on its Id.
        [HttpDelete("{id}")]
        public IActionResult DeleteBloodBankEntry(int id)
        {
            var entry = BloodBanksList.Find(e => e.Id == id);
            if (entry == null) { return NotFound(); }
            BloodBanksList.Remove(entry);
            return NoContent();
        }

        //paginated list of blood bank entries
        [HttpGet("page")]
        public ActionResult<IEnumerable<BloodBankEntry>>GetPaginatedEntries(int page=1,int size = 10)
        {
            if(page<=0||size<=0)
            {
                return BadRequest("page and size must be greater than 0");
            }
            var paginatedEntries = BloodBanksList.Skip((page - 1) * size).Take(size).ToList();
            return paginatedEntries.Any()?paginatedEntries:NoContent();
        }

        //Search for blood bank entries based on donorName
        [HttpGet("search/donorname")]
        public ActionResult<IEnumerable<BloodBankEntry>> SearchByDonorName(string donorName = null)
        {
            var bloodBankResults = BloodBanksList.AsQueryable();
           
            if (!string.IsNullOrEmpty(donorName))
            {
                //case insensitive and partial search
                bloodBankResults = bloodBankResults.Where(e => e.DonorName.Contains(donorName, StringComparison.OrdinalIgnoreCase));
            }
            return bloodBankResults.Any() ? bloodBankResults.ToList() : NotFound("No blood bank entries found.");
        }

        //Search for blood bank entries based on BloodType
        [HttpGet("search/bloodtype")]
        public ActionResult<IEnumerable<BloodBankEntry>> SearchByBloodType(string bloodType)
        {
            var bloodBankResults = BloodBanksList.AsQueryable();

            if (!string.IsNullOrEmpty(bloodType))
            {
                //case insensitive 
                bloodBankResults = bloodBankResults.Where(e => e.BloodType.Equals(bloodType, StringComparison.OrdinalIgnoreCase));
            }
            else
            {

                return BadRequest("bloodType parameter is required.");

            }
            return bloodBankResults.ToList();
        }

        //Search for blood bank entries based on status
        [HttpGet("search/status")]
        public ActionResult<IEnumerable<BloodBankEntry>> SearchByStatus(string status)

        {
            var bloodBankResults = BloodBanksList.AsQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                //case insensitive and partial search
                bloodBankResults = bloodBankResults.Where(e => e.Status.Contains(status, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
               
              return BadRequest("Status parameter is required.");
                
            }
            return bloodBankResults.Any()?bloodBankResults.ToList(): NotFound("No blood bank entries found.");
        }

        //sorting
        [HttpGet("sort")]
        public ActionResult<IEnumerable<BloodBankEntry>> SortBloodBanks(string sortBy = "BloodType", string sortOrder = "asc")
        {
            var bloodBankResults = BloodBanksList.AsQueryable();
            if (sortBy.ToLower() == "bloodtype")
            {

                bloodBankResults = sortOrder == "asc" ? bloodBankResults.OrderBy(e => e.BloodType) : bloodBankResults.OrderByDescending(e => e.BloodType);
            }
            else if (sortBy.ToLower() == "collectiondate")
            {
                bloodBankResults = sortOrder == "asc" ? bloodBankResults.OrderBy(e => e.CollectionDate) : bloodBankResults.OrderByDescending(e => e.CollectionDate);
            }
            else if (sortBy.ToLower() == "expirationdate")
            {
                bloodBankResults = sortOrder == "asc" ? bloodBankResults.OrderBy(e => e.ExpirationDate) : bloodBankResults.OrderByDescending(e => e.ExpirationDate);
            }
            else
            {
                return BadRequest("Invalid sortBy parameter. Supported values are 'bloodType', 'collectionDate', and 'expirationDate'.");
            }
            return bloodBankResults.ToList();
        }

        //Search for blood bank entries with multiple parameters
        [HttpGet("filter")]
        public ActionResult<IEnumerable<BloodBankEntry>> SearchEntries(string bloodType = null, string? status=null,string donorName = null)
        {
            var bloodBankResults = BloodBanksList.AsQueryable();
            //Search for blood bank entries based on blood type.
            if (!string.IsNullOrEmpty(bloodType))
            {
                //exact search
                bloodBankResults = bloodBankResults.Where(e => e.BloodType.Equals(bloodType, StringComparison.OrdinalIgnoreCase));
            }

            //Search for blood bank entries based on status.
            if (!string.IsNullOrEmpty(status))
            {
                //case insensitive exact search
                bloodBankResults = bloodBankResults.Where(e => e.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
            }
            
            return bloodBankResults.ToList();

        }


        
       
        //To Check Valid bloodBankEntry
        private string IsValidEntry(BloodBankEntry entry)
        {
            
            var errors = new List<string>();
            // List of valid blood types
            string[] validBloodTypes = { "A+", "A-", "B+", "B-", "AB+", "AB-", "O+", "O-" };

            // Check if the bloodType is in the valid list
            if (!validBloodTypes.Contains(entry.BloodType.ToUpper()))
            {
                errors.Add("Invalid Blood Group.");
            }
            
            //age validation
            if (entry.Age < 18 || entry.Age > 65)
            {
                errors.Add("Age must be between 18 and 65.");
            }
            //quantity validation
            if (entry.Quantity <= 0)
            {
                errors.Add("Quantity should be greater than 0.");
            }
            //Contact Validation
            if (string.IsNullOrEmpty(entry.ContactInfo) || !entry.ContactInfo.Contains('@'))
            {
                errors.Add("ContactInfo must be a valid email.");
            }
            //CollectionDate validation
            if (entry.CollectionDate > DateTime.Now)
            {
                errors.Add("CollectionDate cannot be in the future.");
            }
            //expirationdate should be greater than collectiondate
            if (entry.ExpirationDate <= entry.CollectionDate)
            {
               errors.Add("ExpirationDate must be after CollectionDate.");
            }

            //badrequest is status is other than "Available", "Requested", "Expired".
            if (entry.Status != "Available" && entry.Status != "Requested" && entry.Status != "Expired")
            {
                errors.Add("Status not valid");
            }
            return errors.Any()?string.Join("\n",errors):null;
        }

}
}
