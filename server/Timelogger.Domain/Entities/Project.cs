using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Timelogger.Domain.Entities
{
	public class Project
	{
		[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProjectId { get; set; }

        [Required]
        public string ProjectName { get; set; }

        [Required]
        [ForeignKey("ProjectStage")]
        public int ProjectStageId { get; set; }

        [Required]
        public DateTime Deadline { get; set; }

        [Required]
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual ProjectStage ProjectStage { get; set; }

        public virtual ICollection<TimeLog> TimeLogs { get; set; }
    }
}
