using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace EmontiOptometrist.Web.Models
{
    public class FAQItem
    {
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Question { get; set; }

        [Required]
        public string Answer { get; set; }

        [StringLength(500)]
        public string Keywords { get; set; }

        [StringLength(100)]
        public string Category { get; set; }

        public int Priority { get; set; } = 2;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        public string[] GetKeywordsArray()
        {
            if (string.IsNullOrEmpty(Keywords))
                return new string[0];

            return Keywords.Split(',')
                          .Select(k => k.Trim().ToLower())
                          .Where(k => !string.IsNullOrEmpty(k))
                          .ToArray();
        }

        public void SetKeywordsArray(string[] keywords)
        {
            Keywords = keywords != null ? string.Join(", ", keywords) : string.Empty;
        }
    }
}
