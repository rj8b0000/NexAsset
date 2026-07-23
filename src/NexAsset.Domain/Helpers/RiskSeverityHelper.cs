using NexAsset.Domain.Enums;

namespace NexAsset.Domain.Helpers;

public static class RiskSeverityHelper
{
    public static RiskSeverity ComputeSeverity(RiskProbability p, RiskImpact i) => (p, i) switch
    {
        (RiskProbability.Low, RiskImpact.Low) => RiskSeverity.Low,
        (RiskProbability.Low, RiskImpact.Medium) => RiskSeverity.Low,
        (RiskProbability.Low, RiskImpact.High) => RiskSeverity.Medium,
        (RiskProbability.Medium, RiskImpact.Low) => RiskSeverity.Low,
        (RiskProbability.Medium, RiskImpact.Medium) => RiskSeverity.Medium,
        (RiskProbability.Medium, RiskImpact.High) => RiskSeverity.High,
        (RiskProbability.High, RiskImpact.Low) => RiskSeverity.Medium,
        (RiskProbability.High, RiskImpact.Medium) => RiskSeverity.High,
        (RiskProbability.High, RiskImpact.High) => RiskSeverity.Critical,
        _ => RiskSeverity.Low
    };
}
