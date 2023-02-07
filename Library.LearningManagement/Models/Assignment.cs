using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.LearningManagement.Models
{
    public class Assignment
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal totalAvailablePoints { get; set; }
        public DateTime DueDate { get; set; }

        public Assignment()
        {
            Name = string.Empty;
            Description = string.Empty;
            totalAvailablePoints= 0;
            DueDate = DateTime.Now;
        }
        public override string ToString()
        {
            return $"[{Name}] {Description} - {DueDate}";
        }
    }
}
