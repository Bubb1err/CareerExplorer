using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections;

namespace CareerExplorer.Web.DTO
{
    public class ApplicantDTO :IEnumerable<ApplicantDTO>
    {
        public int Id { get; set; }
        [BindNever]
        public int VacancyId { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Phone { get; set; }
        public string? GitHub { get; set; }
        public string? Experience { get; set; }

        public IEnumerator<ApplicantDTO> GetEnumerator()
        {
            yield return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
