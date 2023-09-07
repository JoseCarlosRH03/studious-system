using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetTechCore.Models.Supply
{
    public class Material: AuditableEntity
    {
        public required string  CodeCategory    { get; set; }
        public required int     Category        { get; set; }
        public required string  CodeSubCategory { get; set; }
        public required int     SubCategory     { get; set; }
        public required string  CodeMaterial    { get; set; }
        public required int     Materials       { get; set; }
        public string           Description     { get; set; }
        public int              Status          { get; set; }
    }
}
