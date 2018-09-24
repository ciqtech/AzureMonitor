using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LabApplicationBackendLayer.Models
{
    public class Employee
    {
        [JsonProperty(PropertyName = "id")]
        public string EmpId { get; set; }
        [Required]
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; set; }
        [Required]
        [JsonProperty(PropertyName = "Gender")]
        public string Gender { get; set; }
        [Required]
        [JsonProperty(PropertyName = "Department")]
        public string Department { get; set; }
        [Required]
        [JsonProperty(PropertyName = "City")]
        public string City { get; set; }
    }
}