using System.ComponentModel;

namespace Condorcet.B2.AspnetCore.MVC.Application.Core.Domain
{
    public class Project
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        [DisplayName("Date limite")]
        public DateTime? Deadline { get; set; }
    }
}
