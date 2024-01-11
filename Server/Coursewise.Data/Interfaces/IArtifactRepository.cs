using Coursewise.Data.Entities;
using Coursewise.Data.Generics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Data.Interfaces
{
    public interface IArtifactRepository : IGenericRepository<Artifact, int>
    {
    }
}
