using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.AWS.Communication.Models
{
    public class Artifact
    {
        public int ArtifactId { get; set; }

        public string StorageLocation { get; set; }

        public string FileType { get; set; }

        public string Url { get; set; }
    }
}
