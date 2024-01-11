using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coursewise.Domain.Models.Dto
{
    public class DashboardDto
    {
        public IList<JobDetaiDto> Jobs { get; set; } = new List<JobDetaiDto>();
        public IList<ResourceDetailDto> Resources { get; set; } = new List<ResourceDetailDto>();

    }

}
