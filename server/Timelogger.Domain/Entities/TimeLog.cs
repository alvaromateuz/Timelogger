using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Timelogger.Domain.Entities
{
	public class TimeLog
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TimeLogId { get; set; }

		[Required]
        [ForeignKey("Project")]
        public int ProjectId { get; set; }

        [Required]
        [ForeignKey("Developer")]
        public int DeveloperId { get; set; }

        [Required]
        public DateTime LogDate { get; set; }

        [Required]
        public int TimeSpent { get; set; }

        public string Description { get; set; }

        public virtual Developer Developer { get; set; }

        public virtual Project Project { get; set; }
    }
}