using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_Project.Models;

public partial class Template
{
    public int TemplateId { get; set; }

    public string TemplateName { get; set; } = null!;

    public string TemplateCategory { get; set; } = null!;

    public string TemplateContent { get; set; } = null!;

    public string? ImagePath { get; set; }

    [NotMapped]
    public IFormFile TemplateImage { get; set; }
}
