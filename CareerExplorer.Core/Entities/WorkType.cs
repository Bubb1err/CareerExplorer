using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareerExplorer.Core.Entities
{
    public class WorkType
    {
        public int WorkTypeId { get; set; }
        public string WorkTypeTitle { get; set;}
        public int AdminId { get; set; }
        public virtual Admin Admin { get; set; }
        public int VacancyId { get; set; }
        public virtual Vacancy Vacancy { get; set; }
    }
}
