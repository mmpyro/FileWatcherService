using System;
using System.ComponentModel.DataAnnotations;

namespace EntityPlugin
{
    public class File
    {
        [Key]
        public DateTime ModifiedDateTime { get; set; }
        [Required]
        public string FullPath { get; set; }
        [Required]
        public string  ChangeType { get; set; }
        public string NewName { get; set; }
        public string OldName { get; set; }
    }
}