using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FleetTechCore.DTOs.Views
{
    public record struct FileView (
    int Id,
    byte[] Data,
    string FileName,
    string MimeType
    );
}
