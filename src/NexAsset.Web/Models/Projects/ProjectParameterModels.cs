using System;
using System.Collections.Generic;

namespace NexAsset.Web.Models.Projects;

public class ParameterSectionModel
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public int DisplayOrder { get; set; }
    public List<ParameterFieldModel> Parameters { get; set; } = new();
}

public class ParameterFieldModel
{
    public Guid Id { get; set; }
    public Guid SectionId { get; set; }
    public string ParameterName { get; set; } = string.Empty;
    public ParameterInputType InputType { get; set; } = ParameterInputType.Text;
    public string? Unit { get; set; }
    public bool IsRequired { get; set; }
    public int DisplayOrder { get; set; }
    public string? DropdownOptionsJson { get; set; }
    public string? CurrentValue { get; set; }
}

public class ParameterValueItemModel
{
    public Guid ParameterId { get; set; }
    public string? Value { get; set; }
}
