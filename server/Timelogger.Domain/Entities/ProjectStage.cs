using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Timelogger.Domain.Entities
{
	public class ProjectStage
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProjectStageId { get; set; }

        [Required]
        public string ProjectStageName { get; set; }

        public virtual ICollection<Project> Projects { get; set; }
    }
}