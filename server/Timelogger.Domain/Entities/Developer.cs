using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Timelogger.Domain.Entities
{
	public class Developer
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DeveloperId { get; set; }

        [Required]
        public string DeveloperName { get; set; }

        public virtual ICollection<TimeLog> TimeLogs { get; set; }
    }
}